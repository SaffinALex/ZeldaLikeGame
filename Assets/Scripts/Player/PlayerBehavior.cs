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

    private int indexListSavePos;

    public GameObject A, B;
    public  bool isAttracted { get; set; }
    public Vector2 positionToRespawn { get; set; }
    public bool isFlying { get; set; }

    public float delayBetweenItemUse;
    private float timerDelayBetweenItemUse;

    private Vector2 currentDirection;

    // Start is called before the first frame update
    #region position


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
        this.smoothVectorMovement = smoothVectorMovement.normalized;
    }
    #endregion

    #region Collider Behavior

    public void SetPushAnimator(bool value)
    {
        animator.SetBool("IsPushing", value);
    }

    #endregion


    #region move
    #endregion

    #region lifetime
    private void Awake()
    {
        CanMove = true;
        moveX = 0.0f;
        moveY = 0.0f;
        GameVariables.Instance.player = GetComponent<BrainBehavior>();

        playMovement = false;
        oldPosition = Vector3.zero;
        CarryObject = null;
        attractedVector = Vector2.zero;
     
        timerDelayBetweenItemUse = 0;
    }

    private void Update()
    {
        if (timeRecoveryTimer > 0)
        {
            timeRecoveryTimer -= Time.deltaTime;
        }
        if (timeRecoveryTimer <= 0 && isPushBack) isPushBack = false;
        timerDelayBetweenItemUse += Time.deltaTime;

        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Round(newPosition.x);
        newPosition.y = Mathf.Round(newPosition.y);
      //  transform.position = newPosition;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    // Update is called
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject.FindGameObjectsWithTag("PushingObject");
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameVariables.Instance.inventory.SetMaxVisibleHeart(initialLifePoint);

    }
    private void FixedUpdate()
    {
        CheckIfCanMove();

        if (CanMove && !GetBoolAnimator("isJumping"))
        {
         
        }

        if(GetBoolAnimator("isJumping") || GetBoolAnimator("isInLava")  || GetBoolAnimator("isFalling"))
        {
            if(CarryObject != null)
            {
                CarryObject.GetComponent<CarryItemBehavior>().UnGrap();
            }
        }

        if (CanMove)
        {
            UseItem(KeyCode.A, objectA, A);
            UseItem(KeyCode.B, objectB, B);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (!animator.GetBool("IsMoving"))
        {
            animator.SetBool("IsPushing", false);
        }

        if (CanMove && !GameVariables.Instance.cameraSwipe && animator.GetBool("IsMoving") && playMovement && !isPushBack && !isAttracted)
        {

            playMovement = false;
        }
        else if (isPushBack && !GetBoolAnimator("isInLava"))
        {
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy, -pushbackSpeed * Time.deltaTime);
        }

        if (isAttracted && !isFlying)
        {
            Vector3 newPosition = attractedVector;
              if(animator.GetBool("IsMoving"))
              {
                 newPosition += (new Vector3(moveX, moveY, 0) * characterSpeed * Time.deltaTime);
                 playMovement = false;
              } 
           transform.position = newPosition;
        }

        if(isFlying){
            isAttracted = false;
        }

    }

    private void UseItem(KeyCode a1, GameObject objectA, GameObject A)
    {
/*        if (Input.GetKey(a1) && timerDelayBetweenItemUse >= delayBetweenItemUse && objectA == null)
        {
            //On active l'OBJECT instancié
            if (A.GetComponent<CarryWeapon>() == null && A.GetComponent<EmptyWeapon>() == null)
            {
                objectA = Instantiate(A, transform);
                // objectA.transform.parent = gameObject.transform;
                objectA.GetComponent<BaseWeapon>().Activate(this);
            }
            //On Active un EFFET.
            else
            {
                A.GetComponent<BaseWeapon>().Activate(this);
            }
            timerDelayBetweenItemUse = 0;

        }*/
    }

    private void CheckIfCanMove()
    {
        bool t =  !GameVariables.Instance.StopPlayer && !animator.GetBool("isGrabbing") && !animator.GetBool("isAttack") && !animator.GetBool("UseWeapon") && !animator.GetBool("isFalling") && !animator.GetBool("isInLava");
        CanMove = t;
    }
    public Vector2 GetVectorMove()
    {
        return new Vector2(moveX, moveY);
    }
    public void ChangePlayerOrientation(Vector2 v)
    {
        animator.SetFloat("MoveY", v.y);
        animator.SetFloat("MoveX", v.x);
    }
    #endregion

    #region Damage

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

        if(gameState == GameStateManager.GameState.SwipeCamera)
        {
            GameVariables.Instance.StopPlayer = true;
        }

        if (gameState == GameStateManager.GameState.Pause || gameState == GameStateManager.GameState.Talking) enabled = false;
        if (gameState == GameStateManager.GameState.Playing)
        {
            enabled = true;
            GameVariables.Instance.StopPlayer = false;
        }
    }


    #region DEBUG
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

    }
#endif
    #endregion
}
