using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ScriptableObjects.Definitions;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace CombatMenu
{
    public struct CombatActionTurn
    {
        public CombatAction CombatAction;
    }
    
    public class CombatMenuHandler : MonoBehaviour
    {
        public List<GameObject> combatMenuOptions;
        private CombatMenuOptionHandler[] _combatMenuOptionHandlers;

        private int _currentOptionIndex = 0;
        
        public struct CombatMenuData
        {
            [CanBeNull] public CombatAction CombatAction;
        }

        public void MoveRow(int numRows)
        {
            
        }

        public void MoveCols(int numCols)
        {
            int prevIndex = _currentOptionIndex;
            _currentOptionIndex = 1 - _currentOptionIndex;
            UpdateCurrentOption(prevIndex);
        }

        public void UpdateCurrentOption(int prevOptionIndex)
        {
            _combatMenuOptionHandlers[prevOptionIndex].OnExit();
            _combatMenuOptionHandlers[_currentOptionIndex].OnEnter();
        }

        public CombatActionTurn SelectOption()
        {
            CombatActionTurn toReturn = new CombatActionTurn();
            CombatMenuOptionReturnValue returnValue = _combatMenuOptionHandlers[_currentOptionIndex].OnSelect();
            
            // As we add more potential return values, this switch statement will expand and we will use
            // the details of the combat action to determine our menu flow
            switch (returnValue)
            {
                case CombatActionMenuReturnValue combatActionMenuReturnValue:
                    toReturn.CombatAction = combatActionMenuReturnValue.CombatAction;
                    break;
            }

            return toReturn;
        }
        
        private void Awake()
        {
            List<CombatMenuOptionHandler> combatMenuOptionHandlersList = new List<CombatMenuOptionHandler>();
            foreach (GameObject optionHandlerGameObject in combatMenuOptions)
            {
                Debug.Assert(combatMenuOptionHandlersList != null, nameof(combatMenuOptionHandlersList) + " != null");
                combatMenuOptionHandlersList.Add(optionHandlerGameObject.GetComponent<CombatMenuOptionHandler>());
            }

            Debug.Assert(combatMenuOptionHandlersList != null, nameof(combatMenuOptionHandlersList) + " != null");
            _combatMenuOptionHandlers = combatMenuOptionHandlersList.ToArray();
        }

        // Start is called before the first frame update
        void Start()
        {
            _combatMenuOptionHandlers[_currentOptionIndex].OnEnter();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
