using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoorBehavior : MonoBehaviour
{
    public string eventName;
    public bool isOpen;
    public AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        GameVariables.Instance.CreateTriggerEvent(eventName + "Close", () => { GetComponent<Animator>().SetBool("Close", true); GameVariables.Instance.gameAudioSource.PlayOneShot(sound); gameObject.SetActive(true); isOpen = false; });
        GameVariables.Instance.CreateTriggerEvent(eventName + "Open", () => { GetComponent<Animator>().SetBool("Open", true); GameVariables.Instance.gameAudioSource.PlayOneShot(sound);gameObject.SetActive(false); isOpen = true; });
        gameObject.SetActive(!isOpen);
    }


    private void OnDestroy()
    {
        GameVariables.DeleteTriggerEventByName(eventName + "Close");
        GameVariables.DeleteTriggerEventByName(eventName + "Open");
    }
}
