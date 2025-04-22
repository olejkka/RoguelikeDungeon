using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private TileFactory _tileFactory;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private SpawnPointCreator _spawnPointCreator;
    [SerializeField] private TransitionPointCreator _transitionPointCreator;
    [SerializeField] private TileHighlighter _tileHighlighter;
    [SerializeField] private CameraFollower _cameraFollower;
    [SerializeField] private CameraRotator _cameraRotator;

    public static Bootstrapper Instance { get; private set; }

    public TileFactory TileFactory => _tileFactory;
    public PlayerFactory PlayerFactory => _playerFactory;
    public EnemyFactory EnemyFactory => _enemyFactory;
    public SpawnPointCreator SpawnPointCreator => _spawnPointCreator;
    public TransitionPointCreator TransitionPointCreator => _transitionPointCreator;
    public CameraFollower CameraFollower => _cameraFollower;
    public CameraRotator CameraRotator => _cameraRotator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            TileHighlightService.Init(_tileHighlighter);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}