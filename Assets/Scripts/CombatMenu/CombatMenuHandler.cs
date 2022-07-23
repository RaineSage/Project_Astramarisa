using System;
using System.Collections.Generic;
using CharacterCombatHandlers;
using JetBrains.Annotations;
using ScriptableObjects.Definitions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace CombatMenu
{
    public struct CombatActionTurn
    {
        public bool IsComplete;
        public CombatAction CombatAction;
        public List<CharacterCombatHandler> Targets;
    }

    public class CombatMenuHandler : MonoBehaviour
    {
        [FormerlySerializedAs("combatMenuOptions")]
        public List<GameObject> rootCombatMenuOptions;

        public GameObject targetMenuOptionPrefab;

        private CombatMenuOptionHandler[] _combatMenuOptionHandlers;

        private int _currentOptionIndex = 0;
        private CombatActionTurn _playerTurn = new CombatActionTurn();

        public GameObject[] optionLocationNodes;
        private object List;

        public void MoveRow(int numRows)
        {
        }

        public void MoveCols(int numCols)
        {
            int prevIndex = _currentOptionIndex;
            _currentOptionIndex = (numCols + _currentOptionIndex + _combatMenuOptionHandlers.Length) % _combatMenuOptionHandlers.Length;
            UpdateCurrentOption(prevIndex);
        }

        private void UpdateCurrentOption(int prevOptionIndex)
        {
            _combatMenuOptionHandlers[prevOptionIndex].OnExit();
            _combatMenuOptionHandlers[_currentOptionIndex].OnEnter();
        }

        public CombatActionTurn SelectOption()
        {
            CombatActionTurn toReturn = _playerTurn;
            CombatMenuOptionReturnValue returnValue = _combatMenuOptionHandlers[_currentOptionIndex].OnSelect();

            // As we add more potential return values, this switch statement will expand and we will use
            // the details of the combat action to determine our menu flow
            switch (returnValue)
            {
                case CombatActionMenuReturnValue combatActionMenuReturnValue:
                    _playerTurn.CombatAction = combatActionMenuReturnValue.CombatAction;

                    ClearMenu();

                    List<CharacterCombatHandler> targets;
                    switch (_playerTurn.CombatAction.actionType)
                    {
                        case CombatActionType.Attack:
                            targets = CombatManagerHandler.Instance.GetEnemyCharacters();
                            break;
                        case CombatActionType.Heal:
                            targets = new List<CharacterCombatHandler>()
                            {
                                CombatManagerHandler.Instance.GetMainPlayerCharacter()
                            };
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    List<CombatMenuTargetHandler> targetMenuOptions = new List<CombatMenuTargetHandler>();

                    Debug.Assert(targets.Count <= optionLocationNodes.Length);

                    for (int i = 0; i < targets.Count; i++)
                    {
                        CharacterCombatHandler target = targets[i];

                        GameObject targetMenuOption = Instantiate(targetMenuOptionPrefab,
                            optionLocationNodes[i].transform.position, Quaternion.identity, transform);

                        Debug.Assert(target != null, nameof(target) + " != null");
                        targetMenuOption.GetComponent<Text>().text = target.GetName();

                        CombatMenuTargetHandler menuTargetHandler =
                            targetMenuOption.GetComponent<CombatMenuTargetHandler>();
                        menuTargetHandler.target = target;

                        targetMenuOptions.Add(menuTargetHandler);
                    }

                    _combatMenuOptionHandlers = targetMenuOptions.ToArray();
                    _currentOptionIndex = 0;
                    _combatMenuOptionHandlers[_currentOptionIndex].OnEnter();
                    break;
                case CombatMenuTargetReturnValue combatMenuOptionReturnValue:
                    _playerTurn.Targets = new List<CharacterCombatHandler>() {combatMenuOptionReturnValue.Target};
                    _playerTurn.IsComplete = true;
                    ClearMenu();
                    break;
            }

            return _playerTurn;
        }

        private void ClearMenu()
        {
            foreach (CombatMenuOptionHandler optionHandler in _combatMenuOptionHandlers)
            {
                Destroy(optionHandler.gameObject);
            }
        }

        private void Awake()
        {
            ResetMenu();
        }

        public void OnEndTurn()
        {
            ResetMenu();
        }

        private void ResetMenu()
        {
            ClearCombatActionTurn();
            _currentOptionIndex = 0;

            List<CombatMenuOptionHandler> combatMenuOptionHandlersList = new List<CombatMenuOptionHandler>();
            for (int i = 0; i < rootCombatMenuOptions.Count; i++)
            {
                GameObject optionHandlerGameObjectPrefab = rootCombatMenuOptions[i];
                Debug.Assert(combatMenuOptionHandlersList != null, nameof(combatMenuOptionHandlersList) + " != null");
                GameObject optionHandlerGameObject = Instantiate(optionHandlerGameObjectPrefab,
                    optionLocationNodes[i].transform.position, Quaternion.identity, transform);
                combatMenuOptionHandlersList.Add(optionHandlerGameObject.GetComponent<CombatMenuOptionHandler>());
            }

            Debug.Assert(combatMenuOptionHandlersList != null, nameof(combatMenuOptionHandlersList) + " != null");
            _combatMenuOptionHandlers = combatMenuOptionHandlersList.ToArray();
            _combatMenuOptionHandlers[_currentOptionIndex].OnEnter();
        }

        void ClearCombatActionTurn()
        {
            _playerTurn = new CombatActionTurn();
        }
    }
}