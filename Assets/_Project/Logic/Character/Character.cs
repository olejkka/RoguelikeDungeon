using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(CharacterAnimation))]
public class Character : MonoBehaviour
{
    [SerializeField] private int _remainingActions;
    
    [SerializeField] private Health _health;
    [SerializeField] private CharacterMover _mover;
    [SerializeField] private CharacterCombat _combat;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private NeighborTilesSelectionSO _neighborTilesSelectionSO;
    
    private Tile _currentTile;
    private Tile _targetTile;
    private TilesRepository _tilesRepository;
    private CharacterStateMachine _stateMachine;
    
    public int RemainingActions 
    {
        get => _remainingActions;
        set => _remainingActions = value;
    }
    public Health Health => _health;
    public CharacterMover Mover => _mover;
    public CharacterCombat Combat => _combat;
    public CharacterAnimation Animation => _animation;
    public NeighborTilesSelectionSO NeighborTilesSelectionSO => _neighborTilesSelectionSO;
    public Tile CurrentTile
    {
        get => _currentTile;
        set 
        {
            if (_currentTile != null && _currentTile.OccupiedCharacter == this)
                _currentTile.OccupiedCharacter = null;
            _currentTile = value;
            
            if (_currentTile != null)
                _currentTile.OccupiedCharacter = this;
        }
    }
    
    public Tile TargetTile
    {
        get => _targetTile;
        set => _targetTile = value;
    }
    public bool HasTargetTile => _targetTile != null;
    
    
    void Awake()
    {
        _tilesRepository = TilesRepository.Instance;
        InitializeAtCurrentPosition();
        _stateMachine = CharacterStateMachineFactory.CreateCharacterStateMachine(this);
    }

    void Start()
    {
        _stateMachine.EnterState<IdleState>();
    }
    
    void Update()
    {
        _stateMachine.UpdateState();
    }
    
    void InitializeAtCurrentPosition()
    {
        var worldPos = transform.position;
        Vector2Int tilePos = new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
        
        Tile tile = _tilesRepository.GetTileAt(tilePos);
        if (tile == null)
        {
            throw new Exception($"[Character] Тайл не найден на позиции {tilePos}");
        }

        CurrentTile = tile;
        
        Debug.Log($"(Character) {name} завершил инициализацию. CurrentTile {CurrentTile}");
    }
    
    public void StartIdle()
    {
        _animation.PlayIdle();
    }
    
    public void StartMoving()
    {
        Mover.MoveToNearestFloor(TargetTile);
    }
    
    public void StartAttacking()
    {
        Combat.StartAttack();
    }

    public void StopAnimation()
    {
        Animation.StopAnimation();
    }
}
