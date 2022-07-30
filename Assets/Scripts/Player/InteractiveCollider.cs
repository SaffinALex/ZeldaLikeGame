using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveCollider : MonoBehaviour
{
    [SerializeField]
    public bool IsCollide { get; set; }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsCollide = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsCollide = false;
    }


}
