using UnityEngine;

public class PauseState : IGameState
{
    private readonly GameStateMachine_ _stateMachine;
    private float _prevTimeScale;

    public PauseState(GameStateMachine_ stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Tick()
    {
    }

    public void Exit()
    {
        Time.timeScale = _prevTimeScale;
    }
}