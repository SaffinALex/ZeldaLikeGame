using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModularLib;
using System;

public class BrainBehavior : Asset
{
    BinaryCondition isPlayerMoving;
    BinaryCondition isPushing;
    BinaryCondition isHitByEnemyCondition;
    BinaryCondition isHitByFluidCondition;
    VectorCondition onPlayerDirection;

    public MovementModule movementModule;
    public AnimationModule animationModule;
    public ColliderModule colliderModule;
    public DamageModule damageModule;
    public WeaponModule weaponLeft;
    public WeaponModule weaponRight;

    internal GameObject CarryObject;
    internal bool isAttracted;
    internal bool isFlying;

    public GameObject A { get; internal set; }
    public GameObject B { get; internal set; }

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

        isPlayerMoving = new BinaryCondition();
        isPlayerMoving.CreateNewCondition();

        isPushing = new BinaryCondition();
        isPushing.CreateNewCondition();

        isHitByEnemyCondition = new BinaryCondition();
        isHitByEnemyCondition.CreateNewCondition();

        isHitByFluidCondition = new BinaryCondition();
        isHitByFluidCondition.CreateNewCondition();

        onPlayerDirection = new VectorCondition();
        onPlayerDirection.CreateNewCondition();




        movementModule.isMoving.RegisterNewCondition(isPlayerMoving);
        movementModule.directionData.RegisterNewCondition(onPlayerDirection);

        damageModule.IsHitByEnemy.RegisterNewCondition(isHitByEnemyCondition);
        damageModule.IsHitByFluid.RegisterNewCondition(isHitByFluidCondition);

        colliderModule.isPushing.RegisterNewCondition(isPushing);


        BindCallbackOnEvent(isPlayerMoving.OnEventChangedToTrue, animationModule.OnPlayerStartMoving);
        BindCallbackOnEvent(isPlayerMoving.OnEventChangedToFalse, animationModule.OnPlayerStopMoving);

        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToTrue, animationModule.OnPlayerIsHitByFluid);
        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToFalse, animationModule.OnPlayerIsNotHitByFluid);

        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToTrue, movementModule.OnPlayerIsHitByEnemy);
        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToFalse,  movementModule.OnPlayerIsNotHitByEnemy);


        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToTrue, movementModule.OnPlayerIsHitByEnemy);
        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToFalse, movementModule.OnPlayerIsNotHitByEnemy);

        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToTrue, weaponLeft.WhenPlayerIsHit);
        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToTrue, weaponRight.WhenPlayerIsHit);
        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToFalse, weaponRight.WhenPlayerIsNotHit);
        BindCallbackOnEvent(isHitByFluidCondition.OnEventChangedToFalse, weaponLeft.WhenPlayerIsNotHit);


        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToTrue, weaponLeft.WhenPlayerIsHit);
        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToTrue, weaponRight.WhenPlayerIsHit);
        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToFalse, weaponRight.WhenPlayerIsNotHit);
        BindCallbackOnEvent(isHitByEnemyCondition.OnEventChangedToFalse, weaponLeft.WhenPlayerIsNotHit);




        BindCallbackOnEvent(onPlayerDirection.onVectorRight, animationModule.OnPlayerGoRight);
        BindCallbackOnEvent(onPlayerDirection.onVectorLeft, animationModule.OnPlayerGoLeft);
        BindCallbackOnEvent(onPlayerDirection.onVectorDown, animationModule.OnPlayerGoDown);
        BindCallbackOnEvent(onPlayerDirection.onVectorUp, animationModule.OnPlayerGoUp);

        BindCallbackOnEvent(onPlayerDirection.onVectorRight, colliderModule.OnPlayerGoRight);
        BindCallbackOnEvent(onPlayerDirection.onVectorLeft, colliderModule.OnPlayerGoLeft);
        BindCallbackOnEvent(onPlayerDirection.onVectorDown, colliderModule.OnPlayerGoDown);
        BindCallbackOnEvent(onPlayerDirection.onVectorUp, colliderModule.OnPlayerGoUp);

        BindCallbackOnEvent(isPushing.OnEventChangedToTrue, animationModule.OnPushStart);
        BindCallbackOnEvent(isPushing.OnEventChangedToFalse, animationModule.OnPushStop);

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

