using System;
using UnityEngine;

public class EventPublisher : MonoBehaviour
{
    // Declare the event using EventHandler
    public event EventHandler OnSimpleEvent;

    // This method could be triggered by some action
    // in the game, such as pressing a button
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            RaiseSimpleEvent();
        }
    }

    // Method to trigger the event
    public void RaiseSimpleEvent()
    {
        // Raise the event, passing EventArgs.Empty
        // since there's no data
        OnSimpleEvent?.Invoke(this, EventArgs.Empty);
    }
}
