using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class InteractiveCollider : MonoBehaviour
{

    public Tilemap tilemap;

    public float offset_x = 8.0f;
    public float offset_y = 16.0f;
    private GameObject targetObject;
    private Vector2 targetVector;
    [SerializeField]
    public bool IsCollide { get; set; }
    public ColliderModule colliderModule;

    private bool exitCollision;

    private void Awake()
    {
        offset_x = 8.0f;
        offset_y = 16.0f;
    }
    private void FixedUpdate()
    {
        if (exitCollision)
        {
            exitCollision = false;
            IsCollide = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IsCollide = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IsCollide = true;

        Vector3 hitPosition = Vector3.zero;

        tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (tilemap != null)
        {

            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                int x = tilemap.WorldToCell(hitPosition).x;
                int y = tilemap.WorldToCell(hitPosition).y;
                targetVector = (new Vector2(x, y) * 16) + new Vector2(offset_x, offset_y);
                colliderModule.isFacingTile(targetVector, transform.position);
            }
        }
        else
        {
            colliderModule.isFacing(targetObject = collision.gameObject, gameObject.transform.position);
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        exitCollision = true;
    }

    #region DEBUG
    private void OnDrawGizmos()
    {
        if (targetObject != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Rouge semi-transparent
            Gizmos.DrawCube(targetObject.transform.position, new Vector3(16, 16, 0));
        }

        if(targetVector != null)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.5f); // bleu semi-transparent
            Gizmos.DrawCube(targetVector, new Vector3(16, 16, 0));
        }
    }
    #endregion

}
