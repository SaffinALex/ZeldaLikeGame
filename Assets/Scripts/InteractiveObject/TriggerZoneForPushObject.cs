using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneForPushObject : MonoBehaviour
{
    [SerializeField]
    private GameObject objectTarget;
    [SerializeField]
    private string eventNameToTrigger;
    private bool zoneIsTrigger = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == objectTarget && zoneIsTrigger)
        {
            GameVariables.TriggerEventByName(eventNameToTrigger);
            zoneIsTrigger = true;
        }
    }
}
