using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : NetworkBehaviour//, IPlayerJoined
{
    //public enum GameMode
    //{
    //    PC,
    //    VR
    //};

    //public GameMode gameMode;
    //public GameObject titanGO;
    //public GameObject warriorGO;

    //private void Start()
    //{
    //    Debug.Log("Game started & mode is " + gameMode);
    //    if (gameMode == GameMode.PC)
    //    {
    //        //vrCamera.depth = 0;
    //        //pcCamera.depth = 1;
    //        //vrCameraGO.GetComponent<AudioListener>().enabled = false;
    //        //pcCamera.gameObject.GetComponent<AudioListener>().enabled = true;
    //    }
    //    else
    //    {
    //        //vrCamera.depth = 1;
    //        //pcCamera.depth = 0;
    //        //vrCamera.gameObject.GetComponent<AudioListener>().enabled = true;
    //        //pcCamera.gameObject.GetComponent<AudioListener>().enabled = false;
    //    }
    //}

    //public void PlayerJoined(PlayerRef player)
    //{
    //    Debug.Log("Player JOINED");
    //    if (gameMode == GameMode.PC)
    //    {
    //        // Spawn warrior
    //        Runner.Spawn(warriorGO, new Vector3(2.45f, 2.75f, 11.75f), Quaternion.Euler(0f, 180f, 0f), player);
    //        Debug.Log("Hmm this is PC -- WARRIOR GOES BURRR!");
    //    }
    //    else
    //    {
    //        //Runner.Spawn(titanGO);
    //    }
    //}
}
