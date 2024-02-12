/// <summary>
/// Represents the 'Attacking' state of the player.
/// </summary>
public class AttackingState : IAgentState<PlayerController>
{
    /// <summary>
    /// Enter the state. This is typically used to set up or initialize anything 
    /// specific to the attacking state, like starting attack animations.
    /// </summary>
    public void EnterState(PlayerController agent)
    {
        // Implementation for what happens when entering the attacking state
        // e.g., agent.StartAttackAnimation();
    }

    /// <summary>
    /// Handle input specific to the 'Attacking' state. 
    /// This could involve checking for continued attack commands or other actions.
    /// </summary>
    public void HandleInput(PlayerController player)
    {
        // Implementation for handling input while in the attacking state
        // e.g., if (player.IsContinuingAttack()) { /* Continue attack */ }
    }

    /// <summary>
    /// Update the state. This method contains the logic that happens 
    /// each frame while in the attacking state.
    /// </summary>
    public void Update(PlayerController player)
    {
        // Implementation for updating the state, such as checking if the attack is finished
        // e.g., if (!player.IsAttacking()) { player.TransitionToState(new IdleState()); }
    }

    /// <summary>
    /// Exit the state. This method is called when transitioning out of the attacking state,
    /// typically used for cleanup or resetting state-specific settings.
    /// </summary>
    public void ExitState(PlayerController agent)
    {
        // Implementation for what happens when exiting the attacking state
        // e.g., agent.ResetAttack();
    }
}