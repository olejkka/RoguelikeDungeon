using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private TileFactory _tileFactory;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private SpawnPointCreator _spawnPointCreator;
    [SerializeField] private TileHighlighter _tileHighlighter;
    [SerializeField] private CameraFollower _cameraFollower;
    [SerializeField] private CameraRotator _cameraRotator;
    
    public static Bootstrapper Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        TileHighlightService.Init(_tileHighlighter);
    }
    
    void OnEnable()
    {
        TileFactory.OnBoardGenerated += CreateSpawnPoint;
        SpawnPointCreator.OnSpawnPointCreated += GeneratePlayer;
    }
    
    void Start()
    {
        GenerateBoard();
    }

    void OnDisable()
    {
        TileFactory.OnBoardGenerated -= CreateSpawnPoint;
        SpawnPointCreator.OnSpawnPointCreated -= GeneratePlayer;
    }

    void GenerateBoard()
    {
        _tileFactory.ClearTiles();
        _tileFactory.GenerateBoard();
    }

    void CreateSpawnPoint()
    {
        _spawnPointCreator.CreateSpawnPoint();
    }

    void GeneratePlayer()
    {
        Player player = _playerFactory.Generate() as Player;
        if (player != null)
        {
            _cameraFollower.SetTarget(player.transform);
            _cameraRotator.SetTarget(player.transform);
        }
    }
}