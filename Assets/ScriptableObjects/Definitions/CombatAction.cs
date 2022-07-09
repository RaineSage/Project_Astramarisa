using UnityEngine;

namespace ScriptableObjects.Definitions
{
    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction", order = 2)]
    public class CombatAction : ScriptableObject
    {
        public int power;
    }
}