using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveLogic : MonoBehaviour
{
    private Character character;
    private bool needsHighlight = true;


    private void OnEnable()
    {
        PlayerFactory.OnPlayerCreated += OnPlayerCreatedHandler;
    }

    private void Update()
    {
        HighlightAvailableToMoveTiles();
    }

    private void OnDisable()
    {
        PlayerFactory.OnPlayerCreated -= OnPlayerCreatedHandler;
    }

    void OnPlayerCreatedHandler()
    {
        character = FindObjectOfType<Player>();
        
    }
    
    public void HighlightAvailableToMoveTiles()
    {
        if (character.CurrentTile == null)
        {
            Debug.LogWarning($"[FigureLogic] Character {character.gameObject.name} не может найти текущую клетку!");
            return;
        }

        List<Tile> moves = CharacterMoveService.GetAvailableToMoveTiles(character);
        

        TileHighlightService.HighlightTiles(character, moves);
    }
}
