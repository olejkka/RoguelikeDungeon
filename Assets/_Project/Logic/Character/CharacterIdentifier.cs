
public static class CharacterIdentifier
{
    public static bool IsPlayer(Character character)
    {
        return character is Player;
    }
    
    public static bool IsEnemy(Character source, Character target)
    {
        return (source is Player && target is Enemy) ||
               (source is Enemy && target is Player);
    }
}
