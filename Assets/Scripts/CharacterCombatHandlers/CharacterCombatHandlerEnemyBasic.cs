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
            CharacterCombatHandler playerCombatHandler = CombatManagerHandler.Instance.GetMainPlayerCharacter();
            CombatManagerHandler.Instance.BasicAttack(this, playerCombatHandler);
            OnTurnEnd();
        }

        public override void OnTurnBegin()
        {
            Debug.Log("ENEMY TURN");
            _shouldAct = true;
        }
    

        public override void OnTurnEnd()
        {
            _shouldAct = false;
            base.OnTurnEnd();
            CombatManagerHandler.Instance.PassTurn();
        }

        protected override void OnDeath()
        {
            /*PLACEHOLDER*/
            Debug.Log("Enemy has died");
        }

        public override CharacterAllegiance GetAllegiance()
        {
            return CharacterAllegiance.Enemy;
        }
    }
}
