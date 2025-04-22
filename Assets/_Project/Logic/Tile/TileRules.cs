public static class TileRules
{
    public static bool IsWalkable(Tile tile)
    {
        return tile.Type == TileType.Floor || tile.Type == TileType.Transition || tile.Type == TileType.Spawn;
    }
}