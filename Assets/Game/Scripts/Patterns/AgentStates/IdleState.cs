/// <summary>
/// Represents the 'Idle' state of the player.
/// </summary>
public class IdleState : IAgentState<PController>
{
    /// <summary>
    /// Enter the state, used for setting up animations specific to being idle.
    /// </summary>
    public void EnterState(PController agent)
    {
        agent.HandleAnimation();
    }

    /// <summary>
    /// Handle input specific to the 'Idle' state, including movement and weapon handling.
    /// </summary>
    public void HandleInput(PController agent)
    {
        agent.HandleMovement();
        agent.HandleWeapon();
    }

    /// <summary>
    /// Update the state, check if the player started walking to transition to 'Walking'.
    /// </summary>
    public void Update(PController agent)
    {
        if (agent.isWalking)
        {
            agent.TransitionToState(new WalkingState());
        }
    }

    /// <summary>
    /// Exit the state, typically used for cleanup or resetting state-specific settings.
    /// </summary>
    public void ExitState(PController agent)
    {

    }
}