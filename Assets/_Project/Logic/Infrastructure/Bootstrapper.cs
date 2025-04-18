using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private TileFactory _tileFactory;
    [SerializeField] private PlayerFactory _playerFactory;
    [SerializeField] private SpawnPointCreator _spawnPointCreator;
    [SerializeField] private CameraFollower _cameraFollower;

    private void OnEnable()
    {
        SpawnPointCreator.OnSpawnPointCreated += GeneratePlayer;
    }

    private void OnDisable()
    {
        SpawnPointCreator.OnSpawnPointCreated -= GeneratePlayer;
    }

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        _tileFactory.ClearTiles();
        _tileFactory.GenerateBoard();
    }

    private void GeneratePlayer()
    {
        Player player = _playerFactory.Generate();
        _cameraFollower.SetTarget(player.transform);
    }
}