using Fusion.Addons.ConnectionManagerAddon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] ConnectionManager connectionManager;

    public bool gameStarted = false;

    public async void JoinTheNetwork()
    {
        await connectionManager.Connect();
        gameStarted = true;
    }
}
