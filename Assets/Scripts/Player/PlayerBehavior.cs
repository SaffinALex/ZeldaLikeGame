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
    private List<RespawnPos> listSavePos;
    private int indexListSavePos;
    public struct RespawnPos
    {
        public float time;
        public Vector2 position;
    }
    public GameObject A, B;
    public  bool isAttracted { get; set; }
    public Vector2 positionToRespawn { get; set; }
    public bool isFlying { get; set; }

    public float delayBetweenItemUse;
    private float timerDelayBetweenItemUse;
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
        indexListSavePos = 0;
        listSavePos = new List<RespawnPos>();
        for(int i = 0; i < 5; i++)
        {
            listSavePos.Add(new RespawnPos());
        }
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
    // Update is calle
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
            RespawnPos rp = listSavePos[indexListSavePos];
            rp.time = Time.realtimeSinceStartup;
            rp.position = transform.position;
            listSavePos[indexListSavePos] = rp;
            indexListSavePos++;
            if (indexListSavePos >= 5) indexListSavePos = 0;
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
            MoveCharacter();
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
            ReplaceDirection();
        }

        if (CanMove && !GameVariables.Instance.cameraSwipe && animator.GetBool("IsMoving") && playMovement && !isPushBack && !isAttracted)
        {
            Vector3 newPosition = transform.position + ((new Vector3(moveX, moveY, 0)  + (Vector3)smoothVectorMovement ).normalized * characterSpeed )/* Time.deltaTime)*/;

            newPosition.x = Mathf.Round(newPosition.x);
            newPosition.y = Mathf.Round(newPosition.y);
           // transform.position = newPosition;
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
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
        if (Input.GetKey(a1) && timerDelayBetweenItemUse >= delayBetweenItemUse)
        {
            if (objectA == null && A.GetComponent<CarryWeapon>() == null && A.GetComponent<EmptyWeapon>() == null)
            {
                objectA = Instantiate(A, transform);
                // objectA.transform.parent = gameObject.transform;
                objectA.GetComponent<BaseWeapon>().Activate(this);
                timerDelayBetweenItemUse = 0;
            }
            else if (objectA == null)
            {
                A.GetComponent<BaseWeapon>().Activate(this);
                timerDelayBetweenItemUse = 0;
            }
        }
    }

    private void LateUpdate()
    {
        //transform.position =new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }
    private void CheckIfCanMove()
    {
        bool t = !GameVariables.Instance.cameraSwipe &&!animator.GetBool("isGrabbing") && !animator.GetBool("isAttack") && !animator.GetBool("UseWeapon") && !animator.GetBool("isFalling") && !animator.GetBool("isInLava");
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
    public void GiveDamageAndRespawn(int damage)
    {
        if (!isFlying && CanMove)
        {
            if (timeRecoveryTimer <= 0)
            {
                currentLifePoint -= damage;
                GameVariables.Instance.inventory.SetHeart(currentLifePoint);
            }
            timeRecoveryTimer = timeRecovery;
            RespawnPos respawnPoint = listSavePos[0];
            foreach(RespawnPos rp in listSavePos)
            {
                if (rp.time < respawnPoint.time) respawnPoint = rp;
            }

            transform.position = respawnPoint.position;
            isAttracted = false;
            GameVariables.Instance.player.SetBoolAnimator("isFalling", false);
        }
    }

    public void GetDamage(int dmg, Transform target)
    {
        if (timeRecoveryTimer <= 0 && !isFlying && !GetBoolAnimator("isFalling") && CanMove)
        {
            isPushBack = true;
            currentLifePoint -= dmg;
            timeRecoveryTimer = timeRecovery;
            GameVariables.Instance.gameAudioSource.PlayOneShot(hitSound);
            GameVariables.Instance.inventory.SetHeart(currentLifePoint);
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
        if (gameState == GameStateManager.GameState.Pause || gameState == GameStateManager.GameState.Talking) enabled = false;
        else if (gameState == GameStateManager.GameState.Playing) enabled = true;
    }

    public void sinkPlayer(int dmg, float time)
    {
        if (!isFlying)
        {
            if (timeRecoveryTimer <= 0)
            {
                currentLifePoint -= dmg;
                GameVariables.Instance.inventory.SetHeart(currentLifePoint);
            }
            timeRecoveryTimer = timeRecovery;


            CanMove = false;
            StartCoroutine(WhenPlayerSink(time));
        }
    }

    public IEnumerator WhenPlayerSink(float time)
    {
        RespawnPos respawnPoint = listSavePos[0];
        foreach (RespawnPos rp in listSavePos)
        {
            if (rp.time < respawnPoint.time) respawnPoint = rp;
        }
        yield return new WaitForSeconds(time);
        transform.position = respawnPoint.position;
        GameVariables.Instance.player.SetBoolAnimator("isInLava", false);
        CanMove = true;
    }
    #region DEBUG
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

    }
#endif
    #endregion
}
