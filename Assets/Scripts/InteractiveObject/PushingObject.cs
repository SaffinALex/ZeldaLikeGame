using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObject : MonoBehaviour
{
    [SerializeField]
    private float objectPushSpeed;
    private bool isPushing;
    public bool IsPushing { get; set; }
    [SerializeField]
    private int numberPixelToPush;
    [SerializeField]
    private int maximumPushPossibilities;
    [SerializeField]
    private int pushCompteur = 0;

    private bool pushUp, pushDown, pushLeft, pushRight;

    private bool canPlayAudio;
    public AudioClip sound;

    private Vector2 finalPositionSwipe;
    private Vector3 initialPosition;

    public void ResetObject()
    {
        pushCompteur = 0;
        isPushing = false;
    }

    #region Push Methods
    public void PushToDown()
    {
        if (pushCompteur < maximumPushPossibilities)
        {
            finalPositionSwipe = new Vector2(transform.position.x, transform.position.y - numberPixelToPush);
            pushDown = true;
            isPushing = true;
            GameVariables.Instance.player.CanMove = false;
            initialPosition = transform.position;
            GameVariables.Instance.player.SetPushAnimator(false);
        }
    }
    public void PushToLeft()
    {
        if (pushCompteur < maximumPushPossibilities)
        {
            finalPositionSwipe = new Vector2(transform.position.x - numberPixelToPush, transform.position.y);
            pushLeft = true;
            isPushing = true;
            GameVariables.Instance.player.CanMove = false;
            GameVariables.Instance.player.SetPushAnimator(false);
            initialPosition = transform.position;
        }
    }
    public void PushToRight()
    {
        if (pushCompteur < maximumPushPossibilities)
        {
            finalPositionSwipe = new Vector2(transform.position.x + numberPixelToPush, transform.position.y);
            pushRight = true;
            isPushing = true;
            GameVariables.Instance.player.CanMove = false;
            GameVariables.Instance.player.SetPushAnimator(false);
            initialPosition = transform.position;
        }
    }
    public void PushToUp()
    {
        if (pushCompteur < maximumPushPossibilities)
        {
            finalPositionSwipe = new Vector2(transform.position.x, transform.position.y + numberPixelToPush);
            pushUp = true;
            isPushing = true;
            GameVariables.Instance.player.CanMove = false;
            GameVariables.Instance.player.SetPushAnimator(false);
            initialPosition = transform.position;
        }
    }


    #endregion

    private void OnBecameVisible()
    {
        new GameVariables.TriggerEvent("ResetObject" + gameObject.GetInstanceID().ToString(), ResetObject);
        
    }
    private void Awake()
    {
        isPushing = false;
        canPlayAudio = true;
  //      GetComponent<AudioSource>().pitch -= Time.deltaTime * 1 / objectPushSpeed;

    }
    private void FixedUpdate()
    {

        if (isPushing)
        {
            if (canPlayAudio)
            {
                GetComponent<AudioSource>().PlayOneShot(sound);
                canPlayAudio = false;
            }
            if (pushDown)
            {
                transform.position += new Vector3(0, -objectPushSpeed, 0) * Time.deltaTime;
                if (transform.position.y - finalPositionSwipe.y >= -0.1 && transform.position.y - finalPositionSwipe.y <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (pushLeft)
            {
                transform.position += new Vector3(-objectPushSpeed, 0, 0) * Time.deltaTime;
                if (transform.position.x - finalPositionSwipe.x >= -0.1 && transform.position.x - finalPositionSwipe.x <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (pushRight)
            {
                transform.position += new Vector3(objectPushSpeed, 0, 0) * Time.deltaTime;
                if (finalPositionSwipe.x - transform.position.x >= -0.1 && finalPositionSwipe.x - transform.position.x <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (pushUp)
            {
                transform.position += new Vector3(0, objectPushSpeed, 0) * Time.deltaTime;
                if (finalPositionSwipe.y - transform.position.y >= -0.1 && finalPositionSwipe.y - transform.position.y <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }

            if ((Vector2)transform.position == finalPositionSwipe)
            {
                isPushing = false;
                pushCompteur += 1;
                GameVariables.Instance.player.CanMove = true;
                pushDown = pushLeft = pushRight = pushDown = false;
                canPlayAudio = true;
            }
        }
    }
}
