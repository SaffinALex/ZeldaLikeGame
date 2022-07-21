using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectBorder : MonoBehaviour
{
    [SerializeField]
    public float timeToWaitToPush;

    [SerializeField]
    private BoxCollider2D verifierAdjacentCaseCollider;

    [SerializeField]
    private Collider2D mainCollider2D;

    [SerializeField]
    private bool isBorderLeft, isBorderRight, isBorderUp, isBorderDown;

    [SerializeField]
    private bool canPush;

    [SerializeField]
    private PushingObject objectToPush;

    [SerializeField]
    private float timer;

    private bool isPushed;


    public void Awake()
    {
        timer = 0;
        canPush = true;
        isPushed = false;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (verifierAdjacentCaseCollider.IsTouching(collision) && !collision.gameObject.CompareTag("BorderPushObject") && !collision.gameObject.CompareTag("PlayerBody") && !collision.gameObject.CompareTag("PlayerHead"))
        {
            canPush = false;
          
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!verifierAdjacentCaseCollider.IsTouching(collision))
        {
            canPush = true;
        }
    }

    public void Update()
    {
        PlayerBehavior player = GameVariables.Instance.player;

        bool playerCollideWithBlock;
        if (isBorderUp)
            playerCollideWithBlock = (player.colliderDownLeft.IsTriggerWithCollision(mainCollider2D) && player.colliderDownRight.IsTriggerWithCollision(mainCollider2D));
        else if (isBorderRight)
            playerCollideWithBlock = (player.colliderDownLeft.IsTriggerWithCollision(mainCollider2D) && player.colliderUpLeft.IsTriggerWithCollision(mainCollider2D));
        else if (isBorderLeft)
            playerCollideWithBlock = (player.colliderDownRight.IsTriggerWithCollision(mainCollider2D) && player.colliderUpRight.IsTriggerWithCollision(mainCollider2D));
        else if (isBorderDown)
            playerCollideWithBlock = (player.colliderUpLeft.IsTriggerWithCollision(mainCollider2D) && player.colliderUpRight.IsTriggerWithCollision(mainCollider2D));
        else
            playerCollideWithBlock = false;


        if (timer >= timeToWaitToPush && canPush)
        {
            

            timer = 0;
            if (isBorderDown)
                objectToPush.PushToUp();
            else if (isBorderLeft)
                objectToPush.PushToRight();
            else if (isBorderRight)
                objectToPush.PushToLeft();
            else if (isBorderUp)
                objectToPush.PushToDown();
        }
        else if (GameVariables.Instance.player.PlayerIsMoving() && playerCollideWithBlock && canPush)
        {
            isPushed = true;
            GameVariables.Instance.player.SetPushAnimator(true);
            //TO DO A REFAIRE
            if (true)
            {
                if (isBorderDown && GameVariables.Instance.player.LookUp() && GameVariables.Instance.player.GetComponent<Animator>().GetBool("IsMoving"))
                    timer += Time.deltaTime;
                else if (isBorderLeft && GameVariables.Instance.player.LookRight() && GameVariables.Instance.player.GetComponent<Animator>().GetBool("IsMoving"))
                    timer += Time.deltaTime;
                else if (isBorderRight && GameVariables.Instance.player.LookLeft() && GameVariables.Instance.player.GetComponent<Animator>().GetBool("IsMoving"))
                    timer += Time.deltaTime;
                else if (isBorderUp && GameVariables.Instance.player.LookDown() && GameVariables.Instance.player.GetComponent<Animator>().GetBool("IsMoving"))
                    timer += Time.deltaTime;
                else
                {
                    timer = 0;
                    GameVariables.Instance.player.SetPushAnimator(false);
                }
            }
        }
        else if(isPushed)
        {
            GameVariables.Instance.player.SetPushAnimator(false);
            isPushed = false;
        }
        else if (!GameVariables.Instance.player.PlayerIsMoving())
        {
            timer = 0;
        }


    }

}
