using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool _isWall;
    [SerializeField] private GameObject _spawnPoint;
    private bool _isSpawnPoint;
    
    public Vector2 Position { get; private set; }
    public Enemy Enemy { get; set; }
    public bool IsSpawnPoint 
    {
        get
        {
            return _isSpawnPoint;
        }
        set
        {
            _isSpawnPoint = value;
        }
    }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        Position = transform.position;
    }
    
    public bool TryGetSpawnPoint(out Transform point)
    {
        point = _spawnPoint != null ? _spawnPoint.transform : null;
        return point != null;
    }
}