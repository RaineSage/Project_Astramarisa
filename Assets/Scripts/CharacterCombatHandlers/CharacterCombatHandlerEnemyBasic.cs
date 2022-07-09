using UnityEngine;

namespace CharacterCombatHandlers
{
    public class CharacterCombatHandlerEnemyBasic : CharacterCombatHandler
    {
        private bool _shouldAct = false;

        // Update is called once per frame
        private void Update()
        {
            if (!_shouldAct)
            {
                return;
            }
        
            _shouldAct = false;
            CombatManagerHandler.Instance.BasicAttack(this, CombatManagerHandler.Instance.PlayerCombatHandler);
            OnTurnEnd();
        }

        public override void OnTurnBegin()
        {
            _shouldAct = true;
        }
    

        public override void OnTurnEnd()
        {
            _shouldAct = false;
            CombatManagerHandler.Instance.PassTurn();
        }

        protected override void OnDeath()
        {
            /*PLACEHOLDER*/
            Debug.Log("Enemy has died");
        }
    }
}
