using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighborTilesSelection", menuName = "NeighborTilesSelection/NeighborTilesSelection")]
public class NeighborTilesSelectionSO : ScriptableObject
{
    [System.Serializable]
    public class NeighborRule
    {
        public int rectangleWidth = 0;
        public int rectangleHeight = 0;
    }

    public List<NeighborRule> neighborRules = new List<NeighborRule>();

    public List<Vector2Int> GetOffsets()
    {
        List<Vector2Int> offsets = new List<Vector2Int>();

        foreach (var rule in neighborRules)
        {
            for (int x = 0; x <= rule.rectangleWidth; x++)
            {
                for (int y = 0; y <= rule.rectangleHeight; y++)
                {
                    if (x != 0 || y != 0)
                    {
                        offsets.Add(new Vector2Int(x, y));
                        offsets.Add(new Vector2Int(-x, y));
                        offsets.Add(new Vector2Int(x, -y));
                        offsets.Add(new Vector2Int(-x, -y));
                    }
                }
            }
        }

        return offsets;
    }
}