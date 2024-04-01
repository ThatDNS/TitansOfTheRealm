using Fusion.Addons.ConnectionManagerAddon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRoom : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerCount;
    public Button joinButton;

    ConnectionManager connectionManager;

    private void Start()
    {
        connectionManager = FindObjectOfType<ConnectionManager>();

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    public void JoinSession()
    {
        if (connectionManager == null)
        {
            Debug.LogError("No connection manager in the scene. Can not join session.");
            return;
        }

        connectionManager.ConnectToRoom(roomName.text);
    }
}
