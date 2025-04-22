using System;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    
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
        TileFactory.OnRoomGenerated += CreateSpawnPoint;
        SpawnPointCreator.OnSpawnPointCreated += CreateTransitionPoint;
        SpawnPointCreator.OnSpawnPointCreated += CreatePlayer;
        SpawnPointCreator.OnSpawnPointCreated += CreateEnemies;
        Movement.OnSteppingOnATransitionTile += LoadNewLevel;
    }

    private void Start()
    {
        LoadNewLevel();
    }

    private void OnDisable()
    {
        TileFactory.OnRoomGenerated -= CreateSpawnPoint;
        SpawnPointCreator.OnSpawnPointCreated -= CreateTransitionPoint;
        SpawnPointCreator.OnSpawnPointCreated -= CreatePlayer;
        SpawnPointCreator.OnSpawnPointCreated -= CreateEnemies;
        Movement.OnSteppingOnATransitionTile -= LoadNewLevel;
    }

    public void LoadNewLevel()
    {
        Bootstrapper.Instance.TileFactory.ClearTiles();
        Bootstrapper.Instance.TileFactory.GenerateRoom();
    }

    private void CreateSpawnPoint()
    {
        Bootstrapper.Instance.SpawnPointCreator.CreateSpawnPoint();
    }

    private void CreateTransitionPoint()
    {
        Bootstrapper.Instance.TransitionPointCreator.CreateTransitionPoint();
    }

    private void CreatePlayer()
    {
        Player player = Bootstrapper.Instance.PlayerFactory.Generate() as Player;
        
        Bootstrapper.Instance.CameraFollower.SetTarget(player.transform);
        Bootstrapper.Instance.CameraRotator.SetTarget(player.transform);

    }

    private void CreateEnemies()
    {
        Bootstrapper.Instance.EnemyFactory.SpawnEnemies();
    }
}