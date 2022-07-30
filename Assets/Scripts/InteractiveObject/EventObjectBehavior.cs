using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectBehavior : MonoBehaviour
{
    public string eventName;
    public AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        GameVariables.Instance.CreateTriggerEvent(eventName + "Close", () => { GetComponent<Animator>().SetBool("Close", true); GameVariables.Instance.gameAudioSource.PlayOneShot(sound); gameObject.SetActive(true); });
        GameVariables.Instance.CreateTriggerEvent(eventName + "Open", () => { GetComponent<Animator>().SetBool("Open", true); GameVariables.Instance.gameAudioSource.PlayOneShot(sound);gameObject.SetActive(false); });
        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        GameVariables.DeleteTriggerEventByName(eventName + "Close");
        GameVariables.DeleteTriggerEventByName(eventName + "Open");
    }
}
