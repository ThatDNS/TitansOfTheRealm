using UnityEngine;
using System;

public class FuncEventPublisher : MonoBehaviour
{
    // Define a Func event that takes a string and returns a string
    public event Func<string, string> OnFuncEvent;

    // Method to trigger the event and process the return value
    public void RaiseFuncEvent()
    {
        // Triggering the event with a message and receiving a response
        string response = OnFuncEvent?.Invoke("Hello from FuncEventPublisher!") ?? "No response";

        // Example of using the response, like logging it
        Debug.Log($"Response received: {response}");
    }

    // This method could be linked to a UI button or other game action
    public void TriggerEvent()
    {
        RaiseFuncEvent();
    }
}
