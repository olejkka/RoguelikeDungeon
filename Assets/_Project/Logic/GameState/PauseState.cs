using UnityEngine;

public class PauseState : IGameState
{
    private readonly GameStateMachine _stateMachine;
    private float _prevTimeScale;

    public PauseState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Enter()
    {
        _prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        // Показываем UI-окно паузы и блокируем ввод
    }

    public void Tick()
    {
        // Можно слушать кнопку «Продолжить»
    }

    public void Exit()
    {
        Time.timeScale = _prevTimeScale;
        // Скрываем UI-окно паузы, разблокируем ввод
    }
}