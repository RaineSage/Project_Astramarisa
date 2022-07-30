using UnityEngine;

namespace CharacterCombatHandlers
{
    public class CharacterCombatHandlerPlayer : CharacterCombatHandler
    {
        public bool canAct = false;

        public override void OnTurnBegin()
        {
            Debug.Log("PLAYER TURN (press space or enter to attack)");
            canAct = true;
        }

        public override void OnTurnEnd()
        {
            canAct = false;
            base.OnTurnEnd();
            CombatManagerHandler.Instance.PassTurn();
        }

        public void AttackEnemy()
        {
            // CombatManagerHandler.Instance.BasicAttack(this, CombatManagerHandler.Instance.EnemyCombatHandler);
        }
        

        protected override void OnDeath()
        {
            /*PLACEHOLDER*/
            Debug.Log("You Died");
        }

        public override CharacterAllegiance GetAllegiance()
        {
            return CharacterAllegiance.Player;
        }
    }
}
