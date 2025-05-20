using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string _mainScene = "Main";

    [Header("Prefabs")]
    [SerializeField] private GameObject _tileFactoryPrefab;
    [SerializeField] private GameObject _playerFactoryPrefab;
    [SerializeField] private GameObject _enemyFactoryPrefab;
    [SerializeField] private GameObject _spawnPointCreatorPrefab;
    [SerializeField] private GameObject _transitionPointCreatorPrefab;
    [SerializeField] private GameObject _tileHighlighterPrefab;
    [SerializeField] private GameObject _tilesRepositoryPrefab;
    [SerializeField] private GameObject _gameStateMachine;

    public static Bootstrapper Instance { get; private set; }

    public TileFactory TileFactory { get; private set; }
    public PlayerFactory PlayerFactory { get; private set; }
    public EnemyFactory EnemyFactory { get; private set; }
    public SpawnPointCreator SpawnPointCreator { get; private set; }
    public TransitionPointCreator TransitionPointCreator { get; private set; }
    public TileHighlighter TileHighlighter { get; private set; }
    public TilesRepository TilesRepository { get; private set; }
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        TileFactory = InstantiatePrefab<TileFactory>(_tileFactoryPrefab, nameof(TileFactory));
        PlayerFactory = InstantiatePrefab<PlayerFactory>(_playerFactoryPrefab, nameof(PlayerFactory));
        EnemyFactory = InstantiatePrefab<EnemyFactory>(_enemyFactoryPrefab, nameof(EnemyFactory));
        SpawnPointCreator = InstantiatePrefab<SpawnPointCreator>(_spawnPointCreatorPrefab, nameof(SpawnPointCreator));
        TransitionPointCreator = InstantiatePrefab<TransitionPointCreator>(_transitionPointCreatorPrefab, nameof(TransitionPointCreator));
        TileHighlighter = InstantiatePrefab<TileHighlighter>(_tileHighlighterPrefab, nameof(TileHighlighter));
        TilesRepository = InstantiatePrefab<TilesRepository>(_tilesRepositoryPrefab, nameof(TilesRepository));
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(_mainScene) == false)
        {
            SceneManager.LoadScene(_mainScene);
        }
        else
        {
            Debug.LogError("Bootstrapper: Main Scene is not specified.");
        }
    }
    
    private T InstantiatePrefab<T>(GameObject prefab, string name) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError($"Bootstrapper: Prefab для {typeof(T).Name} не назначен.");
            return null;
        }
        
        var obj = Instantiate(prefab);
        obj.name = name;
        DontDestroyOnLoad(obj);

        var component = obj.GetComponent<T>();
        
        if (component == null)
            Debug.LogError($"Bootstrapper: Префаб {prefab.name} не содержит компонент {typeof(T).Name}.");

        return component;
    }
}
