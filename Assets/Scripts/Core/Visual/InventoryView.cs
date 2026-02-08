using System;
using System.Collections.Generic;
using Core.Res;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Core.Visual
{
  public class InventoryView : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ResId, BaseStorageView> _storageViews = new();
    
    public BaseStorageView GetStorageView(ResId resId)
    {
      if (!_storageViews.ContainsKey(resId))
      {
        throw new Exception($"Storage view for {resId} not found in InventoryView.");
      }
      return _storageViews[resId];
    }
    
  }
}