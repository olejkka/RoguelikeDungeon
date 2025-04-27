using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Movement _movement;
    [SerializeField] private NeighborTilesSelectionSO _neighborTilesSelectionSO;
    private Tile _currentTile;
    
    public Health Health => _health;
    public Tile CurrentTile {
        get => _currentTile;
        set {
            if (_currentTile != null && _currentTile.OccupiedCharacter == this)
                _currentTile.OccupiedCharacter = null;
            _currentTile = value;
            
            if (_currentTile != null)
                _currentTile.OccupiedCharacter = this;
        }
    }
    public NeighborTilesSelectionSO NeighborTilesSelectionSO { get => _neighborTilesSelectionSO; set => _neighborTilesSelectionSO = value; }
    

    public void Move(Tile targetTile)
    {
        if (_movement != null)
            _movement.Move(targetTile);
    }

    public void Attack(Tile targetTile)
    {
        var target = targetTile.OccupiedCharacter;
        if (target != null && CharacterIdentifier.IsEnemy(this, target))
        {
            target.Health.TakeDamage(10);
        }
    }
}
