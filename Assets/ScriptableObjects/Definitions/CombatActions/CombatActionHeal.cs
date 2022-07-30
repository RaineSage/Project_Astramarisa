using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Definitions.CombatActions
{
    [CreateAssetMenu(fileName = "CombatAction", menuName = "ScriptableObjects/CombatAction/Heal", order = 2)]
    public class CombatActionHeal : CombatAction
    {
        public PowerFactor[] healFactors;
    }
}