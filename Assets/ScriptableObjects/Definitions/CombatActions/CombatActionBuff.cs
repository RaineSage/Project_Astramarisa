using UnityEngine;

namespace ScriptableObjects.Definitions.CombatActions
{
    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction/Buff", order = 3)]
    public class CombatActionBuff : CombatAction
    {
        public BuffPowerFactor[] buffPowerFactors;
    }
}
