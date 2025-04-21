public static class MoveCalculatorFactory
{
    public static MoveCalculator Create(NeighborTilesSelectionSO settings)
    {
        return new UnblockableMoveCalculator();
    }
}