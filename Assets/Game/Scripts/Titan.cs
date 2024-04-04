using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Titan : MonoBehaviour
{
    public float titanhealth = 100.0f;
    [SerializeField] HealthBar healthBar;
    [SerializeField] StartManager startManager;

    private void Start()
    {
        GameManager.Instance.StartTrackingTime();
    }

    private void Update()
    {
        if (healthBar.GetHealthFillAmount() == 0.0f && !startManager.isGameOver)
        {
            startManager.isGameOver = true;
            GameManager.Instance.EndTrackingTime();
            startManager.ShowVRDeathCanvas();
        }
    }

}
