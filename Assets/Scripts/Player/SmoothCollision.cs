using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCollision : MonoBehaviour
{
    [SerializeField]
    private bool colliderUpLeftTouching, colliderUpRightTouching, colliderDownLeftTouching, colliderDownRightTouching;

    [SerializeField]
    private float playerSmoothMovementSpeed;



    public void UpdateCollisionStatus(InteractiveCollider.Position position)
    {
        switch (position)
        {
            case InteractiveCollider.Position.DownLeft:
                colliderDownLeftTouching = true;
                break;
            case InteractiveCollider.Position.DownRight:
                colliderDownRightTouching = true;
                break;
            case InteractiveCollider.Position.UpLeft:
                colliderUpLeftTouching = true;
                break;
            case InteractiveCollider.Position.UpRight:
                colliderUpRightTouching = true;
                break;
        }
    }

    public void UpdateCollisionStatusWhenExit(InteractiveCollider.Position position)
    {
        switch (position)
        {
            case InteractiveCollider.Position.DownLeft:
                colliderDownLeftTouching = false;
                break;
            case InteractiveCollider.Position.DownRight:
                colliderDownRightTouching = false;
                break;
            case InteractiveCollider.Position.UpLeft:
                colliderUpLeftTouching = false;
                break;
            case InteractiveCollider.Position.UpRight:
                colliderUpRightTouching = false;
                break;
        }
    }
    #region lifetime
    public void Awake()
    {

    }
    public void Start()
    {
        InteractiveCollider.collisionEvent.AddListener(UpdateCollisionStatus);
        InteractiveCollider.collisionEventExit.AddListener(UpdateCollisionStatusWhenExit);
    }
    public void FixedUpdate()
    {
        if (colliderDownLeftTouching && !colliderDownRightTouching)
        {
            if (GameVariables.Instance.player.LookDown())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookLeft())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, playerSmoothMovementSpeed));
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else if (!colliderDownLeftTouching && colliderDownRightTouching)
        {
            if (GameVariables.Instance.player.LookDown())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(-playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookRight())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, playerSmoothMovementSpeed));
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else if (!colliderUpRightTouching && colliderUpLeftTouching)
        {

            if (GameVariables.Instance.player.LookUp())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookLeft())
            {
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, -playerSmoothMovementSpeed));
            }
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else if (colliderUpRightTouching && !colliderUpLeftTouching)
        {
            if (GameVariables.Instance.player.LookUp())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(-playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookRight())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, -playerSmoothMovementSpeed));
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else
        {
            GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        if ((colliderUpLeftTouching && colliderDownLeftTouching) || (colliderDownRightTouching && colliderUpRightTouching))
        {
            GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
            //  GameVariables.Instance.player.SetPositionToOld();
        }
    }

    public void Update()
    {

    }
    #endregion
}
