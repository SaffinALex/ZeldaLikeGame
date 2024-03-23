using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularLib;
using System;
using UnityEngine.Tilemaps;

public class ColliderModule : ModuleBehavior
{
    private Asset brains;

    [SerializeField]
    public InteractiveCollider colliderUpLeft, colliderUpRight, colliderDownLeft, colliderDownRight;

    [SerializeField]
    private float playerSmoothMovementSpeed;

    public BinaryData isPushing;

    private Vector2 currentDirection;
    private bool canPush;


    public override void InitializeModule()
    {
        brains = GetComponent<BrainBehavior>();
        isPushing = new BinaryData(brains);
        canPush = false;
    }

    public override void UpdateModule()
    {
        Vector2 pushingDirection = Vector2.zero;
        isPushing.Value = canPush && ((colliderDownLeft.IsCollide && colliderDownRight.IsCollide) || (colliderDownRight.IsCollide && colliderUpRight.IsCollide) || (colliderUpRight.IsCollide && colliderUpLeft.IsCollide) || (colliderDownLeft.IsCollide && colliderUpLeft.IsCollide));
    }


    public bool isFacing(GameObject obj, Vector3 boxPosition)
    {
        Vector2 directionToObj = (obj.transform.position - boxPosition).normalized;
        float dotProduct = Vector2.Dot(this.currentDirection, directionToObj); // take absolute value
        // If the dot product is close to 1, it means the player is facing the object
        canPush = dotProduct > 0.6f;

 
        return dotProduct > 0.6f;
    }

    public void isFacingTile(Vector3 tilePosition, Vector3 boxPosition)
    {
        Vector2 directionToObj = (tilePosition - boxPosition).normalized;
        float dotProduct = Vector2.Dot(this.currentDirection, directionToObj);
        // If the dot product is close to 1, it means the player is facing the object
        canPush = dotProduct > 0.6f;

    }

    public bool isFacingInteractive(GameObject obj, Vector3 boxPosition)
    {
        Vector2 directionToObj = (obj.transform.position - boxPosition).normalized;
        float dotProduct = Vector2.Dot(this.currentDirection, directionToObj); // take absolute value
        // If the dot product is close to 1, it means the player is facing the object

        return dotProduct > 0.6f;
    }


    public void OnPlayerGoRight()
    {
        currentDirection = Vector2.right;
    }

    public void OnPlayerGoLeft()
    {
        currentDirection = Vector2.left;
    }

    public void OnPlayerGoUp()
    {
        currentDirection = Vector2.up;
    }

    public void OnPlayerGoDown()
    {
        currentDirection = Vector2.down;
    }



}
