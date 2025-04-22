using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Генератор произвольного расположения комнаты. Гарантирует минимум _minSize плит.
/// </summary>
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
        _minSize = Mathf.Clamp(minSize, 1, rows * cols);
        _maxSize = Mathf.Max(maxSize, _minSize);
        _fillProbability = Mathf.Clamp01(fillProbability);
    }

    public TileType[,] Generate()
    {
        TileType[,] map = new TileType[_cols, _rows];
        // Инициализируем пустотой
        for (int x = 0; x < _cols; x++)
            for (int z = 0; z < _rows; z++)
                map[x, z] = TileType.Empty;

        Vector2Int start = new Vector2Int(_cols / 2, _rows / 2);
        int targetSize = Random.Range(_minSize, _maxSize + 1);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);

        // Случайное заполнение
        while (floor.Count < targetSize && queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!InBounds(current) || floor.Contains(current))
                continue;

            floor.Add(current);
            map[current.x, current.y] = TileType.Floor;

            foreach (var dir in Directions())
            {
                if (Random.value < _fillProbability)
                    queue.Enqueue(current + dir);
            }
        }

        // Если получили меньше, чем минимальный порог, добираем соседями
        if (floor.Count < _minSize)
        {
            int needed = _minSize - floor.Count;
            // формируем множество соседних пустых клеток
            var frontier = new HashSet<Vector2Int>();
            foreach (var cell in floor)
            {
                foreach (var dir in Directions())
                {
                    var nbr = cell + dir;
                    if (InBounds(nbr) && map[nbr.x, nbr.y] == TileType.Empty)
                        frontier.Add(nbr);
                }
            }
            // добавляем из frontier случайно до нужного количества
            var frontierList = new List<Vector2Int>(frontier);
            while (needed > 0 && frontierList.Count > 0)
            {
                int idx = Random.Range(0, frontierList.Count);
                var pos = frontierList[idx];
                frontierList.RemoveAt(idx);

                floor.Add(pos);
                map[pos.x, pos.y] = TileType.Floor;

                needed--;
                // расширяем frontier новыми соседями
                foreach (var dir in Directions())
                {
                    var nbr = pos + dir;
                    if (InBounds(nbr) && map[nbr.x, nbr.y] == TileType.Empty && !frontierList.Contains(nbr))
                        frontierList.Add(nbr);
                }
            }
        }

        // Обводим стены вокруг пола
        foreach (var pos in floor)
        {
            foreach (var dir in Directions())
            {
                var nbr = pos + dir;
                if (InBounds(nbr) && map[nbr.x, nbr.y] == TileType.Empty)
                    map[nbr.x, nbr.y] = TileType.Wall;
            }
        }

        return map;
    }

    private bool InBounds(Vector2Int pos)
        => pos.x >= 0 && pos.x < _cols && pos.y >= 0 && pos.y < _rows;

    private static IEnumerable<Vector2Int> Directions()
    {
        yield return Vector2Int.up;
        yield return Vector2Int.down;
        yield return Vector2Int.left;
        yield return Vector2Int.right;
    }
}
