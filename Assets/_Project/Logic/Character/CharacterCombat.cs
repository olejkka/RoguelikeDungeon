using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private int _attackDamage = 10;
    
    private Character _character;
    
    public int AttackDamage => _attackDamage;
    

    void Awake()
    {
        _character = GetComponent<Character>(); 
    }
    
    public void StartAttack()
    {
        Character target = _character.TargetTile.OccupiedCharacter;

        void OnMovementFinished()
        {
            _character.Animation.PlayAttack(target.transform.position);
            target.Health.TakeDamage(AttackDamage);
            
            Debug.Log($"{target.name} takes damage. {target.Health.CurrentHealth} health");

            _character.Mover.MovementFinished -= OnMovementFinished;
        }

        _character.Mover.MovementFinished += OnMovementFinished;

        _character.Mover.MoveToNearestFloor(_character.TargetTile);
    }
}