using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectBorder : MonoBehaviour
{

    [SerializeField]
    public bool CanPush;
    private List<GameObject> listCollisionObject;


    public void Awake()
    {
        CanPush = true;
        listCollisionObject = new List<GameObject>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("BorderPushObject") && !collision.gameObject.CompareTag("PlayerBody") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("PlayerFeet"))
        {
            Debug.Log(collision.gameObject.tag);
            if (!listCollisionObject.Contains(collision.gameObject.gameObject)) listCollisionObject.Add(collision.gameObject);
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("BorderPushObject") && !collision.gameObject.CompareTag("PlayerBody") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("PlayerFeet"))
        {
            Debug.Log(collision.gameObject.tag);
            if (!listCollisionObject.Contains(collision.gameObject.gameObject)) listCollisionObject.Add(collision.gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (listCollisionObject.Contains(collision.gameObject)) listCollisionObject.Remove(collision.gameObject);
    }

    private void Update()
    {
        CanPush = !(listCollisionObject.Count > 0);
    }
}
