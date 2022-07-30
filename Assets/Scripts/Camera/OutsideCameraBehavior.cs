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
