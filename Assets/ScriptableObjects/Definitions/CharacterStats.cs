using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/CharacterStats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        public string name;
        
        public int maxHp;
        public int maxMp;

        public int baseAttack;
        public int baseDefense;

    }
}
