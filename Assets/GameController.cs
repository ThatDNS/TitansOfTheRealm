using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameMode
    {
        PC,
        VR
    };

    public GameMode gameMode;
    public Camera vrCamera;
    public Camera pcCamera;

    private void Start()
    {
        if (gameMode == GameMode.PC)
        {
            vrCamera.depth = 0;
            pcCamera.depth = 1;
        }
        else
        {
            vrCamera.depth = 1;
            pcCamera.depth = 0;
        }
    }
}
