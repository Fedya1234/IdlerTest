using System;
using System.Collections.Generic;
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
    public event Action<ResId> EventInputStorageChanged;
    public event Action<ResId> EventOutputStorageChanged;
    public event Action EventConversionStarted;
    public event Action EventConversionCompleted;

    private readonly ConverterStaticData _staticData;
    private readonly ConverterState _state;
    private readonly ConverterView _view;
    private readonly InventoryController _inventoryController;

    private Dictionary<ResId, StorageController> _inputStorages = new();
    private Dictionary<ResId, StorageController> _outputStorages = new();

    public List<ResId> InputResourceIds => new(_inputStorages.Keys);
    public List<ResId> OutputResourceIds => new(_outputStorages.Keys);
    
    private bool IsConverting => _conversionCancellationTokenSource != null;
    private CancellationTokenSource _conversionCancellationTokenSource;

    public ConverterController(ConverterStaticData staticData, ConverterState state, ConverterView view, InventoryController inventoryController)
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
        storageView.Initialize(resId);
        var storageController = new StorageController(storageStaticData, storageState, storageView);
        storageController.EventChangeCount += OnInStorageCountChanged;
        
        _inputStorages.Add(resId, storageController);
      }

      foreach (var (resId, storageStaticData) in _staticData.OutputCapacity)
      {
        if (!_state.OutputStorages.ContainsKey(resId))
          _state.OutputStorages[resId] = new StorageState();

        var storageState = _state.OutputStorages[resId];
        var storageView = _view.GetOutputStorageView(resId);
        storageView.Initialize(resId);
        var storageController = new StorageController(storageStaticData, storageState, storageView);
        storageController.EventChangeCount += OnOutStorageCountChanged;
        _outputStorages.Add(resId, storageController);
      }
      
      _view.Initialize();
      
    }

    public void Dispose()
    {
      foreach (var storage in _inputStorages.Values)
        storage.EventChangeCount -= OnInStorageCountChanged;

      foreach (var storage in _outputStorages.Values)
        storage.EventChangeCount -= OnOutStorageCountChanged;

      _inputStorages.Clear();
      _outputStorages.Clear();
      
      
    }

    public bool IsOutputFull(out List<ResId> fullOutputs)
    {
      fullOutputs = new List<ResId>();
      foreach (var (resId, storage) in _outputStorages)
      {
        if (storage.IsFull)
          fullOutputs.Add(resId);
      }

      return fullOutputs.Count > 0;
    }

    public bool IsInputEmpty(out List<ResId> emptyInputs)
    {
      emptyInputs = new List<ResId>();
      foreach (var (resId, storage) in _inputStorages)
      {
        if (storage.IsEmpty)
          emptyInputs.Add(resId);
      }

      return emptyInputs.Count > 0;
    }
    
    public bool IsCanConvert() => IsInputEmpty(out var _) == false && IsOutputFull(out var _) == false;
    
    public void TryStartConversion()
    {
      if (IsConverting)
        return;
      
      if (IsCanConvert() == false)
        return;
      
      ConversionLoop().Forget();
    }
    
    private async UniTaskVoid ConversionLoop()
    {
      var buildingPosition = _view.BuildingTransform.position;
      try
      {
        _conversionCancellationTokenSource = new CancellationTokenSource();
        while (IsCanConvert())
        {
          List<UniTask> tasks = new();
          foreach (var storageController in _inputStorages.Values) 
            tasks.Add(storageController.MoveAsync(buildingPosition, _view.BuildingTransform.forward));
          
          await UniTask.WhenAll(tasks);
          
          tasks.Clear();
          
          EventConversionStarted?.Invoke();
          
          await UniTask.WaitForSeconds(_staticData.ConversionDuration, cancellationToken: _conversionCancellationTokenSource.Token);
          
          foreach (var storageController in _outputStorages.Values) 
            tasks.Add(storageController.AddAsync(buildingPosition));
          
          await UniTask.WhenAll(tasks);
          
          EventConversionCompleted?.Invoke();

          UpdateViews();
        }
      }
      catch (OperationCanceledException) when (_conversionCancellationTokenSource.IsCancellationRequested)
      {
        
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
        throw;
      }
    }

    private void UpdateViews()
    {
      
    }
    
    private void OnPlayerInStateChanged(bool isPlayerInside)
    {
      
    }

    private void OnPlayerOutStateChanged(bool isPlayerInside)
    {
      
    }

    private void OnInStorageCountChanged(StorageController storageController)
    {
      
    }

    private void OnOutStorageCountChanged(StorageController storageController)
    {
      
    }
  }
}