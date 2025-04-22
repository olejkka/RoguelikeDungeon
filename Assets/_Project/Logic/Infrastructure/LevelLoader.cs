using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    public static LevelLoader Instance { get; private set; }

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

    private void OnEnable()
    {
        TileRegistrator.OnTilesRegistered += HandleTilesRegistered;
        SpawnPointCreator.OnSpawnPointCreated += HandleSpawnPointCreated;
        Movement.OnSteppingOnATransitionTile += ReloadCurrentScene;
    }

    private void Start()
    {
        LoadNewLevel();
    }

    private void OnDisable()
    {
        TileRegistrator.OnTilesRegistered -= HandleTilesRegistered;
        SpawnPointCreator.OnSpawnPointCreated -= HandleSpawnPointCreated;
        Movement.OnSteppingOnATransitionTile -= ReloadCurrentScene;
    }
    
    
    public void LoadNewLevel()
    {
        ClearEntities();
        TilesRepository.Instance.ClearTiles();
        TileFactory.Instance.ClearTiles();

        // Генерируем комнату и получаем её transform
        Transform roomTransform = TileFactory.Instance.GenerateRoom();

        // Устанавливаем камеру следить за комнатой
        if (_cameraController != null && roomTransform != null)
            _cameraController.SetTarget(roomTransform);
    }
    
    private void HandleTilesRegistered()
    {
        SpawnPointCreator.Instance.CreateSpawnPoint();
        TransitionPointCreator.Instance.CreateTransitionPoint();
    }
    
    private void HandleSpawnPointCreated()
    {
        var player = PlayerFactory.Instance.Generate() as Player;
        
        _cameraController.SetTarget(player.transform);

        EnemyFactory.Instance.SpawnEnemies();
    }

   
    private void ClearEntities()
    {
        var oldPlayer = FindObjectOfType<Player>();
        if (oldPlayer != null)
            Destroy(oldPlayer.gameObject);

        foreach (var enemy in FindObjectsOfType<Enemy>())
            Destroy(enemy.gameObject);
    }

    void ReloadCurrentScene()
    {
        ClearEntities();
        TilesRepository.Instance.ClearTiles();
        TileFactory.Instance.ClearTiles();
        
        var sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
