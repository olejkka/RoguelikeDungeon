using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CharacterMover))]
[RequireComponent(typeof(CharacterAnimation))]
public class Character : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField]private CharacterAnimation _characterAnimation;
    [SerializeField] private NeighborTilesSelectionSO _neighborTilesSelectionSO;
    private Tile _currentTile;
    private Tile _targetTile;
    private TilesRepository _tilesRepository;
    private CharacterStateMachine _stateMachine;
    
    
    public Health Health => _health;
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
    public bool HasHasTargetTile => _targetTile != null;
    
    
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
    }
    
    public void MoveTo()
    {
        _characterMover.MoveTo(TargetTile);
        TargetTile = null;
    }
    
    public void StartBreathing()
    {
        _characterAnimation.PlayBreathing();
    }

    public void StopAnimation()
    {
        _characterAnimation.StopAnimation();
    }
}
