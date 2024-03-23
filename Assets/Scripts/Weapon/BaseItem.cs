using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseItem : ScriptableObject
{
    public int damage;
    public Sprite icon;
    public string objectName;
    [SerializeField]
    protected double recoveryTime;
    protected BrainBehavior player;
    
    public abstract void Use(Transform userTransform, BrainBehavior brain, KeyCode associateKey);
    public double GetRecoveryTime()
    {
        return recoveryTime;
    }
}
