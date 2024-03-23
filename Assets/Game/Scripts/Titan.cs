using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Titan : MonoBehaviour
{
    [SerializeField] int moveSpeed = 2;
    [SerializeField] int turnSpeed = 50;
    [SerializeField] float minRayDistance = 1.0f;
    [SerializeField] float maxRayDistance = 30.0f;
    [SerializeField] GameObject leftRayController;
    [SerializeField] GameObject rightRayController;

    private ActionBasedContinuousMoveProvider _continuousMoveProvider;
    private ActionBasedContinuousTurnProvider _continuousTurnProvider;
    private XRRayInteractor _leftRayInteractor;
    private XRRayInteractor _rightRayInteractor;
    private XRInteractorLineVisual _leftRayInteractorLine;
    private XRInteractorLineVisual _rightRayInteractorLine;

    private void Start()
    {
        //_continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        //_continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();

        //// Don't allow movements at the beginning
        //_continuousMoveProvider.moveSpeed = 0;
        //_continuousTurnProvider.turnSpeed = 0;

        // Extend ray points (to be able to interact with UI button)
        _leftRayInteractor = leftRayController.GetComponent<XRRayInteractor>();
        _rightRayInteractor = rightRayController.GetComponent<XRRayInteractor>();
        _leftRayInteractorLine = leftRayController.GetComponent<XRInteractorLineVisual>();
        _rightRayInteractorLine = rightRayController.GetComponent<XRInteractorLineVisual>();
        _leftRayInteractor.maxRaycastDistance = maxRayDistance;
        _rightRayInteractor.maxRaycastDistance = maxRayDistance;

        // TEMPORARY -- DO THIS ON "START GAME"
        //AllowMovement();
        UIToDirectTouchControls();
    }

    public void AllowMovement()
    {
        Debug.Log("ALLOWING MOVEMENT!");
        _continuousMoveProvider.moveSpeed = moveSpeed;
        _continuousTurnProvider.turnSpeed = turnSpeed;
    }

    /// <summary>
    /// Shortens the ray controller's ray length to allow only close interactions.
    /// </summary>
    public void UIToDirectTouchControls()
    {
        _leftRayInteractor.maxRaycastDistance = minRayDistance;
        _rightRayInteractor.maxRaycastDistance = minRayDistance;

        // Disable Line renderer
        _leftRayInteractorLine.enabled = false;
        _rightRayInteractorLine.enabled = false;
    }

    //private void Update()
    //{
    //    transform.Translate(Vector3.forward * Time.deltaTime);
    //}
}
