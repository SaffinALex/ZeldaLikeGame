using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour
{
    private bool isInCollision;
    public List<string> collisionTag;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string s in collisionTag)
        {
            if (collision.CompareTag(s))
            {
                isInCollision = true;
                break;
            }
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string s in collisionTag)
        {
            if (collision.CompareTag(s) )
            {
                isInCollision = true;
                break;
            }
        }
    }
    public bool IsTrigger()
    {
        return isInCollision;
    }

}


