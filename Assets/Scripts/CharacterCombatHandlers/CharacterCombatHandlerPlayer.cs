using UnityEngine;

namespace CharacterCombatHandlers
{
    public class CharacterCombatHandlerPlayer : CharacterCombatHandler
    {
        public bool canAct = false;

        public override void OnTurnBegin()
        {
            canAct = true;
        }

        public override void OnTurnEnd()
        {
            canAct = false;
            CombatManagerHandler.Instance.PassTurn();
        }

        public void AttackEnemy()
        {
            CombatManagerHandler.Instance.BasicAttack(this, CombatManagerHandler.Instance.EnemyCombatHandler);
        }
        

        protected override void OnDeath()
        {
            /*PLACEHOLDER*/
            Debug.Log("You Died");
        }
    }
}
