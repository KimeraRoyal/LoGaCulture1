using System;
using UnityEngine;

namespace LUTE.EventHandlers
{
    public class HandledButtons : MonoBehaviour
    {
        public Action<string> OnButtonPressed;

        public void RegisterButton(HandledButton _button)
        {
            _button.OnPressed += () => Call(_button.gameObject.name);
        }
        
        private void Call(string _buttonName)
            => OnButtonPressed?.Invoke(_buttonName);
    }
}