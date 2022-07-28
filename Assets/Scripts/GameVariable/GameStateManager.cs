using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    public enum GameState
    {
        Playing,
        Pause,
        Menu,
        Talking,
        SwipeCamera
    }
    private static GameStateManager instance;
    public static GameStateManager Instance
    {
        get
        {
            if (instance == null) instance = new GameStateManager();
            return instance;
        }
    }
    public GameState CurrentGameState { get; private set; }
    public delegate void GameStateChangeHandler(GameState gameState);
    public event GameStateChangeHandler OnGameStateChanged;

    public void SetState(GameState gameState)
    {
        if (CurrentGameState == gameState) return;
        CurrentGameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
    }
}
