using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularLib
{
    public class Asset : MonoBehaviour
    {
        public delegate void Callback();
        internal Dictionary<ModularEvent, List<Callback>> conditionsCallback;
        public void TriggerCallbackForCondition(ModularEvent condition)
        {
            foreach (Callback callback in conditionsCallback[condition])
            {
                callback();
            }
        }

        public void BindCallbackOnEvent(ModularEvent modularEvent, Callback callback)
        {
            // Si l'événement n'est pas déjà dans le dictionnaire, ajoutez-le avec une nouvelle liste de callbacks
            if (!conditionsCallback.ContainsKey(modularEvent))
            {
                conditionsCallback[modularEvent] = new List<Callback>();
            }

            // Ajoutez le callback à la liste des callbacks pour cet événement
            conditionsCallback[modularEvent].Add(callback);
        }
    }


    public class ModularEvent
    {
        public void Throw(ref Asset asset)
        {
            asset.TriggerCallbackForCondition(this);
        }
    }
    public abstract class Data
    {
        private Asset assetRef;
        public List<Condition> conditions;

        public Asset AssetRef
        {
            get { return assetRef; }
            set { if (this.assetRef != value) { this.assetRef = value; } }
        }

        public void OnValueChanged()
        {
            foreach(Condition c in conditions)
            {
                ModularEvent eventToThrow = c.ChooseEvent(this);
                eventToThrow.Throw(ref assetRef);
            }
        }

        public void RegisterNewCondition(Condition condition)
        {
            conditions.Add(condition);
        }

        public void UnRegisterNewCondition(Condition condition)
        {
            conditions.Remove(condition);
        }
    }

    public class BinaryData : Data
    {
        private bool value;

        public bool Value
        {
            get { return value; }
            set { if (this.value != value) { this.value = value; OnValueChanged(); } }
        }

        public BinaryData(Asset brain)
        {
            this.AssetRef = brain;
            conditions = new List<Condition>();
        }
    }

    public class VectorData : Data
    {
        private Vector2 value;

        public Vector2 Value
        {
            get { return value; }
            set { if (this.value != value) { this.value = value; OnValueChanged(); } }
        }

        public VectorData(Asset brain)
        {
            this.AssetRef = brain;
            conditions = new List<Condition>();
        }
    }

    public abstract class Condition
    {
        public abstract ModularEvent ChooseEvent(Data data);
        public abstract void CreateNewCondition();
    }

    public class BinaryCondition : Condition
    {
        public ModularEvent OnEventChangedToTrue;
        public ModularEvent OnEventChangedToFalse;
        public override ModularEvent ChooseEvent(Data data)
        {
            return ((BinaryData)data).Value ? OnEventChangedToTrue : OnEventChangedToFalse;
        }
        public override void CreateNewCondition()
        {
            OnEventChangedToTrue = new ModularEvent();
            OnEventChangedToFalse = new ModularEvent();
        }
    }

    public class VectorCondition : Condition
    {

        public ModularEvent onVectorUp;
        public ModularEvent onVectorDown;
        public ModularEvent onVectorLeft;
        public ModularEvent onVectorRight;
        public override ModularEvent ChooseEvent(Data data)
        {
            ModularEvent modularEvent = new ModularEvent();
            if(((VectorData)data).Value == Vector2.down)
            {
                modularEvent = onVectorDown;
            }
            else if (((VectorData)data).Value == Vector2.up)
            {
                modularEvent = onVectorUp;
            }
            else if (((VectorData)data).Value == Vector2.right)
            {
                modularEvent = onVectorRight;
            }
            else if (((VectorData)data).Value == Vector2.left)
            {
                modularEvent = onVectorLeft;
            }
            return modularEvent;
        }

        public override void CreateNewCondition()
        {
            onVectorUp = new ModularEvent();
            onVectorDown = new ModularEvent();
            onVectorLeft = new ModularEvent();
            onVectorRight = new ModularEvent();

        }
    }

}
