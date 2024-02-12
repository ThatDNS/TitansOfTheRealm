using UnityEngine;

public class EventTriggerExample : MonoBehaviour
{
    // Defining a delegate for the event
    public delegate void MessageHandler(string message);
    // Event based on the delegate 
    public static event MessageHandler OnMessageReceived;

    private void Update()
    {
        // If 'T' key is pressed, trigger the event
        if (Input.GetKeyDown(KeyCode.T))
        {
            TriggerMessageEvent("This is a message from EventTriggerExample");
        }
    }

    // This method triggers the event
    private void TriggerMessageEvent(string message)
    {
        // Triggering the event if there are any subscribers
        OnMessageReceived?.Invoke(message);
    }
}
