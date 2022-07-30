using System;
using System.Collections.Generic;
using System.Linq;
using CharacterCombatHandlers;
using JetBrains.Annotations;
using ScriptableObjects.Definitions;
using ScriptableObjects.Definitions.CombatActions;
using UnityEngine;
using UnityEngine.TextCore.Text;
using CombatActionAttack = ScriptableObjects.Definitions.CombatActions.CombatActionAttack;

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

    public CombatActionAttack basicAttack;

    private readonly List<CharacterCombatHandler> _combatHandlers = new List<CharacterCombatHandler>();
    private int _turnIndex;

    private const bool StandaloneCombatHandler = true;

    public GameObject playerCharacter;
    public GameObject[] enemyCharacter;

    private void Start()
    {
        if (!StandaloneCombatHandler) return;

        CharacterCombatHandler playerCombatHandler = playerCharacter.GetComponent<CharacterCombatHandler>();
        CharacterCombatHandler[] enemyCombatHandlers = enemyCharacter
            .Select(enemyObject => enemyObject.GetComponent<CharacterCombatHandler>()).ToArray();

        BeginCombat(playerCombatHandler, enemyCombatHandlers);
    }

    private int TurnOrderFunction(CharacterCombatHandler c1, CharacterCombatHandler c2)
    {
        int compareResult = c2.GetSpeed().CompareTo(c1.GetSpeed());
        if (compareResult != 0)
        {
            return compareResult;
        }

        if (IsMainPlayerCharacter(c1))
        {
            return -1;
        }

        if (IsMainPlayerCharacter(c2))
        {
            return 1;
        }

        return compareResult;
    }

    public void BeginCombat(CharacterCombatHandler playerCombatHandler, CharacterCombatHandler[] enemyCombatHandlers)
    {
        _combatHandlers.AddRange(enemyCombatHandlers);
        _combatHandlers.Add(playerCombatHandler);
        _combatHandlers.Sort(TurnOrderFunction);

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

    public int CalculateBasePower(CharacterCombatHandler user, CharacterCombatHandler target, PowerFactor[] powerFactors)
    {
        int power = 0;
        foreach (PowerFactor powerFactor in powerFactors)
        {
            switch (powerFactor.powerFactorType)
            {
                case PowerFactorType.Flat:
                    power += (int) powerFactor.powerAmount;
                    break;
                case PowerFactorType.Scaled:
                    int statToScale = 0;
                    switch (powerFactor.statType)
                    {
                        case StatType.None:
                            Debug.LogError("None stat type with scaled power should not happen.");
                            break;
                        case StatType.Attack:
                            statToScale = user.GetAttack();
                            break;
                        case StatType.Magic:
                            Debug.LogError("Magic stat type not yet implemented.");
                            break;
                        case StatType.Defense:
                            statToScale = user.GetDefense();
                            break;
                        case StatType.Resistance:
                            Debug.LogError("Resistance stat type not yet implemented.");
                            break;
                        case StatType.Speed:
                            statToScale = user.GetSpeed();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    power += (int) (statToScale * powerFactor.powerAmount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return power;
    }


    public int CalculateResistances(CharacterCombatHandler user, CharacterCombatHandler target,
        int basePower, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Physical:
                basePower -= target.GetDefense();
                break;
            case DamageType.Magic:
                Debug.LogError("Magic damage type not yet implemented.");
                break;
            case DamageType.True:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Math.Max(basePower, 0);
    }

    public int NonNegativePower(int basePower)
    {
        return Math.Max(basePower, 0);
    }
    

    public void Attack(CharacterCombatHandler attacker, CharacterCombatHandler defender, CombatActionAttack combatAction)
    {
        int damage = CalculateBasePower(attacker, defender, combatAction.damageFactors);
        damage = CalculateResistances(attacker, defender, damage, combatAction.damageType);
        damage = NonNegativePower(damage);
        
        bool defenderIsDead = defender.TakeDamage(damage);
        Debug.Log($"{defender.stats.name} HP is at {defender.GetHp()}/{defender.GetMaxHp()}");
        if (defenderIsDead)
        {
            if (GetMainPlayerCharacter().IsDead())
            {
                OnGameOver();
            }
            else if (GetEnemyCharacters().Count == 0)
            {
                OnPlayerVictory();
            }
        }
    }

    public void Heal(CharacterCombatHandler healer, CharacterCombatHandler target, CombatActionHeal combatAction)
    {
        int healingAmount = CalculateBasePower(healer, target, combatAction.healFactors);
        healingAmount = NonNegativePower(healingAmount);
        target.Heal(healingAmount);
        Debug.Log($"{target.stats.name} HP is at {target.GetHp()}/{target.GetMaxHp()}");
    }

    public void OnPlayerVictory()
    {
        /*PLACEHOLDER*/
        Debug.Log("You Won!");
        Destroy(this.gameObject);
    }

    public void OnGameOver()
    {
        /*PLACEHOLDER*/
        Debug.Log("Game Over");
        Destroy(this.gameObject);
    }

    public static bool IsMainPlayerCharacter(CharacterCombatHandler characterCombatHandler)
    {
        return characterCombatHandler.GetAllegiance() == CharacterCombatHandler.CharacterAllegiance.Player;
    }

    public CharacterCombatHandler GetMainPlayerCharacter()
    {
        return _combatHandlers.FindLast(IsMainPlayerCharacter);
    }

    [ItemCanBeNull]
    public List<CharacterCombatHandler> GetEnemyCharacters()
    {
        return GetEnemyCharacters(false);
    }

    public List<CharacterCombatHandler> GetEnemyCharacters(bool findDead)
    {
        return _combatHandlers.FindAll(c =>
            c.GetAllegiance() == CharacterCombatHandler.CharacterAllegiance.Enemy && c.IsDead() == findDead);
    }
}