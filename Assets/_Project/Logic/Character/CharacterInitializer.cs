using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    public static CharacterInitializer Instance { get; private set; }

    
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
        PlayerFactory.OnPlayerCreated += OnPlayerCreatedHandler;
    }

    private void OnDisable()
    {
        PlayerFactory.OnPlayerCreated -= OnPlayerCreatedHandler;
    }

    void OnPlayerCreatedHandler()
    {
        SetCurrentTile();
    }

    private void SetCurrentTile()
    {
        Character character = FindObjectOfType<Character>();
        if (character == null)
        {
            Debug.LogError("Character не найден на сцене.");
            return;
        }

        Vector2Int position = new Vector2Int(
            Mathf.RoundToInt(character.transform.position.x),
            Mathf.RoundToInt(character.transform.position.z)
        );

        Tile tile = TilesRepository.Instance.GetTileAt(position);
        if (tile == null)
        {
            Debug.LogError($"Tile не найден в позиции {position}");
            return;
        }

        character.CurrentTile = tile;
        tile.OccupiedCharacter = character;
    }
}