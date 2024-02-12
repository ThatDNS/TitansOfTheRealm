using UnityEngine;
using TMPro;
using static GameSessionManager; // Ensures access to GameSessionManager.NewSessionAddedEvent

/// <summary>
/// Manages the UI display of game session data, updating the UI to reflect current and new game sessions.
/// </summary>
public class GameSessionUIManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject sessionItemPrefab; // Prefab for displaying each session item.

    [SerializeField]
    private Transform sessionsContainer; // Parent object in the UI for session items.

    private GameSessionManager gameSessionManager; // Reference to the GameSessionManager.

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        gameSessionManager = GameManager.Instance.SessionManager;

        if (gameSessionManager != null)
        {
            DisplaySessions();
            EventBus.Instance.Subscribe<NewSessionAddedEvent>(OnNewSessionAdded);
        }
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<NewSessionAddedEvent>(OnNewSessionAdded);
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the event when a new session is added, updating the UI accordingly.
    /// </summary>
    /// <param name="eventData">The data for the new session.</param>
    private void OnNewSessionAdded(object eventData)
    {
        var newSessionEvent = (NewSessionAddedEvent)eventData;
        AddSessionToUI(newSessionEvent.SessionData);
    }

    #endregion

    #region UI Update Methods

    /// <summary>
    /// Populates the UI with existing session data from the GameSessionManager.
    /// </summary>
    private void DisplaySessions()
    {
        var sessions = gameSessionManager.GetSessions();

        foreach (var session in sessions)
        {
            AddSessionToUI(session);
        }
    }

    /// <summary>
    /// Adds a single session's data to the UI.
    /// </summary>
    /// <param name="session">The session data to be displayed.</param>
    private void AddSessionToUI(MatchData session)
    {
        var item = Instantiate(sessionItemPrefab, sessionsContainer);
        item.GetComponentInChildren<TextMeshProUGUI>().text = FormatSessionData(session);
    }

    /// <summary>
    /// Formats session data into a string for display in the UI.
    /// </summary>
    /// <param name="session">The session data to format.</param>
    /// <returns>A string representing the formatted session data.</returns>
    private string FormatSessionData(MatchData session)
    {
        return $"Start: {session.StartTime}\nEnd: {session.EndTime}\nDuration: {session.Duration}";
    }

    #endregion
}
