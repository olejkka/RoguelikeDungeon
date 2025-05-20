using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    private static LevelLoader Instance { get; set; }
    private GameStateMachine _gameStateMachine;

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
        var player = PlayerFactory.Instance.Generate() as Player;
        
        if (player == null) 
            return;
        
        EnemyFactory.Instance.SpawnEnemies();
        
        _gameStateMachine = GameStateMachineFactory.CreateGameStateMachine(player);
        _gameStateMachine.EnterState<PlayerTurn>();
    }

   
    private void ClearEntities()
    {
        var oldPlayer = FindObjectOfType<Player>();
        
        if (oldPlayer != null)
            Destroy(oldPlayer.gameObject);

        foreach (var enemy in FindObjectsOfType<Enemy>())
            Destroy(enemy.gameObject);
    }

    void OnPlayerSteppedOnTheTransitionTile()
    {
        ClearEntities();
        TilesRepository.Instance.ClearTiles();
        TileFactory.Instance.ClearTiles();
        
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
