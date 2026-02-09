using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Player;
using Core.Res;
using Core.Storage;
using Core.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Converter
{
  public class ConverterController : IDisposable
  {
    public event Action<ConverterStateData> EventStateDataChanged; 
    public event Action<ResId> EventInputStorageChanged;
    public event Action<ResId> EventOutputStorageChanged;
    public event Action EventConversionStarted;
    public event Action EventConversionCompleted;

    private readonly ConverterStaticData _staticData;
    private readonly ConverterState _state;
    private readonly ConverterView _view;
    private readonly InventoryController _inventoryController;

    private Dictionary<ResId, StoragePlayerListener> _inputStoragePlayerListeners = new();
    private Dictionary<ResId, StoragePlayerListener> _outputStoragePlayerListeners = new();

    public List<ResId> InputResourceIds => new(_inputStoragePlayerListeners.Keys);
    public List<ResId> OutputResourceIds => new(_outputStoragePlayerListeners.Keys);

    private bool IsConverting => _conversionCancellationTokenSource != null;
    private CancellationTokenSource _conversionCancellationTokenSource;

    public ConverterController(ConverterStaticData staticData, ConverterState state, ConverterView view,
      InventoryController inventoryController)
    {
      _staticData = staticData;
      _state = state;
      _view = view;
      _inventoryController = inventoryController;

      foreach (var (resId, storageStaticData) in _staticData.InputCapacity)
      {
        if (!_state.InputStorage.ContainsKey(resId))
          _state.InputStorage[resId] = new StorageState();

        var storageState = _state.InputStorage[resId];
        var storageView = _view.GetInputStorageView(resId);
        storageView.StorageView.Initialize(resId);
        var storageController = new StorageController(storageStaticData, storageState, storageView.StorageView);
        storageController.EventChangeCount += OnStorageCountChanged;
        
        var storagePlayerListener = new StoragePlayerListener(storageView.PlayerTrigger,
          _inventoryController.GetStorage(resId), storageController, _staticData.TransferInterval);

        _inputStoragePlayerListeners.Add(resId, storagePlayerListener);
      }

      foreach (var (resId, storageStaticData) in _staticData.OutputCapacity)
      {
        if (!_state.OutputStorages.ContainsKey(resId))
          _state.OutputStorages[resId] = new StorageState();

        var storageState = _state.OutputStorages[resId];
        var storageView = _view.GetOutputStorageView(resId);
        storageView.StorageView.Initialize(resId);
        var storageController = new StorageController(storageStaticData, storageState, storageView.StorageView);
        storageController.EventChangeCount += OnStorageCountChanged;
        
        var storagePlayerListener = new StoragePlayerListener(storageView.PlayerTrigger, storageController,
          _inventoryController.GetStorage(resId), _staticData.TransferInterval);

        _outputStoragePlayerListeners.Add(resId, storagePlayerListener);
      }

      _view.Initialize();
    }

    public void Dispose()
    {
      foreach (var storage in _inputStoragePlayerListeners.Values)
      {
        storage.ToStorageController.EventChangeCount -= OnStorageCountChanged;
        storage.Dispose();
      }

      foreach (var storage in _outputStoragePlayerListeners.Values)
      {
        storage.FromStorageController.EventChangeCount -= OnStorageCountChanged;
        storage.Dispose();
      }

      _inputStoragePlayerListeners.Clear();
      _outputStoragePlayerListeners.Clear();
    }

    private void OnStorageCountChanged(StorageController storageController)
    {
      CheckConverterState();
    }
    
    private void CheckConverterState()
    {
      IsOutputFull(out var fullOutputs);
      IsInputEmpty(out var emptyInputs);

      EventStateDataChanged?.Invoke(new ConverterStateData(OutputResourceIds.FirstOrDefault(), fullOutputs, emptyInputs));
    }

    public bool IsOutputFull(out List<ResId> fullOutputs)
    {
      fullOutputs = new List<ResId>();
      foreach (var (resId, storage) in _outputStoragePlayerListeners)
      {
        if (storage.FromStorageController.IsFull)
          fullOutputs.Add(resId);
      }

      return fullOutputs.Count > 0;
    }

    public bool IsInputEmpty(out List<ResId> emptyInputs)
    {
      emptyInputs = new List<ResId>();
      foreach (var (resId, storage) in _inputStoragePlayerListeners)
      {
        if (storage.ToStorageController.IsEmpty)
          emptyInputs.Add(resId);
      }

      return emptyInputs.Count > 0;
    }

    public bool IsCanConvert() => IsInputEmpty(out var _) == false && IsOutputFull(out var _) == false;

    public void TryStartConversion()
    {
      if (IsConverting)
        return;

      CheckConverterState();
      ConversionLoop().Forget();
    }

    private async UniTaskVoid ConversionLoop()
    {
      try
      {
        _conversionCancellationTokenSource = new CancellationTokenSource();
        while (!_conversionCancellationTokenSource.IsCancellationRequested)
        {
          if (IsCanConvert() == false)
          {
            await UniTask.WaitForSeconds(0.1f, cancellationToken: _conversionCancellationTokenSource.Token);
            continue;
          }

          List<UniTask> tasks = new();
          foreach (var storageController in _inputStoragePlayerListeners.Values)
            tasks.Add(storageController.ToStorageController.MoveAsync(_view.BuildingTransform.position,
              _view.BuildingTransform.forward));

          await UniTask.WhenAll(tasks);

          tasks.Clear();

          EventConversionStarted?.Invoke();

          await UniTask.WaitForSeconds(_staticData.ConversionDuration,
            cancellationToken: _conversionCancellationTokenSource.Token);

          foreach (var storageController in _outputStoragePlayerListeners.Values)
            tasks.Add(storageController.FromStorageController.AddAsync(_view.BuildingTransform.position,
              _view.BuildingTransform.forward));

          await UniTask.WhenAll(tasks);

          EventConversionCompleted?.Invoke();

          UpdateViews();
        }
      }
      catch (OperationCanceledException) when (_conversionCancellationTokenSource.IsCancellationRequested)
      {
        
      }

    }

    private void UpdateViews()
    {
    }
  }
}