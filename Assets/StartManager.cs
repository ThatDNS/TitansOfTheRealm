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
}
