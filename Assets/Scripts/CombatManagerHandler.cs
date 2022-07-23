using System;
using System.Collections.Generic;
using CharacterCombatHandlers;
using JetBrains.Annotations;
using ScriptableObjects.Definitions;
using UnityEngine;

public class CombatManagerHandler : MonoBehaviour
{
    public static CombatManagerHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public CombatAction basicAttack;

    private readonly List<CharacterCombatHandler> _combatHandlers = new List<CharacterCombatHandler>();
    private int _turnIndex;

    private const bool StandaloneCombatHandler = true;

    public GameObject playerCharacter;
    public GameObject enemyCharacter;

    private void Start()
    {
        if (!StandaloneCombatHandler) return;

        CharacterCombatHandler playerCombatHandler = playerCharacter.GetComponent<CharacterCombatHandler>();
        CharacterCombatHandler enemyCombatHandler = enemyCharacter.GetComponent<CharacterCombatHandler>();

        BeginCombat(playerCombatHandler, new[] {enemyCombatHandler});
    }

    public void BeginCombat(CharacterCombatHandler playerCombatHandler, CharacterCombatHandler[] enemyCombatHandlers)
    {
        _combatHandlers.AddRange(enemyCombatHandlers);
        _combatHandlers.Add(playerCombatHandler);
        _combatHandlers.Sort((c1, c2) => c2.GetSpeed().CompareTo(c1.GetSpeed()));

        _combatHandlers[_turnIndex].OnTurnBegin();
    }

    public void PassTurn()
    {
        do
        {
            _turnIndex = (_turnIndex + 1) % _combatHandlers.Count;
        } while (_combatHandlers[_turnIndex].IsDead());
        
        _combatHandlers[_turnIndex].OnTurnBegin();
    }

    public void BasicAttack(CharacterCombatHandler attacker, CharacterCombatHandler defender)
    {
        Attack(attacker, defender, basicAttack);
    }

    public void Attack(CharacterCombatHandler attacker, CharacterCombatHandler defender, CombatAction combatAction)
    {
        int damage = Math.Max(0, combatAction.power + attacker.GetAttack() - defender.GetDefense());
        bool defenderIsDead = defender.TakeDamage(damage);
        Debug.Log($"{defender.stats.name} HP is at {defender.GetHp()}/{defender.GetMaxHp()}");
        if (defenderIsDead)
        {
            OnBattleFinished();
        }
    }

    public void Heal(CharacterCombatHandler healer, CharacterCombatHandler target, CombatAction combatAction)
    {
        int healingAmount = combatAction.power;
        target.Heal(healingAmount);
        Debug.Log($"{target.stats.name} HP is at {target.GetHp()}/{target.GetMaxHp()}");
    }

    public void OnBattleFinished()
    {
        /*PLACEHOLDER*/
        Debug.Log("Battle Finished");
        Destroy(this.gameObject);
    }

    public CharacterCombatHandler GetMainPlayerCharacter()
    {
        return _combatHandlers.FindLast(c => c.GetAllegiance() == CharacterCombatHandler.CharacterAllegiance.Player);
    }

    [ItemCanBeNull]
    public List<CharacterCombatHandler> GetEnemyCharacters()
    {
        return GetEnemyCharacters(false);
    }
    
    public List<CharacterCombatHandler> GetEnemyCharacters(bool findDead)
    {
        return _combatHandlers.FindAll(c => c.GetAllegiance() == CharacterCombatHandler.CharacterAllegiance.Enemy && c.IsDead() == findDead);
    }
}