using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Titan : MonoBehaviour
{
    [SerializeField] int moveSpeed = 2;
    [SerializeField] int turnSpeed = 50;
    [SerializeField] GameObject leftRayController;
    [SerializeField] GameObject rightRayController;
    [SerializeField] GameObject leftDirectController;
    [SerializeField] GameObject rightDirectController;

    private ActionBasedContinuousMoveProvider _continuousMoveProvider;
    private ActionBasedContinuousTurnProvider _continuousTurnProvider;

    private void Start()
    {
        _continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        _continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();

        // Don't allow movements at the beginning
        _continuousMoveProvider.moveSpeed = 0;
        _continuousTurnProvider.turnSpeed = 0;

        // Enable ray points (to be able to interact with UI button)
        SetActiveAllComponents(leftRayController, true);
        SetActiveAllComponents(rightRayController, true);
        SetActiveAllComponents(leftDirectController, false);
        SetActiveAllComponents(rightDirectController, false);
    }

    public void AllowMovement()
    {
        Debug.Log("ALLOWING MOVEMENT!");
        _continuousMoveProvider.moveSpeed = moveSpeed;
        _continuousTurnProvider.turnSpeed = turnSpeed;
    }

    public void SwitchFromRayToDirectInteraction()
    {
        SetActiveAllComponents(leftRayController, false);
        SetActiveAllComponents(rightRayController, false);
        SetActiveAllComponents(leftDirectController, true);
        SetActiveAllComponents(rightDirectController, true);
    }

    // This is important as if we enable/disable the whole game objects containing ray controller or direct controller
    // then there could be issues where (i think) the input system can't find them anymore and throw NullReferenceException.
    // Alternative here is to disable/enable all components instead of the game object itself.
    private void SetActiveAllComponents(GameObject obj, bool isActive)
    {
        foreach (Component component in obj.GetComponents<Component>())
        {

            if (component is Transform)
                continue;

            if (component is Behaviour behaviour)
                behaviour.enabled = isActive;
            if (component is Renderer renderer)
                renderer.enabled = isActive;
        }
    }
}
