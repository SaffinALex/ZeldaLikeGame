using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerEvent : MonoBehaviour
{
    public UnityEvent eventWhenFished;
    public UnityEvent eventWhenStart;
    public bool isActive;
    private bool eventReady;
    public List<GameObject> enemiesToSpawn;
    private List<GameObject> enemiesSpawn;
    public AudioClip DoorClosed;
    public AudioClip DoorOpen;
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
        eventWhenStart?.Invoke();
        canBeActivate = false;
        eventReady = true;
        GameVariables.Instance.gameAudioSource.PlayOneShot(DoorClosed);

    }
    private void Awake()
    {
        timer = 0;
        enemyAreSpawn = false;
        canBeActivate = true;
        eventReady = false;
        enemiesSpawn = new List<GameObject>();
    }
    private void Update()
    {
        if (eventReady && timer <= DoorClosed.length)
        {
            timer += Time.deltaTime;
        }
        else if(timer >= DoorClosed.length && !enemyAreSpawn)
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
                eventWhenFished?.Invoke();
                GameVariables.Instance.gameAudioSource.PlayOneShot(DoorOpen);
            }
        }
    }

    public void Reset()
    {
        isActive = true;
        eventReady = false;
        foreach (GameObject e in enemiesSpawn)
        {
            if (e == null)
            {
                enemiesSpawn.Remove(e);
            }
        }
        enemyAreSpawn = false;
        canBeActivate = true;
    }
}
