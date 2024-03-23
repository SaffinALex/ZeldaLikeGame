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
    private int numberUnitToPush;
    [SerializeField]
    private int maximumPushPossibilities;
    [SerializeField]
    private int pushCompteur = 0;
    public float timeForPush;
    private float timer;
    //private bool canBePush;

    public AudioClip sound;
    public PushingObjectBorder borderLeft, borderRight, borderUp, borderDown;
    private Vector2 targetPositionSwipe;

    public void ResetObject()
    {
        pushCompteur = 0;
        isPushing = false;
    }

    #region Push Methods

    #endregion

    private void OnBecameVisible()
    {
        new GameVariables.TriggerEvent("ResetObject" + gameObject.GetInstanceID().ToString(), ResetObject);
        
    }
    private void Awake()
    {
        isPushing = false;

    }
    private void FixedUpdate()
    {
        if (GameVariables.Instance.player.GetBoolAnimator("IsPushing") && pushCompteur < maximumPushPossibilities && !isPushing)
        {
            
            timer += Time.deltaTime;
            if(timer >= timeForPush && !isPushing)
            {
                Vector2 distance = (GameVariables.Instance.player.transform.position - transform.position).normalized;
                timer = 0;
               
                
                if(distance.x >= -1.0f && distance.x < -0.7f && borderRight.CanPush)
                {
                    targetPositionSwipe = new Vector2(transform.position.x + numberUnitToPush, transform.position.y);
                    isPushing = true;
                    GameVariables.Instance.StopPlayer = true;
                    GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
                }
                else if (distance.x <= 1.0f && distance.x > 0.7f && borderLeft.CanPush)
                {
                    targetPositionSwipe = new Vector2(transform.position.x - numberUnitToPush, transform.position.y);
                    isPushing = true;
                    GameVariables.Instance.StopPlayer = true;
                    GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
                }
                else if (distance.y <= 1.0f && distance.y > 0.5f && borderDown.CanPush)
                {
                    targetPositionSwipe = new Vector2(transform.position.x, transform.position.y - numberUnitToPush);
                    isPushing = true;
                    GameVariables.Instance.StopPlayer = true;
                    GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
                }
                else if (distance.y >= -1.0f && distance.y < 0.5f && borderUp.CanPush)
                {
                    targetPositionSwipe = new Vector2(transform.position.x, transform.position.y + numberUnitToPush);
                    isPushing = true;
                    GameVariables.Instance.StopPlayer = true;
                    GameVariables.Instance.gameAudioSource.PlayOneShot(sound);
                }
            }
        }
        else if (isPushing)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPositionSwipe, objectPushSpeed);
            if ((Vector2)transform.position == targetPositionSwipe)
            {
                isPushing = false;
                GameVariables.Instance.StopPlayer = false;
                pushCompteur += 1;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
