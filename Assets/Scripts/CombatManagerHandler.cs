using System;
using CharacterCombatHandlers;
using UnityEngine;

public class CombatManagerHandler : MonoBehaviour
{
    private enum Turn
    {
        Player,
        Enemy,
        None
    }

    private static CombatManagerHandler _instance;

    public static CombatManagerHandler Instance => _instance;


    private Turn _currentTurn;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
        }
    }

    public GameObject playerCharacter;
    public GameObject enemyCharacter;
    
    [NonSerialized]
    public CharacterCombatHandler PlayerCombatHandler;
    [NonSerialized]
    public CharacterCombatHandler EnemyCombatHandler;
    
    private void Start()
    {
        PlayerCombatHandler = playerCharacter.GetComponent<CharacterCombatHandler>();
        EnemyCombatHandler = enemyCharacter.GetComponent<CharacterCombatHandler>();
        
        _currentTurn = Turn.Player;
        PlayerCombatHandler.OnTurnBegin();
        Debug.Log("PLAYER TURN (press space or enter to attack)");
    }

    public void PassTurn()
    {
        switch (_currentTurn)
        {
            case Turn.Player:
            {
                _currentTurn = Turn.Enemy;
                Debug.Log("ENEMY TURN");
                EnemyCombatHandler.OnTurnBegin();
                break;
            }
            case Turn.Enemy:
            {
                _currentTurn = Turn.Player;
                Debug.Log("PLAYER TURN");
                PlayerCombatHandler.OnTurnBegin();
                break;
            }
            default:
            {
                Debug.LogError("Should not reach");
                break;
            }
        }
    }
    
    public void BasicAttack(CharacterCombatHandler attacker, CharacterCombatHandler defender)
    {
        int damage = Math.Max(0, attacker.GetAttack() - defender.GetDefense());
        bool defenderIsDead = defender.TakeDamage(damage);
        Debug.Log($"{defender.stats.name} HP is at {defender.GetHp()}/{defender.GetMaxHp()}");
        if (defenderIsDead)
        {
            OnBattleFinished();
        }
    }

    public void OnBattleFinished()
    {
        /*PLACEHOLDER*/
        Debug.Log("Battle Finished");
        Destroy(this.gameObject);
    }
}
