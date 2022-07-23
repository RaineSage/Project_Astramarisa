using CharacterCombatHandlers;
using CombatMenu;
using ScriptableObjects.Definitions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

            switch (combatActionTurn.CombatAction.actionType)
            {
                case CombatActionType.Attack:
                    Debug.Assert(combatActionTurn.Targets.Count == 1);
                    CharacterCombatHandler enemyCombatHandler = combatActionTurn.Targets[0];
                    CombatManagerHandler.Instance.Attack(_playerCombatHandler,
                        enemyCombatHandler, combatActionTurn.CombatAction);
                    break;
                case CombatActionType.Heal:
                    CombatManagerHandler.Instance.Heal(_playerCombatHandler, _playerCombatHandler,
                        combatActionTurn.CombatAction);
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