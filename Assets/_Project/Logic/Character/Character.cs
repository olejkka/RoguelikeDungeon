using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterInitializer))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CharacterMover))]
public class Character : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private CharacterMover characterMover;
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

    public NeighborTilesSelectionSO NeighborTilesSelectionSO
    {
        get => _neighborTilesSelectionSO; 
        set => _neighborTilesSelectionSO = value;
    }
    

    public void Move(Tile targetTile)
    {
        if (characterMover != null)
            characterMover.Move(targetTile);
    }
}
