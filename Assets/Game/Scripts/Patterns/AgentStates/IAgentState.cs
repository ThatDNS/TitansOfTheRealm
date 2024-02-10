/// <summary>
/// Defines a generic state interface for an agent.
/// This interface outlines the basic methods required for a state 
/// in a state machine, applicable to any agent type.
/// </summary>
/// <typeparam name="T">The type of the agent that the state will manage.</typeparam>
public interface IAgentState<T>
{
    /// <summary>
    /// Called when the state is entered.
    /// Use this method to initialize or set up the state for the given agent.
    /// </summary>
    /// <param name="agent">The agent that is entering this state.</param>
    void EnterState(T agent);

    /// <summary>
    /// Handles input for the agent while in this state.
    /// This method should contain input processing relevant to the state.
    /// </summary>
    /// <param name="agent">The agent whose input is to be handled.</param>
    void HandleInput(T agent);

    /// <summary>
    /// Called every frame the agent is in this state.
    /// This method should contain the main logic for the state.
    /// </summary>
    /// <param name="agent">The agent that is updating in this state.</param>
    void Update(T agent);

    /// <summary>
    /// Called when exiting the state.
    /// Use this method for any cleanup or finalizing tasks for the state.
    /// </summary>
    /// <param name="agent">The agent that is exiting this state.</param>
    void ExitState(T agent);
}