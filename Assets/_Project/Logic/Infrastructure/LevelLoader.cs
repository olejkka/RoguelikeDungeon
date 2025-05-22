using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    private static LevelLoader Instance { get; set; }
    
    [SerializeField] private CameraController _cameraController;
    
    private GameStateMachine _gameStateMachine;
    private Player _player;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void OnEnable()
    {
        TileFactory.AllTilesInitialized += OnTilesRegistered;
        SpawnPointCreator.SpawnPointCreated += OnSpawnPointCreated;
    }

    private void Start()
    {
        LoadNewLevel();
    }
    
    private void Update()
    {
        _gameStateMachine?.UpdateState();
    }

    private void OnDisable()
    {
        TileFactory.AllTilesInitialized -= OnTilesRegistered;
        SpawnPointCreator.SpawnPointCreated -= OnSpawnPointCreated;
    }
    
    private void LoadNewLevel()
    {
        ClearEntities();
        TilesRepository.Instance.ClearTiles();
        TileFactory.Instance.ClearTiles();
        
        Transform roomTransform = TileFactory.Instance.GenerateRoom();
        
        if (_cameraController != null && roomTransform != null)
            _cameraController.SetTarget(roomTransform);
    }
    
    private void OnTilesRegistered()
    {
        SpawnPointCreator.Instance.CreateSpawnPoint();
        TransitionPointCreator.Instance.CreateTransitionPoint();
    }
    
    private void OnSpawnPointCreated()
    {
        _player = PlayerFactory.Instance.Generate() as Player;
        
        if (_player == null)
        {
            Debug.LogError("LevelLoader: не удалось получить Player из фабрики");
            return;
        }
        
        _player.OnTransitionTileStepped += OnPlayerSteppedOnTheTransitionTile;
        
        EnemyFactory.Instance.SpawnEnemies();
        
        _gameStateMachine = GameStateMachineFactory.CreateGameStateMachine(_player);
        _gameStateMachine.EnterState<PlayerTurn>();
    }
    
    private void ClearEntities()
    {
        Player oldPlayer = FindObjectOfType<Player>();
        
        if (oldPlayer != null)
            Destroy(oldPlayer.gameObject);

        foreach (var enemy in FindObjectsOfType<Enemy>())
            Destroy(enemy.gameObject);
    }

    void OnPlayerSteppedOnTheTransitionTile()
    {
        _player.OnTransitionTileStepped -= OnPlayerSteppedOnTheTransitionTile;
        
        ClearEntities();
        TilesRepository.Instance.ClearTiles();
        TileFactory.Instance.ClearTiles();
        
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
