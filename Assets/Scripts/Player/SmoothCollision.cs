using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCollision : MonoBehaviour
{
    [SerializeField]
    public InteractiveCollider colliderUpLeft, colliderUpRight, colliderDownLeft, colliderDownRight;

    [SerializeField]
    private float playerSmoothMovementSpeed;



    #region lifetime
    private void Update()
    {
        if((colliderDownLeft.IsCollide && colliderDownRight.IsCollide) || (colliderDownRight.IsCollide && colliderUpRight.IsCollide) || (colliderUpRight.IsCollide && colliderUpLeft.IsCollide) 
            || (colliderDownLeft.IsCollide && colliderUpLeft))
        {
            GameVariables.Instance.player.SetBoolAnimator("IsPushing", true);
        }
        else
        {
            GameVariables.Instance.player.SetBoolAnimator("IsPushing", false);
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

    #endregion
}
