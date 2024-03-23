using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameVariables.TriggerEvent;

public class GameVariables : MonoBehaviour
{
    public bool cameraSwipe { get; set; }
    public BrainBehavior player { get; set; }

    public static List<TriggerEvent> triggerEventList = new List<TriggerEvent>();
    public AudioSource gameAudioSource { get; set; }
    public LoadManager loadManager;
    public UserInterfaceManager userInterfaceManager;
    public AudioSource musicPlayer;
    public bool pauseGame { get; set; }
    public GameObject dialogueBox { get; set; }
    public static GameVariables instance = null;
    public InventoryBehavior inventory { get; set; }

    public static GameVariables Instance
    {
        get
        {
            return instance;
        }
    }

    public bool StopPlayer { get; set; }

    public bool switchIsRed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject); //This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager
            return;
        }
        GameStateManager.Instance.SetState(GameStateManager.GameState.Pause);
        gameAudioSource = GetComponent<AudioSource>();
        userInterfaceManager.LoadUI();
        loadManager.LoadLevel();
        userInterfaceManager.LoadInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.Playing)
            {
                GameStateManager.Instance.SetState(GameStateManager.GameState.Pause);
                pauseGame = true;
                if (player.CarryObject != null)
                {
                    player.CarryObject.GetComponent<ICarryObject>().UnGrap();
                }
            }
            else if (GameStateManager.Instance.CurrentGameState == GameStateManager.GameState.Pause)
            {
                GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
                pauseGame = false;
            }

        }
    }

    public void LoadLevel(HouseTPData data)
    {
        loadManager.LoadLevel(data);
        musicPlayer.Stop();
        musicPlayer.PlayOneShot(data.ambientMusic);
        
    }

    public void CreateTriggerEvent(string name, OnEvent callback)
    {
        new TriggerEvent(name, callback);
    }
               
    public class TriggerEvent
    {
        public string eventName;
        public delegate void OnEvent();
        public OnEvent callback;


        public TriggerEvent(string name, OnEvent callback)
        {
            eventName = name;
            SetTrigger(callback);
            triggerEventList.Add(this);
        }

        public TriggerEvent(string name, Action<EnemiesBehavior> whenEnnemyFall)
        {
            eventName = name;
            SetTrigger(whenEnnemyFall);
        }

        private void SetTrigger(Action<EnemiesBehavior> whenEnnemyFall)
        {
            callback();
        }

        public void OnEventTriggered()
        {
            callback();
        }

        public void SetTrigger(OnEvent callback)
        {
            this.callback = callback;
        }
        public string GetKey()
        {
            return eventName;
        }

        public bool IsValid()
        {
            return eventName != null && eventName != "";
        }

    }


    public static void TriggerEventByName(string eventName)
    {
        
        for (int i = 0; i< triggerEventList.Count; i++)
        {
            if(triggerEventList[i].eventName == eventName)
            {
                triggerEventList[i].OnEventTriggered();
            }
        }
    }

    public static void DeleteTriggerEventByName(string eventName)
    {
        for (int i = 0; i < triggerEventList.Count; i++)
        {
            if(triggerEventList[i].eventName == eventName)
            {
                triggerEventList.RemoveAt(i);
                i--;
            }
        }
    }


}
