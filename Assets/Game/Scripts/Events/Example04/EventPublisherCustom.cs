using UnityEngine;
using System;

public class CustomEventArgs : EventArgs
{
    public string Message { get; set; }
}

public class EventPublisherCustom : MonoBehaviour
{
    // Declare the event using EventHandler with the custom EventArgs
    public event EventHandler<CustomEventArgs> OnCustomEvent;

    // Method to trigger the event
    public void RaiseCustomEvent(string message)
    {
        // Raise the event with the provided message
        OnCustomEvent?.Invoke(this, new CustomEventArgs() { Message = message });
    }

    // This method could be tied to a UI button click or other in-game event
    public void TriggerEvent()
    {
        RaiseCustomEvent("This is a custom message from EventPublisher");
    }
}
