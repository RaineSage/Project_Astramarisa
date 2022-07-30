using CharacterCombatHandlers;
using CombatMenu;
using ScriptableObjects.Definitions.CombatActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlHandlers
{
    public class CombatControlHandler : MonoBehaviour
    {
        public GameObject combatMenuObject;
        private CombatMenuHandler _combatMenuHandler;
        private CharacterCombatHandlerPlayer _playerCombatHandler;

        private void Start()
        {
            _combatMenuHandler = combatMenuObject.GetComponent<CombatMenuHandler>();
            _playerCombatHandler = GetComponent<CharacterCombatHandlerPlayer>();
        }

        void OnSelect(InputValue input)
        {
            if (!_playerCombatHandler.canAct)
            {
                return;
            }

            CombatActionTurn combatActionTurn = _combatMenuHandler.SelectOption();
            if (!combatActionTurn.IsComplete) return;

            switch (combatActionTurn.CombatAction)
            {
                case CombatActionAttack combatActionAttack:
                    Debug.Assert(combatActionTurn.Targets.Count == 1);
                    CharacterCombatHandler enemyCombatHandler = combatActionTurn.Targets[0];
                    CombatManagerHandler.Instance.Attack(_playerCombatHandler,
                        enemyCombatHandler, combatActionAttack);
                    break;
                case CombatActionHeal combatActionHeal:
                    Debug.Assert(combatActionTurn.Targets.Count == 1);
                    CombatManagerHandler.Instance.Heal(_playerCombatHandler, combatActionTurn.Targets[0],
                        combatActionHeal);
                    break;
                case CombatActionBuff combatActionBuff:
                    Debug.Assert(combatActionTurn.Targets.Count == 1);
                    CombatManagerHandler.Instance.Buff(_playerCombatHandler, combatActionTurn.Targets[0],
                        combatActionBuff);
                    break;
            }

            _playerCombatHandler.canAct = false;

            _combatMenuHandler.OnEndTurn();
            _playerCombatHandler.OnTurnEnd();
        }

        void OnMenuRight(InputValue input)
        {
            if (!_playerCombatHandler.canAct)
            {
                return;
            }

            _combatMenuHandler.MoveCols(1);
        }

        void OnMenuLeft(InputValue input)
        {
            if (!_playerCombatHandler.canAct)
            {
                return;
            }

            _combatMenuHandler.MoveCols(-1);
        }
    }
}