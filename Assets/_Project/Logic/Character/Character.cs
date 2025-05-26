using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(CharacterAnimation))]
public class Character : MonoBehaviour
{
    [SerializeField] private CharacterHealth _health;
    [SerializeField] private CharacterMover _mover;
    [SerializeField] private CharacterCombat _combat;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private NeighborTilesSelectionSO _neighborTilesSelectionSO;
    
    [SerializeField] private int _maxActions = 1;

    private Tile _currentTile;
    private Tile _targetTile;
    private TilesRepository _tilesRepository;
    private CharacterStateMachine _stateMachine;
    
    public bool HasTargetTile => _targetTile != null;
    public CharacterHealth Health => _health;
    public CharacterMover Mover => _mover;
    public CharacterCombat Combat => _combat;
    public CharacterAnimation Animation => _animation;
    public NeighborTilesSelectionSO NeighborTilesSelectionSO => _neighborTilesSelectionSO;
    public int MaxActions => _maxActions;
    public int RemainingActions { get; private set; }
    public Tile TargetTile => _targetTile;

    public Tile CurrentTile
    {
        get => _currentTile;
        set 
        {
            if (_currentTile != null && _currentTile.OccupiedCharacter == this)
                _currentTile.SetOccupiedCharacter(null);
            
            _currentTile = value;
            
            if (_currentTile != null)
                _currentTile.SetOccupiedCharacter(this);
        }
    }
    
    
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

    public void SetTargetTile(Tile targetTile)
    {
        _targetTile = targetTile;
        
        Debug.Log(TargetTile);
    }
    
    public void StartIdle()
    {
        _animation.PlayIdle();
    }
    
    public void StartMoving()
    {
        Mover.MoveTo(TargetTile);
    }
    
    public void StartAttacking()
    {
        Combat.StartAttack();
    }

    public void ChangeRemainingActions(int count)
    {
        RemainingActions += count;
    }
    
    public void ResetActions()
    {
        RemainingActions = MaxActions;
    }
}
