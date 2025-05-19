using UnityEngine;

public class GameOverState : IGameState
{
    private readonly GameStateMachine_ _stateMachine;

    public GameOverState(GameStateMachine_ stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Time.timeScale = 0.15f;
        Debug.Log("Игра окончена");
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}