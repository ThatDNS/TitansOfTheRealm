using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkStart : MonoBehaviour
{
    public NetworkRunner _runner;
    [SerializeField] private GameMode _gameMode;

    //async void StartGame(GameMode mode)
    //{
    //    // let it know that we will be providing user input
    //    _runner.ProvideInput = true;

    //    // Create the NetworkSceneInfo from the current scene
    //    var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
    //    var sceneInfo = new NetworkSceneInfo();
    //    if (scene.IsValid)
    //    {
    //        sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
    //    }

    //    // Start or join (depends on gamemode) a session with a specific name
    //    await _runner.StartGame(new StartGameArgs()
    //    {
    //        GameMode = mode,
    //        SessionName = "TestRoom",
    //        Scene = scene,
    //        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
    //    });
    //}

    //private void Start()
    //{
    //    StartGame(_gameMode);
    //}
}
