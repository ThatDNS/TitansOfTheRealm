using Fusion;
using Fusion.Addons.ConnectionManagerAddon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] GameObject roomGameObject;

    [SerializeField] Button refreshButton;
    [SerializeField] Transform sessionListContent;

    private ConnectionManager connectionManager;

    private void Start()
    {
        connectionManager = FindObjectOfType<ConnectionManager>();
    }

    public void CreateRoom()
    {
        Debug.Log("Creating room with name " + roomName.text);
        connectionManager.ConnectToRoom(roomName.text);
    }

    public void RefreshSessions()
    {
        StartCoroutine(RefreshWait());
    }

    private IEnumerator RefreshWait()
    {
        // Prevents spamming
        refreshButton.interactable = false;

        // Refresh the sessions list on UI
        RefreshSessionUI();

        yield return new WaitForSeconds(0.5f);

        refreshButton.interactable = true;
    }

    private void RefreshSessionUI()
    {
        // Clear the session / room list UI
        foreach (Transform child in sessionListContent)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Updating UI with " + connectionManager.sessions.Count + " sessions!");
        foreach (SessionInfo session in connectionManager.sessions)
        {
            if (session.IsVisible)
            {
                GameObject roomEntry = Instantiate(roomGameObject, sessionListContent);
                GameRoom gameRoom = roomEntry.GetComponent<GameRoom>();
                gameRoom.roomName.text = session.Name;
                gameRoom.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                gameRoom.joinButton.interactable = ((session.IsOpen) && (session.PlayerCount < session.MaxPlayers));
            }
        }
    }
}
