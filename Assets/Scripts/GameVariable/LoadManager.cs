using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager: MonoBehaviour
{
    public GameObject player;
    public Vector2 playerPositionOnLoad;
    public FollowCameraBehavior followCamera;
    public string sceneName;
    public void LoadLevel()
    {
        Debug.Assert(player.GetComponent<PlayerBehavior>() != null, "Player no have script playerBehavior");
        GameVariables.Instance.player = Instantiate(player).GetComponent<PlayerBehavior>();
        GameVariables.Instance.player.transform.position = playerPositionOnLoad;
        FollowCameraBehavior f = Instantiate(followCamera);
        f.target = GameVariables.Instance.player.transform;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }
}
