/// <summary>
/// Represents the 'Walking' state of the player.
/// </summary>
public class WalkingState : IAgentState<PController>
{
    /// <summary>
    /// Enter the state, typically to initialize or set up animations.
    /// </summary>
    public void EnterState(PController agent)
    {
        agent.HandleAnimation();
    }

    /// <summary>
    /// Handle input specific to the 'Walking' state, like movement and weapon handling.
    /// </summary>
    public void HandleInput(PController agent)
    {
        agent.HandleMovement();
        agent.HandleWeapon();
    }

    /// <summary>
    /// Update the state, check if the player stopped walking to transition to 'Idle'.
    /// </summary>
    public void Update(PController agent)
    {
        if (!agent.isWalking)
        {
            agent.TransitionToState(new IdleState());
        }
    }

    /// <summary>
    /// Exit the state, usually for cleanup or resetting state-specific settings.
    /// </summary>
    public void ExitState(PController agent)
    {
    }
}