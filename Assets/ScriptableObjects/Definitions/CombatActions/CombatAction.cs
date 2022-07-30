using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Definitions.CombatActions
{
    public enum CombatActionType
    {
        Attack,
        Heal,

        [InspectorName(null)] Count
    }

    public enum TargetType
    {
        Enemy,
        Ally
    }

    public enum TargetSpread
    {
        Single,
        All
    }

    public enum StatType
    {
        None,
        Attack,
        Magic,
        Defense,
        Resistance,
        Speed,
        Luck
    }

    public enum PowerFactorType
    {
        Flat,
        Scaled
    }

    public enum DamageType
    {
        Physical,
        Magic,
        True
    }

    public enum ElementType
    {
        Neutral,
        Wind,
        Earth,
        Water,
        Fire,
        Time,
        Space,
        Light,
        Dark,
        Poison
    }
    
    [Serializable]
    public class PowerFactor
    {
        public PowerFactorType powerFactorType;
        public float powerAmount;
        public StatType statType;
    }

    // Left as separate from damage/heal power factors in case of deviation in the future
    [Serializable]
    public class BuffPowerFactor
    {
        public PowerFactorType powerFactorType;
        public float powerAmount;
        public StatType statType;
        public int turnDuration;
    }

    
    public class CombatAction : ScriptableObject
    {
        public TargetType targetType;
        public TargetSpread targetSpread;

        public ElementType elementType;
    }

}