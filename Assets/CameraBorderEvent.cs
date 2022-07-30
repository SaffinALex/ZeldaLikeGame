using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderEvent : MonoBehaviour
{
    public string eventName;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameVariables.Instance.cameraSwipe)
        {
            GameVariables.Instance.cameraSwipe = true;
            GameVariables.TriggerEventByName(eventName);
        }
    }
}
