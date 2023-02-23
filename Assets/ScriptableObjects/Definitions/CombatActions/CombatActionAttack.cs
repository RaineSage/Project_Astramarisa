using UnityEngine;

namespace ScriptableObjects.Definitions.CombatActions
{
    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction/Attack", order = 1)]
    public class CombatActionAttack : CombatAction
    {
        public PowerFactor[] damageFactors;
        public DamageType damageType;
    }
}