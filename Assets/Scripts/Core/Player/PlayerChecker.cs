using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Player
{
  public class PlayerChecker : IDisposable
  {
    public event Action<bool> EventStateChanged;

    private readonly float _range;
    private readonly Vector3 _position;
    private readonly float _checkInterval;
    private readonly PlayerMoveController _moveController;

    private CancellationTokenSource _cancellationTokenSource = new();
    private bool _state;

    public bool State => _state;

    public PlayerChecker(float range, Vector3 position, float checkInterval, PlayerMoveController moveController)
    {
      _range = range;
      _position = position;
      _checkInterval = checkInterval;
      _moveController = moveController;

      CheckLoop().Forget();
    }

    public void Dispose()
    {
      _cancellationTokenSource?.Cancel();
      _cancellationTokenSource?.Dispose();
      _cancellationTokenSource = null;
    }

    private async UniTaskVoid CheckLoop()
    {
      while (!_cancellationTokenSource.IsCancellationRequested)
      {
        await UniTask.WaitForSeconds(_checkInterval, cancellationToken: _cancellationTokenSource.Token);

        if (_cancellationTokenSource.IsCancellationRequested)
          break;

        var isInRange = Vector3.SqrMagnitude(_moveController.Position - _position) <= _range * _range;

        if (isInRange == _state)
          return;

        _state = isInRange;
        EventStateChanged?.Invoke(_state);
      }
    }
  }
}