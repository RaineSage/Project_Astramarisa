using UnityEngine;

namespace ScriptableObjects.Definitions
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        public new string name;
        
        public int maxHp;
        public int maxMp;

        public int baseAttack;
        public int baseDefense;

        public int baseMagic;
        public int baseResistance;
        
        public int baseSpeed;
    }
}
