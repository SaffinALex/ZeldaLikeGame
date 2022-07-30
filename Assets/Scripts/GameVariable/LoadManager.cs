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
    private FollowCameraBehavior instantianteFollowCamera;
    public AudioClip music;
    public string sceneName;
    private string actualSceneName;
    public void LoadLevel()
    {
        Debug.Assert(player.GetComponent<PlayerBehavior>() != null, "Player no have script playerBehavior");
        GameVariables.Instance.player = Instantiate(player).GetComponent<PlayerBehavior>();
        GameVariables.Instance.player.transform.position = playerPositionOnLoad;
        instantianteFollowCamera = Instantiate(followCamera);
        instantianteFollowCamera.target = GameVariables.Instance.player.transform;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        actualSceneName = sceneName;
        GameVariables.Instance.musicPlayer.PlayOneShot(music);
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }

    public void LoadLevel(HouseTPData data)
    {
        GameVariables.Instance.player.transform.position = data.playerSpawnPosition;
        instantianteFollowCamera.maxPosition = data.maxPosition;
        instantianteFollowCamera.minPosition = data.minPosition;
        SceneManager.UnloadSceneAsync(actualSceneName);
        SceneManager.LoadScene(data.sceneName, LoadSceneMode.Additive);
        actualSceneName = data.sceneName;
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        instantianteFollowCamera.SetBoxColliderActive(data.boxColliderIsActive);
    }
}
