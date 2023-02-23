using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using ScriptableObjects.Definitions;
using ScriptableObjects.Definitions.CombatActions;
using UnityEngine;

namespace CharacterCombatHandlers
{
    public class BuffData
    {
        public BuffPowerFactor Buff;
        public int TurnsLeft;
    }

    public abstract class CharacterCombatHandler : MonoBehaviour
    {
        public enum CharacterAllegiance
        {
            Player,
            Enemy,
            None
        }

        public CharacterStats stats;

        private int _hp;
        private int _mp;

        private bool _isDead;

        private List<BuffData> _buffs = new List<BuffData>();

        public abstract void OnTurnBegin();

        public virtual void OnTurnEnd()
        {
            foreach (BuffData buffData in _buffs)
            {
                buffData.TurnsLeft -= 1;
            }

            _buffs = _buffs.FindAll(buff => buff.TurnsLeft > 0);
        }

        protected abstract void OnDeath();

        public abstract CharacterAllegiance GetAllegiance();

        private void Start()
        {
            /* We don't want to do this for the player. We likely want to have an additional ScriptableObject that extends CharacterStats to keep track of the player's HP. */
            _hp = stats.maxHp;
            _mp = stats.maxMp;
        }

        public void ApplyBuffs(BuffPowerFactor[] buffPowerFactors)
        {
            foreach (BuffPowerFactor buffPowerFactor in buffPowerFactors)
            {
                BuffData newBuffData = new()
                {
                    Buff = buffPowerFactor,
                    TurnsLeft = buffPowerFactor.turnDuration
                };
                _buffs.Add(newBuffData);
            }
        }

        public int BaseStatWithBuffs(StatType stat, int baseStat)
        {
            List<BuffData> statBuffs = _buffs.FindAll(buff => buff.Buff.statType == stat).ToList();
            List<BuffData> scaledBuffs =
                statBuffs.FindAll(buff => buff.Buff.powerFactorType == PowerFactorType.Flat).ToList();
            List<BuffData> flatBuffs =
                statBuffs.FindAll(buff => buff.Buff.powerFactorType == PowerFactorType.Flat).ToList();

            float runningStatTotal = scaledBuffs.Aggregate<BuffData, float>(baseStat,
                (current, buffPowerFactor) => current * buffPowerFactor.Buff.powerAmount);

            runningStatTotal += flatBuffs.Sum(buffPowerFactor => buffPowerFactor.Buff.powerAmount);

            return (int) runningStatTotal;
        }

        public int GetAttack()
        {
            return BaseStatWithBuffs(StatType.Attack, stats.baseAttack);
        }

        public int GetDefense()
        {
            return BaseStatWithBuffs(StatType.Defense, stats.baseDefense);
        }

        public int GetHp()
        {
            return _hp;
        }

        public int GetMaxHp()
        {
            return stats.maxHp;
        }

        public bool TakeDamage(int damageAmount)
        {
            _hp = Math.Max(0, _hp -= damageAmount);
            _isDead = _hp <= 0;
            if (_isDead)
            {
                OnDeath();
            }

            return _isDead;
        }

        public void Heal(int healAmount)
        {
            _hp = Math.Min(GetMaxHp(), healAmount + _hp);
        }

        public int GetSpeed()
        {
            return BaseStatWithBuffs(StatType.Speed, stats.baseSpeed);
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public string GetName()
        {
            return stats.name;
        }

        public int GetMagicAttack()
        {
            return BaseStatWithBuffs(StatType.Magic, stats.baseMagic);
        }

        public int GetResistance()
        {
            return BaseStatWithBuffs(StatType.Resistance, stats.baseResistance);
        }
    }
}