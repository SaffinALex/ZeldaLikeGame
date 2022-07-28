using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideCameraBehavior : MonoBehaviour
{
    [SerializeField]
    private float cameraLerpSpeed;
    [SerializeField]
    private int numberCaseSwipeUp, numberCaseSwipeDown, numberCaseSwipeLeft, numberCaseSwipeRight;
    private bool swipeUp, swipeDown, swipeLeft, swipeRight;
    private bool canMove;
    private Vector2 finalPositionSwipe;
    public BoxCollider2D[] cameraCollider;


    public void swipeToDown( )
    {
        finalPositionSwipe = new Vector2(transform.position.x , transform.position.y - numberCaseSwipeDown);
        swipeDown = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToLeft( )
    {
        finalPositionSwipe = new Vector2(transform.position.x - numberCaseSwipeLeft, transform.position.y );
        swipeLeft = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToRight( )
    {
        finalPositionSwipe = new Vector2(transform.position.x + numberCaseSwipeRight, transform.position.y);
        swipeRight = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToUp()
    {
        finalPositionSwipe = new Vector2(transform.position.x , transform.position.y + numberCaseSwipeUp);
        swipeUp = true;
        GameVariables.Instance.cameraSwipe = true;
    }

    private void Awake()
    {
        swipeDown = swipeLeft  = swipeRight = swipeDown = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void FixedUpdate()
    {

        if (GameVariables.Instance.cameraSwipe)
        {
            if (swipeDown)
            {
                transform.position += new Vector3(0, -cameraLerpSpeed, 0)*Time.deltaTime;
                GameVariables.Instance.player.SlideToPosition(new Vector2(0, -cameraLerpSpeed/480 ));
                if (transform.position.y - finalPositionSwipe.y >= -0.1 && transform.position.y - finalPositionSwipe.y <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (swipeLeft)
            {
                transform.position += new Vector3(-cameraLerpSpeed, 0, 0) * Time.deltaTime;
                GameVariables.Instance.player.SlideToPosition(new Vector2(-cameraLerpSpeed / 480, 0));
                if ( transform.position.x - finalPositionSwipe.x  >= -0.1 && transform.position.x - finalPositionSwipe.x <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (swipeRight)
            {
                transform.position += new Vector3(cameraLerpSpeed, 0, 0) * Time.deltaTime;
                GameVariables.Instance.player.SlideToPosition(new Vector2(cameraLerpSpeed / 480, 0));
                if (finalPositionSwipe.x - transform.position.x >= -0.1 && finalPositionSwipe.x - transform.position.x <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }
            else if (swipeUp)
            {
                transform.position += new Vector3(0, cameraLerpSpeed, 0) * Time.deltaTime;
                GameVariables.Instance.player.SlideToPosition(new Vector2(0, cameraLerpSpeed / 480));
                if(finalPositionSwipe.y - transform.position.y >= -0.1 && finalPositionSwipe.y - transform.position.y <= 0.1)
                {
                    transform.position = new Vector3(finalPositionSwipe.x, finalPositionSwipe.y, transform.position.z);
                }
            }

            if((Vector2)transform.position == finalPositionSwipe)
            {
                GameVariables.Instance.cameraSwipe = false;
                GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
                swipeDown = swipeLeft = swipeRight = swipeDown = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
      
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if(gameState == GameStateManager.GameState.SwipeCamera)
        {
            foreach (BoxCollider2D b in cameraCollider) b.enabled = false;
        }
        else if (gameState == GameStateManager.GameState.Playing)
        {
            foreach (BoxCollider2D b in cameraCollider) b.enabled = true;
        }
    }
}
