using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Storage
{
  public class StorageController
  {
    public event Action<StorageController> EventChangeCount;

    private StorageState _state;
    private readonly IStorageView _view;
    private int _capacity;
    private int _previousAmount;

    public int CurrentAmount => _state.Amount;
    public int Capacity => _capacity;
    public int SpaceLeft => _capacity - _state.Amount;
    public float FillPercent => (float) _state.Amount / _capacity;
    public bool IsFull => _state.Amount >= _capacity;
    public bool IsEmpty => _state.Amount <= 0;
    public int PreviousAmount => _previousAmount;

    public StorageController(int capacity, StorageState state, IStorageView view)
    {
      _capacity = capacity;
      _state = state;
      _view = view;
      view.UpdateCount(_state.Amount);
    }

    public async UniTask AddAsync(Vector3 fromPosition)
    {
      await _view.Add(fromPosition);
      SetCount(_state.Amount + 1);
    }
    
    public async UniTask MoveAsync(Vector3 position, Vector3 forward)
    {
      Remove();
      await _view.Move(position, forward);
    }
    
    private void Add()
    {
      SetCount(_state.Amount + 1);
    }

    private void Remove()
    {
      SetCount(_state.Amount - 1);
    }

    private void SetCount(int amount)
    {
      _previousAmount = _state.Amount;
      _state.Amount = amount;
      EventChangeCount?.Invoke(this);
    }
  }
}