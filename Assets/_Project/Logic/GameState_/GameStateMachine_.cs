using UnityEngine;

public class GameStateMachine_ : MonoBehaviour
{
    private IGameState _currentState;

    public static GameStateMachine_ Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Initialize(IGameState startingState)
    {
        _currentState = startingState;
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState?.Tick();
    }

    public void ChangeState(IGameState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public T GetState<T>() where T : class, IGameState
    {
        return _currentState as T;
    }
}