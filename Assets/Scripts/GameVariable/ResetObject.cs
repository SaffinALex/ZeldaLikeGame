using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    private Vector3 initialPosition;
    public bool mustBeReset;
    void OnBecameInvisible()
    {
        if (mustBeReset)
        {
            this.transform.position = initialPosition;
            GameVariables.TriggerEventByName("ResetObject" + gameObject.GetInstanceID().ToString());
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    // Update is called once per frame
    void Awake()
    {
        initialPosition = this.transform.position;
    }

}
