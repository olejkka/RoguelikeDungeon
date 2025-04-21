using UnityEngine;

public class TileRegistrator : MonoBehaviour
{
    public static TileRegistrator Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        TileFactory.OnBoardGenerated += OnBoardGeneratedHandler;
    }

    private void OnDisable()
    {
        TileFactory.OnBoardGenerated -= OnBoardGeneratedHandler;
    }

    void OnBoardGeneratedHandler()
    {
        RegisterAllTiles();
    }

    private void RegisterAllTiles()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();

        foreach (var tile in tiles)
        {
            tile.Initialize();

            TilesRepository.Instance.RegisterTile(tile, tile.Position);
        }
    }
}