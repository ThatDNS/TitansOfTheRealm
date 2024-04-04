using Fusion.Addons.ConnectionManagerAddon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartManager : MonoBehaviour
{
    [SerializeField] GameObject pcLobbyCanvas;
    [SerializeField] GameObject vrLobbyCanvas;
    [SerializeField] GameObject temporaryVRRig;
    [SerializeField] GameObject vrHardwareRig;
    [SerializeField] ConnectionManager connectionManager;

    [SerializeField] GameSettlement gameOverCanvas;

    public bool isGameOver = false;

    private void Start()
    {
        isGameOver = false;
        gameOverCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (connectionManager.isConnected && pcLobbyCanvas.activeInHierarchy)
            pcLobbyCanvas.SetActive(false);
    }

    public void ReadyForVR()
    {
        vrLobbyCanvas.SetActive(false);
        temporaryVRRig.SetActive(false);
        vrHardwareRig.SetActive(true);
    }

    public void ShowVRDeathCanvas()
    {
        gameOverCanvas.gameObject.SetActive(isGameOver);
        gameOverCanvas.UpdateMessage("PC");
    }
}
