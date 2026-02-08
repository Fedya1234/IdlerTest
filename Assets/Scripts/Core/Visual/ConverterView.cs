using System.Collections.Generic;
using Core.Res;
using Core.Storage;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Core.Visual
{
  public class ConverterView : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ResId, BaseStorageView> _inputStorages = new();
    [OdinSerialize] private Dictionary<ResId, BaseStorageView> _outputStorages = new();
    [SerializeField] private Transform _buildingTransform;
    
    
    public Dictionary<ResId, BaseStorageView> InputStorages => _inputStorages;
    public Dictionary<ResId, BaseStorageView> OutputStorages => _outputStorages;
    public Transform BuildingTransform => _buildingTransform;


    public void Initialize()
    {
      
    }
    
    public IStorageView GetInputStorageView(ResId resId)
    {
      return _inputStorages[resId];
    }
    
    public IStorageView GetOutputStorageView(ResId resId)
    {
      return _outputStorages[resId];
    }
  }
}