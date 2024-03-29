using Fusion.Addons.ConnectionManagerAddon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartManager : MonoBehaviour
{
    [SerializeField] GameObject vrHardwareRig;
    [SerializeField] GameObject pcHardwareRig;
    [SerializeField] ConnectionManager connectionManager;
    [SerializeField] GameObject pcJoinCanvas;

    public bool gameStarted = false;

    private void Start()
    {
        if (vrHardwareRig.activeInHierarchy)
        {

        }
        else if (pcHardwareRig.activeInHierarchy)
        {

        }
    }

    public async void JoinTheNetwork()
    {
        await connectionManager.Connect();
        gameStarted = true;
    }
}
