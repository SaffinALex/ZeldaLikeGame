using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectBorder : MonoBehaviour
{
    [SerializeField]
    public float timeToWaitToPush;

    [SerializeField]
    private BoxCollider2D verifierAdjacentCaseCollider;

    [SerializeField]
    private Collider2D mainCollider2D;

    [SerializeField]
    private bool isBorderLeft, isBorderRight, isBorderUp, isBorderDown;

    [SerializeField]
    private bool canPush;

    [SerializeField]
    private PushingObject objectToPush;

    [SerializeField]
    private float timer;

    private bool isPushed;


    public void Awake()
    {
        timer = 0;
        canPush = true;
        isPushed = false;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (verifierAdjacentCaseCollider.IsTouching(collision) && !collision.gameObject.CompareTag("BorderPushObject") && !collision.gameObject.CompareTag("PlayerBody") && !collision.gameObject.CompareTag("PlayerHead"))
        {
            canPush = false;
          
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!verifierAdjacentCaseCollider.IsTouching(collision))
        {
            canPush = true;
        }
    }

    public void Update()
    {

    }

}
