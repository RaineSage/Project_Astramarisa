using ScriptableObjects.Definitions;
using ScriptableObjects.Definitions.CombatActions;
using UnityEngine;
using UnityEngine.UI;

namespace CombatMenu
{
    public class CombatActionMenuReturnValue : CombatMenuOptionReturnValue
    {
        public CombatAction CombatAction { get; set; }
    }
    
    public class CombatMenuActionHandler : CombatMenuOptionHandler
    {
        public CombatAction combatAction;

        public override CombatMenuOptionReturnValue OnSelect()
        {
            CombatActionMenuReturnValue returnValue = new CombatActionMenuReturnValue {CombatAction = combatAction};
            return returnValue;
        }
    }
}
