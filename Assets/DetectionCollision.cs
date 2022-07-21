using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private PushingObject objectToPush;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && objectToPush.IsPushing)
        {
            objectToPush.IsPushing = false;
        }
    }
}
