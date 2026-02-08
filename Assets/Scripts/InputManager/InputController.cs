using System;
using UnityEngine;

namespace InputManager
{
    [CreateAssetMenu(menuName = "Input/InputController")]
    public class InputController : ScriptableObject
    {
        public event Action EventMovePointerDown;
        public event Action EventMovePointerUp;
        
        [NonSerialized] private Vector2 _move;
        
        
        public void OnMove(Vector2 input)
        {
            _move = input;
        }

        public void OnMovePointerDown()
        {
            EventMovePointerDown?.Invoke();
        }

        public void OnMovePointerUp()
        {
            EventMovePointerUp?.Invoke();
            _move = Vector2.zero;
        }
    }
}