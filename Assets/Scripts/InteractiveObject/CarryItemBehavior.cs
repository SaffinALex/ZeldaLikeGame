using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryItemBehavior : MonoBehaviour
{

    private BrainBehavior player;
    #region projectile

    public int numberOfBounce;
    private int cptBounce;
    public bool isDestroyedOnGround;
    #endregion

    #region trajectory
    [Tooltip("Position we want to hit")]
    private Vector2 targetPos;
    public float distance;
    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;
    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;
    Vector2 startPos;
    private Vector2 playerDirection;
    Vector2 nextPos;
    public float numeratorAfterBouncing;

    #endregion

    #region Trajectory

    public enum CarryObjectState
    {
        inGround,
        carryByPlayer,
        launched,
        stable,
    }

    private CarryObjectState carryObjectState;

    public AudioClip hitGroundSound;
    public ShadowBehavior shadowPos;
    private Vector3 initialLaunchPos;
    public AudioClip throwObject;
    public AudioClip carryAudioSong;


    private void CalculateTrajectoryWhenLaunch()
    {
        // Compute the next position, with arc added in
        float baseY;
        float arc;
        float x0 = startPos.x;
        float x1 = targetPos.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        if (carryObjectState == CarryObjectState.stable)
        {
            nextX = targetPos.x;
            baseY = Mathf.MoveTowards(transform.position.y, targetPos.y, speed * Time.deltaTime);
            nextPos = new Vector2(nextX, baseY);
            cptBounce = numberOfBounce;
        }
        else if (dist == 0)
        {
            baseY = Mathf.MoveTowards(transform.position.y, targetPos.y, speed * Time.deltaTime);
            nextPos = new Vector2(nextX, baseY);
        }
        else if (dist != 0)
        {
            baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
            arc = (arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist));
            nextPos = new Vector2(nextX, baseY + arc);
        }


        // Do something when we reach the target
        if (nextPos == targetPos) Bouncing();
    }

    internal CarryObjectState getState()
    {
        return carryObjectState;
    }
    #endregion


    public void UnGrap()
    {
        carryObjectState = CarryObjectState.launched;
    }

    public void Bouncing()
    {
        cptBounce++;
        speed /= numeratorAfterBouncing;
        arcHeight /= numeratorAfterBouncing;
        distance /= numeratorAfterBouncing;
        GameVariables.Instance.gameAudioSource.PlayOneShot(hitGroundSound);
        if (carryObjectState != CarryObjectState.stable)
            targetPos += (distance * playerDirection);
        startPos = transform.position;


    }

    void FixedUpdate()
    {
        switch (carryObjectState)
        {
            case CarryObjectState.stable:
            case CarryObjectState.launched:
                if (cptBounce <= numberOfBounce)
                {
                    CalculateTrajectoryWhenLaunch();
                    transform.position = nextPos;
                    shadowPos.transform.position = new Vector3(nextPos.x, initialLaunchPos.y, 0);
                }
                break;
            case CarryObjectState.carryByPlayer:
                nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
                transform.position = nextPos;
                break;
        }

    }

    public void ThrowItem()
    {
        carryObjectState = CarryObjectState.launched;
        startPos = transform.position;
        playerDirection = GameVariables.Instance.player.GetMovementModule().getCurrentDirection();
        targetPos = (Vector2)this.player.transform.position + (distance * playerDirection);
        if (!player.GetBoolAnimator("IsMoving"))
        {
            targetPos = new Vector2(player.transform.position.x, player.transform.position.y);
            carryObjectState = CarryObjectState.stable;
        }
        else
        {
            GameVariables.Instance.gameAudioSource.PlayOneShot(throwObject);
        }
        player.SetBoolAnimator("isCarrying", false);
    }

    public void Initialize(BrainBehavior player)
    {
        this.player = player;
        cptBounce = 0;
        player.SetBoolAnimator("isCarrying", true);
        GameVariables.Instance.gameAudioSource.PlayOneShot(carryAudioSong);
        nextPos = new Vector2(player.transform.position.x, player.transform.position.y + 16);
        transform.position = nextPos;
        initialLaunchPos = player.transform.position;
        shadowPos.transform.SetParent(null);
        shadowPos.transform.position = new Vector3(transform.position.x, transform.position.y - 8, 0);
        transform.SetParent(null);
        carryObjectState = CarryObjectState.carryByPlayer;
    }

    public void OnDestroy()
    {
        Destroy(shadowPos.gameObject);
    }



#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(targetPos, new Vector3(5, 5, 5));
    }
#endif
}
