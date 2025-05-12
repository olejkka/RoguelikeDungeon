using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    private static LevelLoader Instance { get; set; }

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
        PlayerTurnState.PlayerSteppedOnTheTransitionTile += OnPlayerSteppedOnTheTransitionTile;
    }

    private void Start()
    {
        LoadNewLevel();
    }

    private void OnDisable()
    {
        TileFactory.AllTilesInitialized -= OnTilesRegistered;
        SpawnPointCreator.SpawnPointCreated -= OnSpawnPointCreated;
        PlayerTurnState.PlayerSteppedOnTheTransitionTile -= OnPlayerSteppedOnTheTransitionTile;
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
        if (player != null)
        {
            var highlighter = player.GetComponent<AvailableMovesHighlighter>();
        
            List<Enemy> enemies = EnemyFactory.Instance.SpawnEnemies();
        
            var playerState = new PlayerTurnState(GameStateMachine.Instance, player, highlighter, enemies);
            GameStateMachine.Instance.Initialize(playerState);
        }
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
