using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveCollider : MonoBehaviour
{
    [System.Serializable]
    public class myCollisionEvent : UnityEvent<Position>
    {

    }

    public static myCollisionEvent collisionEvent;
    public static myCollisionEvent collisionEventExit;
    public enum Position
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public Position position;
    private List<Collider2D> collisions;

    public List<Collider2D> collisionsTriggered { get; private set; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collisionsTriggered.Contains(collision) && collision.CompareTag("PushingObject"))
        {
            collisionsTriggered.Add(collision);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collisionsTriggered.Contains(collision))
        {
            collisionsTriggered.Remove(collision);

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        collisionEvent.Invoke(position);
        if (!collisions.Contains(collision.collider))
        {
            collisions.Add(collision.collider);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collisions.Contains(collision.collider))
        {
            collisions.Add(collision.collider);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionEventExit.Invoke(position);
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
    }

    public bool IsCollideWithCollision(Collider2D collision)
    {
        return collisions.Contains(collision);
    }

    public bool IsTriggerWithCollision(Collider2D collision)
    {
        return collisionsTriggered.Contains(collision);
    }

    public bool IsCollideWithSomething()
    {
        return collisions.Count > 0;
    }

    public void Awake()
    {
        collisions = new List<Collider2D>();
        collisionsTriggered = new List<Collider2D>();
        collisionEvent = new myCollisionEvent();
        collisionEventExit = new myCollisionEvent();
    }

}
