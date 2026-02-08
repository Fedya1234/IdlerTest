using System.Collections.Generic;
using Core.Res;
using Core.Storage;
using Core.Visual;

namespace Core.Player
{
  public class InventoryController
  {
    private readonly int _size;
    private readonly PlayerInventoryState _state;
    private readonly InventoryView _view;
    private Dictionary<ResId, StorageController> _storages = new();
    
    public InventoryController(int size, PlayerInventoryState state, InventoryView view)
    {
      _size = size;
      _state = state;
      _view = view;
    }
    
    public StorageController GetStorage(ResId resId)
    {
      if (!_storages.ContainsKey(resId))
      {
        _storages[resId] = new StorageController(
          _size,
          _state.GetStorage(resId),
          _view.GetStorageView(resId)
        );
      }
      return _storages[resId];
    }

  }
}