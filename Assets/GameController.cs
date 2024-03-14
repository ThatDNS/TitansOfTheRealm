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
    [SerializeField] FixCamera _camera;

    private void Start()
    {
        if (gameMode == GameMode.PC)
        {
            _camera.AttachToWarrior();
        }
        else
        {
            _camera.AttachToTitan();
        }
    }
}
