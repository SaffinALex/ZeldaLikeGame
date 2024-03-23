using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NpcBehavior : MonoBehaviour
{
    public List<Sprite> listSpriteDown;
    public List<Sprite> listSpriteUp;
    public List<Sprite> listSpriteLeft;
    public List<Sprite> listSpriteRight;
    public float timeRefresh;
    public bool roundTrip;
    private float currentTime;
    private bool inverseRoad;
    [SerializeField]
    public List<GameObject> path;
    private int pathIndex;
    public bool isMoving;
    public int speed;
    public List<string> dialogues;

    private int index;
    private SpriteRenderer currentSprite;

    public GameObject dialogueBox;
    public float inputDelay;
    public int maximumLetterByLine;
    private float timeLeft = 0;
    private int characterIndex;
    private string text;
    public float delayToDisplayLetter;
    private float lastLetterTime = 0;
    private int nextCharacter = 1;
    private int dialogueIndex = 0;
    private string displayText = "";
    public AudioClip letterDialogue;
    public AudioClip nextDialogue;
    public AudioSource audioSource;
    public Text textUI;
    public enum Position
    {
        Up,
        Right,
        Left,
        Down
    }
    public Position baseDirection;
    private Position currentDirection;
    private Position lastDirection;
    private bool playerInRange;
    void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        inverseRoad = false;
        index = 0;
        currentTime = 0;
        currentDirection = baseDirection;
        if (path.Count > 1)
        {
            pathIndex = 0;
        }
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void Start()
    {
        dialogueBox = GameVariables.Instance.dialogueBox;
        textUI = dialogueBox.GetComponentInChildren(typeof(Text)) as Text;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Update is calle
    private void Update()
    {
        if (isMoving && path.Count > 1 && !GameVariables.Instance.pauseGame)
        {
            CheckDirectionBeforeNextPoint();
        }

        if (dialogueBox.activeInHierarchy && playerInRange)
        {
            timeLeft += Time.deltaTime;
            DisplayNpcText();
            textUI.text = displayText;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange && !dialogueBox.activeInHierarchy && dialogues.Count > 0)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Talking);
            dialogueBox.SetActive(true);
            text = dialogues[dialogueIndex];
            GameVariables.Instance.pauseGame = true;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= timeRefresh)
        {
            currentTime = 0;
            switch (currentDirection)
            {
                case Position.Down:
                    if (listSpriteDown.Count > 0)
                    {
                        if (index >= listSpriteDown.Count) index = 0;
                        currentSprite.sprite = (listSpriteDown[index]);
                        index++;
                    }
                    break;
                case Position.Up:
                    if (listSpriteUp.Count > 0)
                    {
                        if (index >= listSpriteUp.Count) index = 0;
                        currentSprite.sprite = (listSpriteUp[index]);
                        index++;
                    }
                    break;
                case Position.Right:
                    if (listSpriteRight.Count > 0)
                    {
                        if (index >= listSpriteRight.Count) index = 0;
                        currentSprite.sprite = (listSpriteRight[index]);
                        index++;
                    }
                    break;
                case Position.Left:
                    if (listSpriteLeft.Count > 0)
                    {
                        if (index >= listSpriteLeft.Count) index = 0;
                        currentSprite.sprite = (listSpriteLeft[index]);
                        index++;
                    }
                    break;
            }
        }
        if (!GameVariables.Instance.pauseGame)
        {
            if (isMoving && path.Count > 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, path[pathIndex].transform.position, speed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
             playerInRange = CheckIfPlayerCanTalk(collision.gameObject.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            playerInRange = false;
        }
    }
    private void CheckDirectionBeforeNextPoint()
    {
        if (transform.position == path[pathIndex].transform.position)
        {
            currentTime = timeRefresh;
            if (!inverseRoad)
            {
                if (pathIndex >= path.Count - 1)
                {
                    if (roundTrip)
                    {
                        inverseRoad = true;
                        pathIndex--;
                    }
                }
                else
                {
                    pathIndex++;
                }
            }
            else
            {
                if (pathIndex <= 0)
                {
                    if (roundTrip)
                    {
                        inverseRoad = false;
                        pathIndex++;
                    }
                }
                else
                {
                    pathIndex--;
                }
            }
            ChangeNpcDirection();
        }
    }
    private void ChangeNpcDirection()
    {
        Vector2 dir = (transform.position - path[pathIndex].transform.position).normalized;
        if (dir.y == 0)
        {
            if (dir.x == 1) currentDirection = Position.Left;
            else if (dir.x == -1) currentDirection = Position.Right;
        }
        else if (dir.x == 0)
        {
            if (dir.y == 1) currentDirection = Position.Down;
            else if (dir.y == -1) currentDirection = Position.Up;
        }
        else if (dir.x == -1 && dir.y == -1)
        {
            currentDirection = Position.Up;
        }
        else
        {
            currentDirection = Position.Down;
        }
    }
    #region Dialogue
    private bool CheckIfPlayerCanTalk(Transform playerTransform)
    {
        return GameVariables.instance.player.GetColliderModule().isFacingInteractive(gameObject, playerTransform.position);
    }
    private void DisplayNpcText()
    {
        lastLetterTime += Time.deltaTime;
        if (lastLetterTime >= delayToDisplayLetter)
        {
            nextCharacter++;
            lastLetterTime = 0;
            if (nextCharacter <= text.Length && displayText != text)
            {
                displayText = text.Substring(0, nextCharacter);
                GameVariables.Instance.gameAudioSource.PlayOneShot(letterDialogue);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && timeLeft >= inputDelay)
        {
            if (displayText == text)
            {
                if (dialogueIndex < dialogues.Count - 1)
                {
                    dialogueIndex++;
                    text = dialogues[dialogueIndex];
                }
                else
                {
                    GameVariables.Instance.pauseGame = false;
                    dialogueBox.SetActive(false);
                    GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
                    dialogueIndex = 0;
                    ChangeNpcDirection();
                }
                displayText = "";
                textUI.text = "";
                nextCharacter = 1;
                lastLetterTime = 0;
                timeLeft = 0;
                GameVariables.Instance.gameAudioSource.PlayOneShot(nextDialogue);
            }
            else
            {
                displayText = text;
                timeLeft = 0;

            }
        }
    }
    #endregion
    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pause) enabled = false;
        else if (gameState == GameStateManager.GameState.Playing) enabled = true;
    }
    #region Debug
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        int index = 0;
        foreach (GameObject controlPoint in path)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(controlPoint.transform.position, new Vector3(16, 16, 0));
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.red;

            Handles.color = Color.red;
            Handles.Label(path[index].transform.position, (index).ToString(), guiStyle);
            index++;
        }
        for (int i = 1; i < path.Count && path.Count > 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(path[i - 1].transform.position, path[i].transform.position);
        }

    }
    private void OnValidate()
    {
        // if (speed % 16 != 0) speed = (int)(speed / 16) * 16;
    }
#endif
    #endregion
}

