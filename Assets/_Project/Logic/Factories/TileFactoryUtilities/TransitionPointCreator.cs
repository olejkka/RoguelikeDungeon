using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TransitionPointCreator : MonoBehaviour
{
    public static TransitionPointCreator Instance { get; private set; }

    [Header("Transition Material")]
    [SerializeField] private Material _transitionMaterial;

    private Tile _currentTransitionTile;
    private Material _originalMaterial;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void CreateTransitionPoint()
    {
        ClearTransitionPoint();

        if (_transitionMaterial == null)
        {
            Debug.LogError("Transition Material не назначен в инспекторе!");
            return;
        }

        Tile[] tiles = FindObjectsOfType<Tile>();
        Tile spawnTile = tiles.FirstOrDefault(t => t.Type == TileType.Spawn);
        
        if (spawnTile == null)
        {
            Debug.LogWarning("Spawn tile not found. Can't place transition point.");
            return;
        }

        var floorTiles = tiles
            .Where(t => t.Type == TileType.Floor && t != spawnTile)
            .ToList();

        if (floorTiles.Count == 0)
        {
            Debug.LogWarning("Нет подходящих тайлов для точки перехода.");
            return;
        }

        // Выбираем плиту на максимальном расстоянии (по квадрату) от spawn
        float maxDistSq = -1f;
        Tile selected = null;
        Vector2 spawnPos2 = new Vector2(spawnTile.transform.position.x, spawnTile.transform.position.z);
        
        foreach (var tile in floorTiles)
        {
            Vector2 pos2 = new Vector2(tile.transform.position.x, tile.transform.position.z);
            float distSq = (pos2 - spawnPos2).sqrMagnitude;
            
            if (distSq > maxDistSq)
            {
                maxDistSq = distSq;
                selected = tile;
            }
        }

        _currentTransitionTile = selected;
        selected.Type = TileType.Transition;

        // Заменяем материал
        var renderer = selected.GetComponentInChildren<Renderer>();
        
        if (renderer != null)
        {
            _originalMaterial = renderer.material;
            renderer.material = _transitionMaterial;
        }
        else
        {
            Debug.LogWarning("У выбранного тайла нет Renderer для замены материала.");
        }
    }
    
    public void ClearTransitionPoint()
    {
        if (_currentTransitionTile != null)
        {
            _currentTransitionTile.Type = TileType.Floor;
            var renderer = _currentTransitionTile.GetComponentInChildren<Renderer>();
            
            if (renderer != null && _originalMaterial != null)
                renderer.material = _originalMaterial;

            _currentTransitionTile = null;
            _originalMaterial = null;
        }
    }
}
