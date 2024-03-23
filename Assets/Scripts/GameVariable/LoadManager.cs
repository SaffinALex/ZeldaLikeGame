using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager: MonoBehaviour
{
    public GameObject player;
    public CameraController followCamera;
    private CameraController instantianteFollowCamera;
    public HouseTPData mapData;
    private string actualSceneName;
    public void LoadLevel()
    {
        GameVariables.Instance.player = Instantiate(player).GetComponent<BrainBehavior>();
        instantianteFollowCamera = Instantiate(followCamera);
        instantianteFollowCamera.player = GameVariables.Instance.player.transform;
        GameVariables.Instance.LoadLevel(mapData);
    }

    public void LoadLevel(HouseTPData data)
    {
        GameVariables.Instance.player.transform.position = data.playerSpawnPosition;
/*        instantianteFollowCamera.maxPosition = data.maxPosition;
        instantianteFollowCamera.minPosition = data.minPosition;*/
        if (actualSceneName != null)
        {
            SceneManager.UnloadSceneAsync(actualSceneName);
        }

        SceneManager.LoadScene(data.sceneName, LoadSceneMode.Additive);
        actualSceneName = data.sceneName;
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
/*        instantianteFollowCamera.SetBoxColliderActive(data.boxColliderIsActive);*/
    }
}
