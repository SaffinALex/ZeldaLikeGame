using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public string eventName;
    public AudioClip sound;
    private bool isDisable;
    public bool onlyOneTime;
    private void Awake()
    {
        GameVariables.Instance.CreateTriggerEvent(eventName, () =>
        {
            if (!isDisable)
            {
                GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
                else gameObject.SetActive(true);
                if(onlyOneTime) isDisable = true;
            }
        });
        GameVariables.Instance.CreateTriggerEvent("ResetDungeon", () => { if (!isDisable) gameObject.SetActive(false); });
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameVariables.DeleteTriggerEventByName(eventName);
        GameVariables.DeleteTriggerEventByName("ResetDungeon");
    }
}
