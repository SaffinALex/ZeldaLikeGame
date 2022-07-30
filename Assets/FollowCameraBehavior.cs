using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveOffset;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public float smoothCoeff;
    public float cameraLerpSpeed;
    private Vector2 finalPositionSwipe;
    public float distanceToSwipe;
    public Transform target;
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    public float playerSpeedSwipe;
    public float playerCaseSwipe;
    public List<BoxCollider2D> cameraCollider;
    [SerializeField]
    private int numberCaseSwipeUp, numberCaseSwipeDown, numberCaseSwipeLeft, numberCaseSwipeRight;
    private PlayerBehavior player;
    private Vector2 finalPositionPlayer;
    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.SwipeCamera)
        {
            SetBoxColliderActive(false);
        }
        else if (gameState == GameStateManager.GameState.Playing && GameStateManager.Instance.PreviousGameState == GameStateManager.GameState.SwipeCamera)
        {
            SetBoxColliderActive(true);

        }
    }
    public void SetBoxColliderActive(bool t)
    {
        foreach (BoxCollider2D b in cameraCollider) b.enabled = t;
    }
    public void SwipeCamera()
    {

    }
    private void Start()
    {
        GameVariables.Instance.CreateTriggerEvent("SwipeLeft", () => { swipeToLeft(); maxPosition.x -= moveOffset; minPosition.x -= moveOffset; });
        GameVariables.Instance.CreateTriggerEvent("SwipeRight", () => { swipeToRight(); maxPosition.x += moveOffset; minPosition.x += moveOffset; });
        GameVariables.Instance.CreateTriggerEvent("SwipeUp", () => { swipeToUp(); maxPosition.y += moveOffset; minPosition.y += moveOffset; });
        GameVariables.Instance.CreateTriggerEvent("SwipeDown", () => { swipeToDown(); maxPosition.y -= moveOffset; minPosition.y -= moveOffset; });
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        player = GameVariables.Instance.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != target.position && !GameVariables.Instance.cameraSwipe) {
            float position_x = Mathf.Clamp(target.transform.position.x, minPosition.x, maxPosition.x);
            float position_y = Mathf.Clamp(target.transform.position.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, new Vector3(position_x, position_y, transform.position.z), smoothCoeff);
        }

        if (GameVariables.Instance.cameraSwipe)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.SwipeCamera);
            transform.position = Vector2.MoveTowards(transform.position, finalPositionSwipe, cameraLerpSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -50);
            player.transform.position = Vector2.MoveTowards(player.transform.position, finalPositionPlayer, playerSpeedSwipe * Time.deltaTime);

            if ((Vector2)transform.position == finalPositionSwipe)
            {
                GameVariables.Instance.cameraSwipe = false;
                GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
                swipeDown = swipeLeft = swipeRight = swipeDown = false;
            }
        } 
    }

    public void swipeToDown()
    {
        finalPositionSwipe = new Vector2(transform.position.x, transform.position.y - numberCaseSwipeDown);
        finalPositionPlayer = new Vector2(player.transform.position.x, player.transform.position.y - playerCaseSwipe);
        swipeDown = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToLeft()
    {
        finalPositionSwipe = new Vector2(transform.position.x - numberCaseSwipeLeft, transform.position.y);
        finalPositionPlayer = new Vector2(player.transform.position.x - playerCaseSwipe, player.transform.position.y);
        swipeLeft = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToRight()
    {
        finalPositionSwipe = new Vector2(transform.position.x + numberCaseSwipeRight, transform.position.y);
        finalPositionPlayer = new Vector2(player.transform.position.x + playerCaseSwipe, player.transform.position.y );
        swipeRight = true;
        GameVariables.Instance.cameraSwipe = true;
    }
    public void swipeToUp()
    {
        finalPositionSwipe = new Vector2(transform.position.x, transform.position.y + numberCaseSwipeUp);
        finalPositionPlayer = new Vector2(player.transform.position.x, player.transform.position.y + playerCaseSwipe);
        swipeUp = true;
        GameVariables.Instance.cameraSwipe = true;
    }
}
