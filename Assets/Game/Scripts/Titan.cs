using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Titan : MonoBehaviour
{
    [SerializeField] private GameObject leftRayController;
    [SerializeField] private GameObject rightRayController;
    [SerializeField] private GameObject leftDirectController;
    [SerializeField] private GameObject rightDirectController;

    private ActionBasedContinuousMoveProvider _continuousMoveProvider;
    private ActionBasedContinuousTurnProvider _continuousTurnProvider;

    private void Start()
    {
        _continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        _continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();

        // Don't allow movements at the beginning
        _continuousMoveProvider.enabled = false;
        _continuousTurnProvider.enabled = false;

        // Enable ray points (to be able to interact with UI button)
        leftRayController.SetActive(true);
        rightRayController.SetActive(true);
        leftDirectController.SetActive(false);
        rightDirectController.SetActive(false);
    }

    public void AllowMovement()
    {
        Debug.Log("ALLOWING MOVEMENT!");
        _continuousMoveProvider.enabled = true;
        _continuousTurnProvider.enabled = true;
    }

    public void SwitchFromRayToDirectInteraction()
    {
        leftRayController.SetActive(false);
        rightRayController.SetActive(false);
        leftDirectController.SetActive(true);
        rightDirectController.SetActive(true);
    }
}
