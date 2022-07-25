using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Definitions
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
        Speed
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


    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction", order = 2)]
    public class CombatAction : ScriptableObject
    {
        public CombatActionType actionType;
        public TargetType targetType;
        public TargetSpread targetSpread;

        public PowerFactor[] powerFactors;
        public DamageType damageType;
        public ElementType elementType;
    }
}