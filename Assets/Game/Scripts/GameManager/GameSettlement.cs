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
    }

    public void UpdateMessage(string winner)
    {
        tmpWon.text = "Won: " + winner;
        PlayTime.text = $"Play Time: ,{GameManager.Instance.GetPlayTime()}";
        numberofTree.text = $"You destory {GameManager.Instance.getTreeNum()} trees, ";
    }
}
