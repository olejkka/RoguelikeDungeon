using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransitionPointCreator : MonoBehaviour
{
    public static event Action OnTransitionPointCreated;

    [Header("Transition Material")]
    [SerializeField] private Material _transitionMaterial;
    

    public void CreateTransitionPoint()
    {
        if (_transitionMaterial == null)
        {
            Debug.LogError("Transition Material не назначен в инспекторе!");
            return;
        }

        Tile[] tiles = FindObjectsOfType<Tile>();

        Tile spawnTile = tiles.FirstOrDefault(tile => tile.Type == TileType.Spawn);
        if (spawnTile == null)
        {
            Debug.LogWarning("Spawn tile not found. Can't place transition point.");
            return;
        }

        Tile[] floorTiles = tiles
            .Where(tile => tile.Type == TileType.Floor && tile != spawnTile)
            .ToArray();

        if (floorTiles.Length == 0)
        {
            Debug.LogWarning("Нет подходящих тайлов для точки перехода.");
            return;
        }

        Tile selected = floorTiles[Random.Range(0, floorTiles.Length)];

        selected.Type = TileType.Transition;

        Renderer tileRenderer = selected.GetComponentInChildren<Renderer>();
        if (tileRenderer != null)
        {
            tileRenderer.material = _transitionMaterial;
        }
        else
        {
            Debug.LogWarning("У выбранного тайла нет Renderer для замены материала.");
        }

        OnTransitionPointCreated?.Invoke();
    }
}