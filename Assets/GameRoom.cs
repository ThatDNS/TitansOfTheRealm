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
    StartManager startManager;

    private void Start()
    {
        connectionManager = FindObjectOfType<ConnectionManager>();
        startManager = FindObjectOfType<StartManager>();

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
    }

    public void JoinSession(bool isVR)
    {
        if (connectionManager == null)
        {
            Debug.LogError("No connection manager in the scene. Can not join session.");
            return;
        }

        if (startManager != null)
            startManager.ReadyForVR();

        connectionManager.ConnectToRoom(roomName.text);
    }
}
