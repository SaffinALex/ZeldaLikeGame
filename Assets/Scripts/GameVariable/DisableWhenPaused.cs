using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWhenPaused : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.Pause) enabled = false;
        else if (gameState == GameStateManager.GameState.Playing) enabled = true;
    }
}
