using CharacterCombatHandlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ControlHandlers
{
    public class CombatControlHandler : MonoBehaviour
    {
        private CharacterCombatHandlerPlayer _playerCombatHandler;
        private void Start()
        {
            _playerCombatHandler = GetComponent<CharacterCombatHandlerPlayer>();
        }

        void OnSelect(InputValue input)
        {
            if (!_playerCombatHandler.canAct)
            {
                return;
            }

            _playerCombatHandler.canAct = false;
            _playerCombatHandler.AttackEnemy();
        }
    }
}
