using System;
using ScriptableObjects;
using ScriptableObjects.Definitions;
using UnityEngine;

namespace CharacterCombatHandlers
{
    public abstract class CharacterCombatHandler : MonoBehaviour
    {
        public enum CharacterAllegiance
        {
            Player,
            Enemy,
            None
        }
        
        public CharacterStats stats;

        private int _attackModifier;
        private int _defenseModifier;
        
        private int _hp;
        private int _mp;

        private bool _isDead;
        
        public abstract void OnTurnBegin();
        public abstract void OnTurnEnd();
        protected abstract void OnDeath();

        public abstract CharacterAllegiance GetAllegiance(); 
        
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
            return stats.baseSpeed;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public string GetName()
        {
            return stats.name;
        }
    }
}
