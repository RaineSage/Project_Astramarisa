using UnityEngine;

namespace ScriptableObjects.Definitions
{
    public enum CombatActionType
    {
        Attack,
        Heal,
        
        [InspectorName(null)]
        Count
    }
    
    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction", order = 2)]
    public class CombatAction : ScriptableObject
    {
        public int power;
        public CombatActionType actionType;
    }
}