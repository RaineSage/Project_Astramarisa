using UnityEngine;
using UnityEngine.UI;

namespace CombatMenu
{
    public abstract class CombatMenuOptionReturnValue
    {
    }

    public abstract class CombatMenuOptionHandler : MonoBehaviour
    {
        private Text _text;

        void Awake()
        {
            _text = GetComponent<Text>();
        }


        public void OnEnter()
        {
            _text.fontStyle = FontStyle.Bold;
        }

        public void OnExit()
        {
            _text.fontStyle = FontStyle.Normal;
        }

        public abstract CombatMenuOptionReturnValue OnSelect();
    }
}
