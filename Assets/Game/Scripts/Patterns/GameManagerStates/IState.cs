public interface IState<T>
{
    void EnterState(T state);
    void HandleInput(T state);
    void Update(T state);
    void ResumeState(T state);
    void ExitState(T state);
}
