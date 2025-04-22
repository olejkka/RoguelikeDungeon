using System.Collections.Generic;
using UnityEngine;

public class RandomRoomLayoutGenerator
{
    private int _rows;
    private int _cols;
    private int _minSize;
    private int _maxSize;
    private float _fillProbability;

    public RandomRoomLayoutGenerator(int rows, int cols, int minSize, int maxSize, float fillProbability = 0.7f)
    {
        _rows = rows;
        _cols = cols;
        _minSize = minSize;
        _maxSize = maxSize;
        _fillProbability = Mathf.Clamp01(fillProbability);
    }

    public TileType[,] Generate()
    {
        TileType[,] map = new TileType[_cols, _rows];

        for (int x = 0; x < _cols; x++)
        {
            for (int z = 0; z < _rows; z++)
            {
                map[x, z] = TileType.Empty;
            }
        }

        Vector2Int start = new Vector2Int(_cols / 2, _rows / 2);
        int roomSize = Random.Range(_minSize, _maxSize);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        while (floor.Count < roomSize && queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (!InBounds(current) || floor.Contains(current)) continue;

            floor.Add(current);
            map[current.x, current.y] = TileType.Floor;

            foreach (Vector2Int dir in Directions())
            {
                if (Random.value < _fillProbability)
                    queue.Enqueue(current + dir);
            }
        }

        foreach (var pos in floor)
        {
            foreach (Vector2Int dir in Directions())
            {
                Vector2Int neighbor = pos + dir;
                if (InBounds(neighbor) && map[neighbor.x, neighbor.y] == TileType.Empty)
                    map[neighbor.x, neighbor.y] = TileType.Wall;
            }
        }

        return map;
    }

    private bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _cols && pos.y >= 0 && pos.y < _rows;
    }

    private static IEnumerable<Vector2Int> Directions()
    {
        yield return Vector2Int.up;
        yield return Vector2Int.down;
        yield return Vector2Int.left;
        yield return Vector2Int.right;
    }
}
