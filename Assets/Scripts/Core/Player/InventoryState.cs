using System.Collections.Generic;
using Core.Res;
using Core.Storage;
using Sirenix.Serialization;

namespace Core.Player
{
  public class InventoryState
  {
    [OdinSerialize] private Dictionary<ResId, StorageState> _storages = new();
    
    public StorageState GetStorage(ResId resId)
    {
      if (!_storages.ContainsKey(resId))
      {
        _storages[resId] = new StorageState();
      }
      return _storages[resId];
    }
  }
}