using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    private Vector3 initialPosition;
    public bool mustBeReset;
    private bool reset;
    void OnBecameInvisible()
    {
        if (mustBeReset) reset = true;
    }

    // Update is called once per frame
    void Awake()
    {
        initialPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (reset)
        {
            this.transform.position = initialPosition;
            GameVariables.TriggerEventByName("ResetObject"+gameObject.GetInstanceID().ToString());
            GameVariables.DeleteTriggerEventByName("ResetObject" + this.GetInstanceID().ToString());
            reset = false;
        }
    }
}
