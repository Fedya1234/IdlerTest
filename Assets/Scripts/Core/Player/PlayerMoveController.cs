using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InputManager;
using UnityEngine;

namespace Core.Player
{
    public class PlayerMoveController : IDisposable
    {
        private readonly IPlayerUnitView _view;
        private readonly InputController _inputController;
        private readonly PlayerStaticData _staticData;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Transform _cameraTransform;
        
        public Vector3 Position => _view.Position;
        
        public PlayerMoveController(IPlayerUnitView view, InputController inputController, PlayerStaticData staticData)
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main Camera not found in the scene.");
                return;
            }
            
            _cameraTransform = Camera.main.transform;
            _view = view;
            _inputController = inputController;
            _staticData = staticData;
            _cancellationTokenSource = new CancellationTokenSource();
            
            UpdateAsync(_cancellationTokenSource.Token).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
        
        private async UniTaskVoid UpdateAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var inputDirection = GetDirection(_inputController.Move);
                var movedDirection = inputDirection * _staticData.Speed * Time.deltaTime;
                
                _view.Move(movedDirection);
                
                var direction = inputDirection;
                
                var forward = Vector3.Slerp(_view.Direction, direction, _staticData.RotationSpeed * Time.deltaTime);
                _view.SetForward(forward);
                
                await UniTask.Yield(PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }
        }
        
        private Vector3 GetDirection(Vector2 screenDirection)
        {
            var cameraForward = _cameraTransform.forward;
            var cameraRight = _cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            return cameraForward.normalized * screenDirection.y + cameraRight.normalized * screenDirection.x;;
        }
    }
}