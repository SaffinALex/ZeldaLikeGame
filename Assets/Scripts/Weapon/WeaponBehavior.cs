using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    protected BrainBehavior player;
    protected KeyCode associateKey;
    protected int damage;

    public abstract void Activate();

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        Activate();
    }

    public void initialize(KeyCode associateKey, BrainBehavior brain, int damage)
    {
        this.associateKey = associateKey;
        player = brain;
        this.damage = damage;
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {

        if (gameState == GameStateManager.GameState.Pause || gameState == GameStateManager.GameState.Talking) enabled = false;
        if (gameState == GameStateManager.GameState.Playing)
        {
            enabled = true;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
}
