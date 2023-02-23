using CharacterCombatHandlers;
using UnityEngine.Serialization;

namespace CombatMenu
{
    public class CombatMenuTargetReturnValue : CombatMenuOptionReturnValue
    {
        public CharacterCombatHandler Target { get; set; }
    }
    
    class CombatMenuTargetHandler : CombatMenuOptionHandler
    {
        public CharacterCombatHandler target;
        
        public override CombatMenuOptionReturnValue OnSelect()
        {
            CombatMenuTargetReturnValue returnValue = new CombatMenuTargetReturnValue {Target = target};
            return returnValue;
        }
    }
}