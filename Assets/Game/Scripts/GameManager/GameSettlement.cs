using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettlement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpWon;
    [SerializeField] TextMeshProUGUI PlayTime;
    [SerializeField] TextMeshProUGUI numberofTree;

    private void Start()
    {
        tmpWon.text = "Won: ";
        PlayTime.text = $"Play Time: ,{ GameManager.Instance.GetPlayTime()}";
        numberofTree.text = $"You destory { GameManager.Instance.getTreeNum()} trees, ";
    }
}
