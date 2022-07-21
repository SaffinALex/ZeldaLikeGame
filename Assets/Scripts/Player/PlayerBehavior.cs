using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField]
    private int initialLifePoint;

    private int currentLifePoint;

    [SerializeField]
    private float characterSpeed;
    public float CharacterSpeed
    {
        get { return characterSpeed; }
        set { }
    }

    [SerializeField]
    private Camera mainCamera;

    private Animator animator;

    public bool CanMove { get; set; }

    private Vector3 oldPosition;

    private GameObject[] allCollision;

    public float timeRecovery;

    private float timeRecoveryTimer;
    public AudioClip hitSound;

    private float moveX, moveY;

    private bool playMovement;

    private bool isPushBack;
    public float pushbackSpeed;
    private Vector2 targetEnemy;
    private GameObject objectA, objectB;
    public InteractiveCollider colliderDownLeft, colliderDownRight, colliderUpLeft, colliderUpRight;
    private Vector2 smoothVectorMovement = new Vector2(0, 0);
    [SerializeField]
    public GameObject CarryObject;
    private Vector2 attractedVector;
    private float coefWhenAttracted;
    public GameObject A, B;
    public  bool isAttracted { get; set; }
    public Vector2 positionToRespawn { get; set; }
    public bool isInvincible { get; set; }
    // Start is called before the first frame update

        #region position
    public bool LookDown()
    {
        return animator.GetFloat("MoveY") == -1;
    }
    public bool LookUp()
    {
        return animator.GetFloat("MoveY") == 1;
    }
    public bool LookLeft()
    {
        return animator.GetFloat("MoveX") == -1;
    }
    public bool LookRight()
    {
        return animator.GetFloat("MoveX") == 1;
    }

    internal void SetAttractedVector(Vector2 normalized, float coefWhenAttracted)
    {

        attractedVector = normalized;
        this.coefWhenAttracted = coefWhenAttracted;
    }

    public bool PlayerIsMoving()
    {
        return animator.GetBool("IsMoving");
    }

    public void SetPositionToOld(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetSmoothMovement(Vector2 smoothVectorMovement)
    {
        this.smoothVectorMovement = smoothVectorMovement;
    }
    #endregion

    #region Collider Behavior

    public void SetPushAnimator(bool value)
    {
        animator.SetBool("IsPushing", value);
    }

    #endregion


    #region move
    private void MoveCharacter()
    {

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            oldPosition = transform.position;
            moveX = 0.0f;
            moveY = 0.0f;
            playMovement = true;
            animator.SetBool("IsMoving", true);


            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveY = -1.0f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveY = 1.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveX = -1.0f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveX = 1.0f;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
            animator.SetFloat("MoveY", moveY);
            animator.SetFloat("MoveX", moveX);
    }

    public void ReplacePlayer()
    {
        if (smoothVectorMovement == Vector2.zero && CanMove)
        {
            float x = Mathf.Round(transform.position.x);
            float y = Mathf.Round(transform.position.y );
            transform.position = new Vector3(x, y, transform.position.z);
           // GetComponent<Rigidbody2D>().MovePosition();
        }
    }

    public void SlideToPosition(Vector2 position)
    {
        GetComponent<Rigidbody2D>().MovePosition(transform.position + new Vector3(position.x, position.y, transform.position.z));
    }
    #endregion

    #region lifetime
    private void Awake()
    {
        CanMove = true;
        moveX = 0.0f;
        moveY = 0.0f;
        GameVariables.Instance.player = this;
        currentLifePoint = initialLifePoint;
        playMovement = false;
        oldPosition = Vector3.zero;
        CarryObject = null;
        attractedVector = Vector2.zero;
    }

    private void Update()
    {
        if (timeRecoveryTimer > 0)
        {
            timeRecoveryTimer -= Time.deltaTime;
        }
        if (timeRecoveryTimer <= 0 && isPushBack) isPushBack = false;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Update is calle
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject.FindGameObjectsWithTag("PushingObject");
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

    }
    private void FixedUpdate()
    {
        CheckIfCanMove();
        if (CanMove)
        {
            MoveCharacter();
            if (Input.GetKey(KeyCode.A))
            {
                if (objectA == null && A.GetComponent<Sword>()== null && A.GetComponent<CarryWeapon>() == null && A.GetComponent<EmptyWeapon>() == null)
                {
                    objectA = Instantiate(A, transform);
                   // objectA.transform.parent = gameObject.transform;
                    objectA.GetComponent<BaseWeapon>().Activate(this);
                }
                else if (objectA == null)
                {
                    A.GetComponent<BaseWeapon>().Activate(this);
                }
            }
            if (Input.GetKey(KeyCode.B))
            {
                if (objectB == null && A.GetComponent<Sword>() == null && B.GetComponent<CarryWeapon>() == null && B.GetComponent<EmptyWeapon>() == null)
                {
                    objectB = Instantiate(B, transform);
                    // objectA.transform.parent = gameObject.transform;
                    objectB.GetComponent<BaseWeapon>().Activate(this);
                }
                else if (objectB == null)
                {
                    B.GetComponent<BaseWeapon>().Activate(this);
                }
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
        if (!animator.GetBool("IsMoving"))
        {
            animator.SetBool("IsPushing", false);
            ReplaceDirection();
        }
        if (CanMove && !GameVariables.Instance.cameraSwipe && animator.GetBool("IsMoving") && playMovement && !isPushBack && !isAttracted) 
        {
            Vector3 newPosition = transform.position + ((new Vector3(moveX, moveY, 0) + (Vector3)smoothVectorMovement).normalized * characterSpeed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
            playMovement = false;
        }
        else if (isPushBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy, -pushbackSpeed * Time.deltaTime);
        }
        if (isAttracted && !isInvincible)
        {
            Vector3 newPosition = attractedVector;
              if(animator.GetBool("IsMoving"))
              {
                  newPosition += (new Vector3(moveX, moveY, 0) * characterSpeed * coefWhenAttracted * Time.deltaTime);
                  playMovement = false;
              } 
           GetComponent<Rigidbody2D>().MovePosition(newPosition); 
           // transform.position = newPosition;
        }
        if(isInvincible){
            isAttracted = false;
        }

    }

    private void CheckIfCanMove()
    {
        bool t = !GameVariables.Instance.cameraSwipe &&!animator.GetBool("isGrabbing") && !animator.GetBool("isAttack") && !animator.GetBool("UseWeapon");
        CanMove = t;
    }
    public Vector2 GetVectorMove()
    {
        return new Vector2(moveX, moveY);
    }
    public void ReplaceDirection()
    {
        if (moveX != 0 && moveY != 0)
        {
            if (animator.GetFloat("MoveY") <= -1 || animator.GetFloat("MoveY") >= 1)
            {
                animator.SetFloat("MoveX", 0);
                moveX = 0;
            }
            else
            {
                animator.SetFloat("MoveY", 0);
                moveY = 0;
            }
        }
    }
    public void ChangePlayerOrientation(Vector2 v)
    {
        animator.SetFloat("MoveY", v.y);
        animator.SetFloat("MoveX", v.x);
    }
    #endregion

    #region Damage
    public void GiveDamageAndRespawn(int damage, Vector2 oldPosition)
    {
        if (!isInvincible)
        {
            if (timeRecoveryTimer <= 0)
            {
                currentLifePoint -= damage;
            }
            timeRecoveryTimer = timeRecovery;
            transform.position = positionToRespawn;
        }
    }

    public void GetDamage(int dmg, Transform target)
    {
        if (timeRecoveryTimer <= 0 && !isInvincible)
        {
            isPushBack = true;
            currentLifePoint -= dmg;
            timeRecoveryTimer = timeRecovery;
            GameVariables.Instance.gameAudioSource.PlayOneShot(hitSound);
            targetEnemy = target.position;
        }
    }
    #endregion

    public void SetBoolAnimator(string variableName, bool value)
    {
        animator.SetBool(variableName, value);
    }
    public bool GetBoolAnimator(string variableName)
    {
        return animator.GetBool(variableName);
    }
    public string GetWeaponATag()
    {
        if (A != null) return A.tag;
        else return "";
    }
    public string GetWeaponBTag()
    {
        if (B != null) return B.tag;
        else return "";
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pause) enabled = false;
        else if (gameState == GameStateManager.GameState.Playing) enabled = true;
    }

}
