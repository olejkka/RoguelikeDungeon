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
        Vector2Int tilePos = new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );

        Tile tile = _tilesRepo.GetTileAt(tilePos);
        if (tile == null)
        {
            Debug.LogError($"[CharacterInitializer] Tile not found at {tilePos}");
            return;
        }

        _character.CurrentTile = tile;
    }
}