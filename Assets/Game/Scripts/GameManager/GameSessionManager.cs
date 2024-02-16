using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages game sessions, including starting, ending, and persisting match data.
/// </summary>
public class GameSessionManager : MonoBehaviour
{
    #region Fields and Properties

    private List<MatchData> sessions = new List<MatchData>();
    private MatchData currentMatch;
    private bool isSessionActive = false;

    /// <summary>
    /// Indicates whether a game session is currently active.
    /// </summary>
    public bool IsSessionActive => isSessionActive;

    private Player player;

    /// <summary>
    /// Path to save game session data.
    /// </summary>
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "gameSessions.json");

    #endregion

    #region Unity Lifecycle Methods

    private void Start()
    {
        FindPlayerReference();
        LoadSessions();
    }

    #endregion

    #region Match Management

    /// <summary>
    /// Initializes the game session manager, finding the player reference and loading existing sessions.
    /// </summary>
    private void FindPlayerReference()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }

    /// <summary>
    /// Starts a new game match, initializing match data and marking the session as active.
    /// </summary>
    public void StartMatch()
    {
        if (isSessionActive)
        {
            Debug.LogWarning("GameSessionManager: A match is already in progress.");
            return;
        }
        currentMatch = new MatchData { StartTime = DateTime.Now };
        isSessionActive = true;
        Debug.Log($"Match started at {currentMatch.StartTime}");
    }

    /// <summary>
    /// Ends the current game match, finalizes match data, adds it to the sessions list, and triggers the new session added event.
    /// </summary>
    public void EndMatch()
    {
        if (!isSessionActive)
        {
            Debug.LogWarning("GameSessionManager: No match is currently in progress.");
            return;
        }

        FinalizeCurrentMatch();
        sessions.Add(currentMatch);
        EventBus.Instance.Publish(new NewSessionAddedEvent(currentMatch));
        SaveSessions();
        isSessionActive = false;
        Debug.Log($"Match ended at {currentMatch.EndTime}");
    }

    /// <summary>
    /// Finalizes data for the current match, including calculating duration and capturing player data.
    /// </summary>
    private void FinalizeCurrentMatch()
    {
        currentMatch.EndTime = DateTime.Now;
        currentMatch.Duration = currentMatch.EndTime - currentMatch.StartTime;
        //currentMatch.playerData = new Player.PlayerData
        //{
        //    stones = player.GetStones()
        //};
    }

    #endregion

    #region Data Persistence

    /// <summary>
    /// Saves the list of game sessions to a file.
    /// </summary>
    private void SaveSessions()
    {
        var wrapper = new ListWrapper { Sessions = sessions };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Game sessions saved at: {SaveFilePath}");
    }

    /// <summary>
    /// Loads game sessions from a file, if available.
    /// </summary>
    private void LoadSessions()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            sessions = JsonUtility.FromJson<ListWrapper>(json).Sessions;
        }
    }

    /// <summary>
    /// Retrieves the list of recorded game sessions.
    /// </summary>
    /// <returns>A list of game session data.</returns>
    public List<MatchData> GetSessions()
    {
        return sessions;
    }

    #endregion

    #region Nested Classes and Structs

    [Serializable]
    public struct MatchData
    {
        [SerializeField] private string startTime;
        [SerializeField] private string endTime;
        [SerializeField] private string duration;
        public Player.PlayerData playerData;

        public DateTime StartTime
        {
            get => DateTime.Parse(startTime);
            set => startTime = value.ToString();
        }

        public DateTime EndTime
        {
            get => DateTime.Parse(endTime);
            set => endTime = value.ToString();
        }

        public TimeSpan Duration
        {
            get => TimeSpan.Parse(duration);
            set => duration = value.ToString();
        }
    }

    [Serializable]
    private class ListWrapper
    {
        public List<MatchData> Sessions;
    }

    public struct NewSessionAddedEvent
    {
        public MatchData SessionData { get; }

        public NewSessionAddedEvent(MatchData sessionData)
        {
            SessionData = sessionData;
        }
    }

    #endregion
}
