public interface ICharacterAction
{
    bool CanExecute(Character actor, Tile target);
    
    void Execute(Character actor, Tile target);
}