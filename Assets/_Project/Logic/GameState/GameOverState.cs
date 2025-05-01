using UnityEngine;

public class GameOverState : IGameState
{
    private readonly GameStateMachine _stateMachine;

    public GameOverState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        Debug.Log("Игра окончена");
    }

    public void Tick()
    {
    }

    public void Exit()
    {
    }
}