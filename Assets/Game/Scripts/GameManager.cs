using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages overall game state and session timing. Inherits from Singleton to ensure only one instance exists.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Private Variables

    private DateTime _sessionStartTime;
    private DateTime _sessionEndTime;
    private GameSessionManager _sessionManager;

    private Stack<IState<GameManager>> stateHistory = new Stack<IState<GameManager>>();
    private IState<GameManager> currentState;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the session manager component associated with the game manager.
    /// </summary>
    public GameSessionManager SessionManager => _sessionManager;

    #endregion

    #region Unity Lifecycle Methods

    private new void Awake()
    {
        base.Awake();
        InitializeSessionManager();
        InitializeStateMachine();
    }

    private void Start()
    {
        _sessionStartTime = DateTime.Now;
    }

    private void OnApplicationQuit()
    {
        _sessionEndTime = DateTime.Now;
        TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);
        // Potential place to log session duration or handle other end-of-session logic
    }

    #endregion

    #region State Management

    /// <summary>
    /// Initializes the game manager's session manager component and state machine.
    /// </summary>
    private void InitializeStateMachine()
    {
        currentState = new GMMainMenuState();
        // The initial state is set to the main menu.
    }

    /// <summary>
    /// Ensures the session manager component is initialized, either by finding an existing instance or creating a new one.
    /// </summary>
    private void InitializeSessionManager()
    {
        _sessionManager ??= GetComponent<GameSessionManager>();
        _sessionManager ??= FindAnyObjectByType<GameSessionManager>();
    }

    /// <summary>
    /// Manages the transition between game states, handling state exit and entry logic.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    /// <param name="isTemporaryTransition">Indicates if the transition is temporary (e.g., going to a pause menu).</param>
    private void TransitionState(IState<GameManager> newState, bool isTemporaryTransition = false)
    {
        if (!isTemporaryTransition)
        {
            currentState?.ExitState(this);
        }
        else
        {
            stateHistory.Push(currentState);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    /// <summary>
    /// Changes the game's current state to a new state.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    public void ChangeState(IState<GameManager> newState)
    {
        TransitionState(newState);
    }

    /// <summary>
    /// Resumes the previous state from the state history stack.
    /// </summary>
    public void ResumePreviousState()
    {
        if (stateHistory.Count > 0)
        {
            currentState.ExitState(this);
            currentState = stateHistory.Pop();
            currentState.ResumeState(this);
        }
    }

    /// <summary>
    /// Logs the current state stack to the debug console.
    /// </summary>
    public void DebugLogStateStack()
    {
        string stackHistory = string.Empty;
        foreach (var state in stateHistory)
        {
            stackHistory += state.GetType().Name + "\t";
        }
        Debug.Log($"Current State Stack: {stackHistory}");
    }

    #endregion

    #region GUI Methods

    GUIStyle myButtonStyle;

    void OnGUI()
    {
        InitializeGUIStyle();

        RenderMainMenuStateGUI();
        RenderPlayStateGUI();
        //RenderOptionsMenuStateGUI();
        RenderPauseStateGUI();

        DisplayCurrentStateName();
    }

    /// <summary>
    /// Initializes the style for GUI buttons if it has not already been set.
    /// </summary>
    private void InitializeGUIStyle()
    {
        if (myButtonStyle == null)
        {
            myButtonStyle = new GUIStyle(GUI.skin.button) { fontSize = 50 };
        }
    }

    /// <summary>
    /// Renders GUI elements for the Main Menu state.
    /// </summary>
    private void RenderMainMenuStateGUI()
    {
        if (currentState is GMMainMenuState)
        {
            if (GUI.Button(new Rect(10, 310, 300, 100), "Play", myButtonStyle))
            {
                ChangeState(new GMPlayState());
            }
            if (GUI.Button(new Rect(10, 425, 300, 100), "Options", myButtonStyle))
            {
                TransitionState(new GMOptionsMenuState(), true);
            }
            if (GUI.Button(new Rect(10, 540, 300, 100), "Exit", myButtonStyle))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }

    /// <summary>
    /// Renders GUI elements for the Play state.
    /// </summary>
    private void RenderPlayStateGUI()
    {
        if (currentState is GMPlayState)
        {
            if (GUI.Button(new Rect(10, 310, 300, 100), "Pause", myButtonStyle))
            {
                TransitionState(new GMPauseState(), true);
            }
            if (GUI.Button(new Rect(10, 425, 300, 100), "Options", myButtonStyle))
            {
                TransitionState(new GMOptionsMenuState(), true);
            }
            if (GUI.Button(new Rect(10, 540, 300, 100), "End Match", myButtonStyle))
            {
                ChangeState(new GMMainMenuState());
            }
        }
    }

    /// <summary>
    /// Renders GUI elements for the Options Menu state.
    /// </summary>
    private void RenderOptionsMenuStateGUI()
    {
        if (currentState is GMOptionsMenuState)
        {
            if (GUI.Button(new Rect(10, 810, 300, 100), "Save", myButtonStyle))
            {
                // Save options logic
            }
            if (GUI.Button(new Rect(10, 925, 300, 100), "Back", myButtonStyle))
            {
                ResumePreviousState();
            }
        }
    }

    /// <summary>
    /// Renders GUI elements for the Pause state.
    /// </summary>
    private void RenderPauseStateGUI()
    {
        if (currentState is GMPauseState)
        {
            if (GUI.Button(new Rect(10, 310, 300, 100), "Resume", myButtonStyle))
            {
                ResumePreviousState();
            }
            if (GUI.Button(new Rect(10, 425, 300, 100), "Main Menu", myButtonStyle))
            {
                ChangeState(new GMMainMenuState());
            }
        }
    }

    /// <summary>
    /// Displays the name of the current state in the GUI.
    /// </summary>
    private void DisplayCurrentStateName()
    {
        GUIStyle stateNameStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 50,
            normal = { textColor = Color.white }
        };

        string stateName = currentState != null ? currentState.GetType().Name : "No State";
        string displayText = $"Current State: {stateName}";

        Vector2 labelSize = stateNameStyle.CalcSize(new GUIContent(displayText));
        GUI.Label(new Rect((Screen.width - labelSize.x) / 2, Screen.height - 80, labelSize.x, labelSize.y), displayText, stateNameStyle);
    }

    #endregion

    /*
    Additional commented-out methods or logic can be placed here for reference or future use.
    */
}
