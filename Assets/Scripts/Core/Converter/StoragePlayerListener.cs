using System;
using System.Threading;
using Core.Storage;
using Cysharp.Threading.Tasks;
using Tools;

namespace Core.Converter
{
  public class StoragePlayerListener : IDisposable
  {
    private readonly PlayerTrigger _playerTrigger;
    private readonly StorageController _fromStorageController;
    private readonly StorageController _toStorageController;
    private readonly float _transferInterval;

    private CancellationTokenSource _cancellationTokenSource;
    
    public StorageController FromStorageController => _fromStorageController;
    public StorageController ToStorageController => _toStorageController;

    public StoragePlayerListener(PlayerTrigger playerTrigger, StorageController fromStorageController, StorageController toStorageController, float transferInterval)
    {
      _playerTrigger = playerTrigger;
      _fromStorageController = fromStorageController;
      _toStorageController = toStorageController;
      _transferInterval = transferInterval;
      _playerTrigger.EventStateChanged += OnPlayerTriggerStateChanged;
    }

    public void Dispose()
    {
      _playerTrigger.EventStateChanged -= OnPlayerTriggerStateChanged;
    }

    private void OnPlayerTriggerStateChanged(bool isPlayerInside)
    {
      if (isPlayerInside)
      {
        _cancellationTokenSource = new CancellationTokenSource();
        StartTransfer(_cancellationTokenSource.Token)
          .Forget();
      }
      else
      {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
      }
    }
    
    private async UniTask StartTransfer(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        if (_fromStorageController.IsEmpty || _toStorageController.IsFull)
        {
          await UniTask.WaitForSeconds(_transferInterval, cancellationToken: cancellationToken);
          continue;
        }

        var fromPosition = _fromStorageController.View.TopElementPosition();
        _fromStorageController.HideOne();
        _toStorageController.AddAsync(fromPosition.Position, fromPosition.Forward).Forget();
        await UniTask.WaitForSeconds(_transferInterval, cancellationToken: cancellationToken);
      }
    }
  }
}