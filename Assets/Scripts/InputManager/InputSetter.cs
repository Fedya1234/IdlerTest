using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager
{
    public class InputSetter : MonoBehaviour, InputSystem_Actions.IPlayerActions
    {
        [SerializeField] InputController _inputController;
        [SerializeField] private Joystick _leftJoystick; 

        private bool _isMoving;
        
        private InputSystem_Actions m_Actions;
        private InputSystem_Actions.PlayerActions m_Player;
        
        private void Awake()
        {
            m_Actions = new InputSystem_Actions();
            m_Player = m_Actions.Player;
            m_Player.AddCallbacks(this);   
        }

        private void OnDestroy()
        {
            m_Actions.Dispose();
        }

        private void OnEnable()
        {
            _leftJoystick.EventOnPointerDown += LeftJoystickPointerDown;
            _leftJoystick.EventOnPointerUp += LeftJoystickPointerUp;

            m_Player.Enable();
        }
        
        private void OnDisable()
        {
            _leftJoystick.EventOnPointerDown -= LeftJoystickPointerDown;
            _leftJoystick.EventOnPointerUp -= LeftJoystickPointerUp;

            m_Player.Disable();
        }

        private void Update()
        {
            if (_isMoving == false)
                return;
            
            _inputController.OnMove(_leftJoystick.Direction);
        }

        private void LeftJoystickPointerDown()
        {
            _isMoving = true;
            _inputController.OnMovePointerDown();
        }
        
        private void LeftJoystickPointerUp()
        {
            _isMoving = false;
            _inputController.OnMovePointerUp();
        }
        
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var move = context.ReadValue<Vector2>();
            
            if (context.started)
                _inputController.OnMovePointerDown();
            else if (context.canceled)
                _inputController.OnMovePointerUp();
            else
                _inputController.OnMove(move);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
           
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }
    }
}