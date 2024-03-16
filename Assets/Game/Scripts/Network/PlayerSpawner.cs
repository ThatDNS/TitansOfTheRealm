using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    // TODO: Separate runner from PlayerSpawner
    private NetworkRunner _runner;
    
    public string sessionName = "GameRoom";
    public bool playerSpawned = false;

    [SerializeField] GameMode _gameMode;
    [SerializeField] NetworkPrefabRef warriorPrefab;
    [SerializeField] NetworkPrefabRef titanPrefab;
    [SerializeField] NetworkPrefabRef titanLeftHand;
    [SerializeField] NetworkPrefabRef titanRightHand;

    [SerializeField] ForestSpawner forestSpawner;
    NetworkObject warriorObj = null;
    NetworkObject titanObj = null;
    NetworkObject titanLeftHandObj = null;
    NetworkObject titanRightHandObj = null;

    private void Awake()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
    }

    private void Start()
    {
        playerSpawned = false;
        StartGame(_gameMode);
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
            return;

        if (player.PlayerId == 2)
        {
            warriorObj = runner.Spawn(warriorPrefab, new Vector3(2.45f, 2.75f, 11.75f), Quaternion.Euler(0f, 180f, 0f), player);
        }
        else
        {
            titanObj = runner.Spawn(titanPrefab, new Vector3(5.25f, -6, -15.75f), Quaternion.identity, player);
            titanLeftHandObj = runner.Spawn(titanLeftHand, Vector3.zero, Quaternion.identity, player);
            titanRightHandObj = runner.Spawn(titanRightHand, Vector3.zero, Quaternion.identity, player);
            titanLeftHandObj.GetComponent<HandPresencePhysics>().titan = titanObj.transform;
            titanLeftHandObj.GetComponent<HandPresencePhysics>().FindTitanHand();
            titanRightHandObj.GetComponent<HandPresencePhysics>().titan = titanObj.transform;
            titanRightHandObj.GetComponent<HandPresencePhysics>().FindTitanHand();
        }
        playerSpawned = true;
        forestSpawner.SpawnTrees(runner);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (player.PlayerId == 2)
        {
            if (warriorObj != null)
            {
                runner.Despawn(warriorObj);
            }
        }
        else
        {
            if (titanObj != null)
            {
                runner.Despawn(titanObj);
            }
            if (titanLeftHandObj != null)
            {
                runner.Despawn(titanLeftHandObj);
            }
            if (titanRightHandObj != null)
            {
                runner.Despawn(titanRightHandObj);
            }
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
