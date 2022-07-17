using CharacterCombatHandlers;
using CombatMenu;
using ScriptableObjects.Definitions;
using UnityEngine;
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

            switch (combatActionTurn.CombatAction.actionType)
            {
                case CombatActionType.Attack:
                    CombatManagerHandler.Instance.Attack(_playerCombatHandler,
                        CombatManagerHandler.Instance.EnemyCombatHandler, combatActionTurn.CombatAction);
                    break;
                case CombatActionType.Heal:
                    CombatManagerHandler.Instance.Heal(_playerCombatHandler, _playerCombatHandler,
                        combatActionTurn.CombatAction);
                    break;
            }

            _playerCombatHandler.canAct = false;
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