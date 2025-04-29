using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInitializer : MonoBehaviour
{
    private Character _character;
    private TilesRepository _tilesRepo;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _tilesRepo = TilesRepository.Instance;
    }
    
    public void InitializeAtCurrentPosition()
    {
        var worldPos = transform.position;
        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );

        Tile tile = _tilesRepo.GetTileAt(gridPos);
        if (tile == null)
        {
            Debug.LogError($"[CharacterInitializer] Tile not found at {gridPos}");
            return;
        }

        _character.CurrentTile = tile;
    }
}