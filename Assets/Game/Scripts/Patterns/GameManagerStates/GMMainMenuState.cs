using System.Reflection;
using UnityEngine;

public class GMMainMenuState : IState<GameManager>
{
    public void EnterState(GameManager gm)
    {
        if(gm.SessionManager.IsSessionActive) gm.SessionManager.EndMatch();
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void ExitState(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void HandleInput(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void ResumeState(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    public void Update(GameManager gm)
    {
        Debug.Log($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
    }
}
