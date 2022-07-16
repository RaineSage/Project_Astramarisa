using System;
using ScriptableObjects;
using ScriptableObjects.Definitions;
using UnityEngine;

namespace CharacterCombatHandlers
{
    public abstract class CharacterCombatHandler : MonoBehaviour
    {
        public CharacterStats stats;

        private int _attackModifier;
        private int _defenseModifier;
        
        private int _hp;
        private int _mp;
        
        public abstract void OnTurnBegin();
        public abstract void OnTurnEnd();
        protected abstract void OnDeath();

        private void Start()
        {
            /* We don't want to do this for the player. We likely want to have an additional ScriptableObject that extends CharacterStats to keep track of the player's HP. */
            _hp = stats.maxHp;
            _mp = stats.maxMp;
        }

        public int GetAttack()
        {
            return stats.baseAttack + _attackModifier;
        }
        
        public int GetDefense()
        {
            return stats.baseDefense + _defenseModifier;
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
            bool isDead = _hp <= 0;
            if (isDead)
            {
                OnDeath();
            }

            return isDead;
        }

        public void Heal(int healAmount)
        {
            _hp = Math.Min(GetMaxHp(), healAmount + _hp);
        }
    }
}
