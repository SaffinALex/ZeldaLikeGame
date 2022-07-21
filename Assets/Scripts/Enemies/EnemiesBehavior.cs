using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemiesBehavior : MonoBehaviour
{
    #region Variables
    #region StatsVar
    public int damage;
    public int initialLife;
    public int currentLife { get; set; }
    public float timeRecoveryTimer { get; set; }
    public float timeRecovery;
    public float speed;
    public float rangeDetection;
    public float attackRange;

    public float pushbackSpeed;
    public bool isPushBack { get; set; }
    #endregion
    #region MovementVar
    public int caseSize;
    public int numberOfMovingCase;
    public float timeToWait;
    private float isActiveTimer;
    public float CurrentTimeToWait { get; set; }
    public Vector2 target { get; set; }
    public enum Position
    {
        Up,
        Down,
        Left,
        Right
    }
    public Position currentDirection;
    public bool canMove;
    #endregion
    #region AttackVar
    public AudioClip hitSound;
    public AudioClip deadSound;
    public UnityEvent OnAttackEvent;
    public Transform player;
    private bool playerInRange;
    #endregion
    #region SpriteVar
    public float currentTime { get; set; }
    public bool isAttracted { get; set; }

    public float timeRefresh;
    public List<Sprite> listSpriteDown;
    public List<Sprite> listSpriteUp;
    public List<Sprite> listSpriteLeft;
    public List<Sprite> listSpriteRight;
    private SpriteRenderer currentSprite;
    private int index = 0;
    public float coefWhenAttracted { get; set; }
    public Vector2 attractedVector { get; set; }
    #endregion
    #region PositionVar
    private Transform initialPos;
    #endregion

    public abstract void SetAttractedVector(Vector2 normalized, float coefWhenAttracted);
    #endregion
    // Start is called before the first frame update
    public void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        currentTime = 0;
        target = Vector2.zero;
        initialPos = transform;
        isActiveTimer = 0;
        currentLife = initialLife;
        timeRecoveryTimer = 0;
        player = GameVariables.Instance.player.transform;
    }

    public void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    // Update is called once per frame
    public void Update()
    {
        if (timeRecoveryTimer > 0) timeRecoveryTimer -= Time.deltaTime;
        if (Vector3.Distance(player.position, transform.position) <= rangeDetection)
        {
            playerInRange = true;
            target = player.position;
        }
        else
        {
            playerInRange = false;
            if (target == (Vector2)player.position)
            {
                target = Vector2.zero;
                canMove = true;
            }
        }

        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            player.gameObject.GetComponent<PlayerBehavior>().GetDamage(damage, transform);
        }
    }

    private void FixedUpdate()
    {
        if (isActiveTimer > 0) isActiveTimer -= Time.deltaTime;
        if (isPushBack)
        {
            MoveBack();
        }
        if (GetComponent<Animator>().GetBool("isRecoveryTime") && timeRecoveryTimer <= 0)
        {
            GetComponent<Animator>().SetBool("isRecoveryTime", false);
            isPushBack = false;
        }
        if (!isPushBack && isActiveTimer <= 0)
        {
            currentTime += Time.deltaTime;
            CurrentTimeToWait += Time.deltaTime;
            if (CurrentTimeToWait >= timeToWait) canMove = true;
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
            if (!playerInRange) MoveRandomly();
            else MoveToPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        target = Vector2.zero;
    }

    public void ChangeNpcDirection()
    {
        Position oldDirection = currentDirection;
        Vector2 dir = ((Vector2)transform.position - target).normalized;
        if (dir.y <= 0 && dir.x <= 0)
        {
            if (dir.x > dir.y) currentDirection = Position.Up;
            else currentDirection = Position.Right;
        }
        else if (dir.y >= 0 && dir.x >= 0)
        {
            if (dir.x < dir.y) currentDirection = Position.Down;
            else currentDirection = Position.Left;
        }
        else if (dir.y >= 0 && dir.x <= 0)
        {
            if (dir.x > dir.y) currentDirection = Position.Right;
            else currentDirection = Position.Down;
        }
        else if (dir.y <= 0 && dir.x >= 0)
        {
            if (dir.x < dir.y) currentDirection = Position.Up;
            else currentDirection = Position.Left;
        }
        if (currentDirection != oldDirection) currentTime = timeRefresh;
    }

    public abstract void MoveRandomly();

    public abstract void MoveToPlayer();

    public abstract void GetDamage(int dmg);
    public abstract void  MoveBack();
    public void SetIsActive(float time)
    {
        isActiveTimer = time;
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pause || gameState == GameStateManager.GameState.Talking) enabled = false;
        else if (gameState == GameStateManager.GameState.Playing) enabled = true;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Update is calle
    #region debug
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (playerInRange) Gizmos.color = new Color(1, 0, 0, 0.5f);
        else Gizmos.color = new Color(0, 1, 0, 0.5f);

        Gizmos.DrawSphere(transform.position, rangeDetection);
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
    #endif
    #endregion
}
