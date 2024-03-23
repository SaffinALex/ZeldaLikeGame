using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCollision : MonoBehaviour
{
/*    [SerializeField]
    public InteractiveCollider colliderUpLeft, colliderUpRight, colliderDownLeft, colliderDownRight;

    [SerializeField]
    private float playerSmoothMovementSpeed;



    #region lifetime
    private void Update()
    {
        GameVariables.Instance.player.SetBoolAnimator("IsPushingDown", false);
        GameVariables.Instance.player.SetBoolAnimator("IsPushingRight", false);
        GameVariables.Instance.player.SetBoolAnimator("IsPushingUp", false);
        GameVariables.Instance.player.SetBoolAnimator("IsPushingLeft", false);
        GameVariables.Instance.player.SetBoolAnimator("IsPushing", false);

        if (GameVariables.Instance.player.GetBoolAnimator("IsMoving"))
        {
            if ((colliderDownLeft.IsCollide && colliderDownRight.IsCollide))
            {
                GameVariables.Instance.player.SetBoolAnimator("IsPushingLeft", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingRight", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingUp", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingDown", true);
                GameVariables.Instance.player.SetBoolAnimator("IsPushing", true);
            }
            if (colliderDownRight.IsCollide && colliderUpRight.IsCollide)
            {
                GameVariables.Instance.player.SetBoolAnimator("IsPushingDown", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingLeft", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingUp", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingRight", true);
                GameVariables.Instance.player.SetBoolAnimator("IsPushing", true);
            }
            if (colliderUpRight.IsCollide && colliderUpLeft.IsCollide)
            {
                GameVariables.Instance.player.SetBoolAnimator("IsPushingDown", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingRight", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingLeft", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingUp", true);
                GameVariables.Instance.player.SetBoolAnimator("IsPushing", true);
            }
            if (colliderDownLeft.IsCollide && colliderUpLeft.IsCollide)
            {
                GameVariables.Instance.player.SetBoolAnimator("IsPushingDown", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingRight", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingUp", false);
                GameVariables.Instance.player.SetBoolAnimator("IsPushingLeft", true);
                GameVariables.Instance.player.SetBoolAnimator("IsPushing", true);
            }
        }

        
    }
    public void FixedUpdate()
    {
        if (colliderDownLeft.IsCollide && !colliderDownRight.IsCollide)
        {
            if (GameVariables.Instance.player.LookDown())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookLeft())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, playerSmoothMovementSpeed));
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else if (!colliderDownLeft.IsCollide && colliderDownRight.IsCollide)
        {
            if (GameVariables.Instance.player.LookDown())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(-playerSmoothMovementSpeed, 0));
            else if (GameVariables.Instance.player.LookRight())
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, playerSmoothMovementSpeed));
            else
                GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
        else if (!colliderUpRight.IsCollide && colliderUpLeft.IsCollide)
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
        else if (colliderUpRight.IsCollide && !colliderUpLeft.IsCollide)
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
        if ((colliderUpLeft.IsCollide && colliderDownLeft.IsCollide) || (colliderDownRight.IsCollide && colliderUpRight.IsCollide))
        {
            GameVariables.Instance.player.SetSmoothMovement(new Vector2(0, 0));
        }
    }
*//*
    #endregion*/
}
