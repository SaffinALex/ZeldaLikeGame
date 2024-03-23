using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularLib;
using System;

public class BrainBehavior : Asset
{
    private BinaryCondition isPlayerMoving;
    private BinaryCondition isPushingCondition;
    private BinaryCondition isHitByEnemyCondition;
    private BinaryCondition isHitByFluidCondition;
    private BinaryCondition isJumpingCondition;

    private BinaryData isJumping;

    VectorCondition onPlayerDirection;

    public MovementModule movementModule;
    public AnimationModule animationModule;
    public ColliderModule colliderModule;
    public DamageModule damageModule;
    public WeaponModule weaponLeft;
    public WeaponModule weaponRight;

    internal GameObject CarryObject;
    internal bool isAttracted;

    public GameObject A { get; internal set; }
    public GameObject B { get; internal set; }
    public BinaryCondition IsPlayerMoving { get => isPlayerMoving; set => isPlayerMoving = value; }
    public BinaryCondition IsPushingCondition { get => isPushingCondition; set => isPushingCondition = value; }
    public BinaryCondition IsHitByEnemyCondition { get => isHitByEnemyCondition; set => isHitByEnemyCondition = value; }
    public BinaryCondition IsHitByFluidCondition { get => isHitByFluidCondition; set => isHitByFluidCondition = value; }
    public BinaryCondition IsJumpingCondition { get => isJumpingCondition; set => isJumpingCondition = value; }
    public bool IsJumping { get => isJumping.Value; set => isJumping.Value = value; }

    public void Awake()
    {
        movementModule.InitializeModule();
        colliderModule.InitializeModule();
        damageModule.InitializeModule();

        weaponLeft.InitializeModule();
        weaponRight.InitializeModule();

        conditionsCallback = new Dictionary<ModularEvent, List<Callback>>();
    }
    public void Start()
    {

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        isJumping = new BinaryData(this);

        IsPlayerMoving = new BinaryCondition();
        IsPlayerMoving.CreateNewCondition();

        IsJumpingCondition = new BinaryCondition();
        IsJumpingCondition.CreateNewCondition();

        IsHitByEnemyCondition = new BinaryCondition();
        IsHitByEnemyCondition.CreateNewCondition();

        IsHitByFluidCondition = new BinaryCondition();
        IsHitByFluidCondition.CreateNewCondition();

        onPlayerDirection = new VectorCondition();
        onPlayerDirection.CreateNewCondition();

        IsJumpingCondition = new BinaryCondition();
        IsJumpingCondition.CreateNewCondition();


        movementModule.isMoving.RegisterNewCondition(IsPlayerMoving);
        movementModule.directionData.RegisterNewCondition(onPlayerDirection);
        isJumping.RegisterNewCondition(isJumpingCondition);

        damageModule.IsHitByEnemy.RegisterNewCondition(IsHitByEnemyCondition);
        damageModule.IsHitByFluid.RegisterNewCondition(IsHitByFluidCondition);



        BindCallbackOnEvent(IsPlayerMoving.OnEventChangedToTrue, animationModule.OnPlayerStartMoving);
        BindCallbackOnEvent(IsPlayerMoving.OnEventChangedToFalse, animationModule.OnPlayerStopMoving);

        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToTrue, animationModule.OnPlayerIsHitByFluid);
        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToFalse, animationModule.OnPlayerIsNotHitByFluid);

        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToTrue, movementModule.OnPlayerIsHitByEnemy);
        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToFalse,  movementModule.OnPlayerIsNotHitByEnemy);


        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToTrue, movementModule.OnPlayerIsHitByEnemy);
        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToFalse, movementModule.OnPlayerIsNotHitByEnemy);

        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToTrue, weaponLeft.WhenPlayerIsHit);
        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToTrue, weaponRight.WhenPlayerIsHit);
        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToFalse, weaponRight.WhenPlayerIsNotHit);
        BindCallbackOnEvent(IsHitByFluidCondition.OnEventChangedToFalse, weaponLeft.WhenPlayerIsNotHit);

        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToTrue, weaponRight.WhenPlayerIsHit);
        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToFalse, weaponRight.WhenPlayerIsNotHit);

        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToTrue, animationModule.SwitchJumpAnimation);
        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToFalse, animationModule.SwitchJumpAnimation);

        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToTrue, damageModule.SwitchInvinciblePropertie);
        BindCallbackOnEvent(IsJumpingCondition.OnEventChangedToFalse, damageModule.SwitchInvinciblePropertie);


        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToTrue, weaponLeft.WhenPlayerIsHit);
        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToTrue, weaponRight.WhenPlayerIsHit);
        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToFalse, weaponRight.WhenPlayerIsNotHit);
        BindCallbackOnEvent(IsHitByEnemyCondition.OnEventChangedToFalse, weaponLeft.WhenPlayerIsNotHit);


        BindCallbackOnEvent(onPlayerDirection.onVectorRight, animationModule.OnPlayerGoRight);
        BindCallbackOnEvent(onPlayerDirection.onVectorLeft, animationModule.OnPlayerGoLeft);
        BindCallbackOnEvent(onPlayerDirection.onVectorDown, animationModule.OnPlayerGoDown);
        BindCallbackOnEvent(onPlayerDirection.onVectorUp, animationModule.OnPlayerGoUp);

        BindCallbackOnEvent(onPlayerDirection.onVectorRight, colliderModule.OnPlayerGoRight);
        BindCallbackOnEvent(onPlayerDirection.onVectorLeft, colliderModule.OnPlayerGoLeft);
        BindCallbackOnEvent(onPlayerDirection.onVectorDown, colliderModule.OnPlayerGoDown);
        BindCallbackOnEvent(onPlayerDirection.onVectorUp, colliderModule.OnPlayerGoUp);

    /*    BindCallbackOnEvent(IsPushing.OnEventChangedToTrue, animationModule.OnPushStart);
        BindCallbackOnEvent(IsPushing.OnEventChangedToFalse, animationModule.OnPushStop);*/

    }

    public MovementModule GetMovementModule()
    {
        return movementModule;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

     public void GetDamage(int damage, Transform transform)
    {
        damageModule.GetDamage(damage, transform);
    }

    internal bool isFacing(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    public void FixedUpdate()
    {
        movementModule.FixedUpdateModule();
        damageModule.FixedUpdateModule();

    }

    public void Update()
    {
        movementModule.UpdateModule();
        colliderModule.UpdateModule();
        damageModule.UpdateModule();

        weaponRight.UpdateModule();
        weaponLeft.UpdateModule();
    }

    internal void SetBoolAnimator(string v1, bool v2)
    {
        animationModule.SetBoolAnimator(v1, v2);
    }

    internal bool GetBoolAnimator(string v)
    {
        return animationModule.GetBoolAnimator(v);
    }

    public ColliderModule GetColliderModule()
    {
        return colliderModule;
    }

    public AnimationModule GetAnimationModule()
    {
        return animationModule;
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {

        if (gameState == GameStateManager.GameState.SwipeCamera)
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

}

