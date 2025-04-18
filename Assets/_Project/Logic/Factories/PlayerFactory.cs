using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;

    public Player Generate()
    {
        Transform spawnPoint = FindSpawnPointTransform();
        if (spawnPoint == null)
        {
            Debug.LogError("Точка спавна не найдена (Tile.IsSpawnPoint == true, но поле _spawnPoint не задано)");
            return null;
        }

        return Instantiate(_playerPrefab, spawnPoint.position, Quaternion.identity);
    }

    private Transform FindSpawnPointTransform()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (Tile tile in tiles)
        {
            if (tile.IsSpawnPoint && tile.TryGetSpawnPoint(out Transform spawnPoint))
            {
                return spawnPoint;
            }
        }

        return null;
    }
}