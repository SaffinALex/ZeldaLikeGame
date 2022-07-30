using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerEvent : MonoBehaviour
{
    public string[] eventsBeginName;
    public string[] eventsEndName;
    public bool isActive;
    private bool eventReady;
    public List<GameObject> enemiesToSpawn;
    private List<GameObject> enemiesSpawn;
    public AudioClip EnnemiesSpawn;
    private float timer;
    private bool canBeActivate;
    private bool enemyAreSpawn;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet") && isActive && canBeActivate) BeginEvent();
    }

    public void BeginEvent()
    {
        foreach(string s in eventsBeginName)
        {
            GameVariables.TriggerEventByName(s);
        }
        canBeActivate = false;
        eventReady = true;

    }
    private void Awake()
    {
        timer = 0;
        enemyAreSpawn = false;
        canBeActivate = true;
        eventReady = false;
        enemiesSpawn = new List<GameObject>();
    }
    private void Start()
    {
        GameVariables.Instance.CreateTriggerEvent("ResetDungeon", () => Reset()) ;
    }
    private void Update()
    {

         if(!enemyAreSpawn && eventReady)
        {
            foreach (GameObject e in enemiesToSpawn)
            {
                GameObject o = Instantiate(e);
                o.transform.position = new Vector2(Random.Range(transform.position.x - 32, transform.position.x + 32), Random.Range(transform.position.y - 32, transform.position.y + 32));
                enemiesSpawn.Add(o);
            }
            enemyAreSpawn = true;
        }

        if (enemyAreSpawn && eventReady && isActive)
        {
            if (enemiesSpawn.Count > 0)
            {
                for(int i = 0; i < enemiesSpawn.Count; i++)
                {
                    if (!enemiesSpawn[i].activeInHierarchy)
                    {
                        enemiesSpawn.RemoveAt(i);
                        i--;
                    }
                    i++;
                }
            }
            if (enemiesSpawn.Count == 0)
            {
                isActive = false;
                eventReady = false;
                foreach (string s in eventsEndName)
                {
                    GameVariables.TriggerEventByName(s);
                }
            }
        }
    }

    public void Reset()
    {
        isActive = true;
        eventReady = false;
        for(int i = 0; i < enemiesSpawn.Count; i++)
                {
                    if (enemiesSpawn[i].activeInHierarchy)
                    {
                        enemiesSpawn.RemoveAt(i);
                Destroy(enemiesSpawn[i]);
                        i--;
                    }
                    i++;
                }
        enemyAreSpawn = false;
        canBeActivate = true;
    }
}
