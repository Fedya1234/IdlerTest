using System.Collections.Generic;
using Core.Res;
using Core.Storage;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Tools;
using UnityEngine;

namespace Core.Visual
{
  public class ConverterView : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ResId, BaseStorageView> _inputStorages = new();
    [OdinSerialize] private Dictionary<ResId, BaseStorageView> _outputStorages = new();
    [SerializeField] private Transform _buildingTransform;
    [OdinSerialize] private Dictionary<ResId, PlayerTrigger> _inStorageTriggers = new();
    [OdinSerialize] private Dictionary<ResId, PlayerTrigger> _outStorageTriggers = new();
    public Dictionary<ResId, BaseStorageView> InputStorages => _inputStorages;
    public Dictionary<ResId, BaseStorageView> OutputStorages => _outputStorages;
    public Dictionary<ResId, PlayerTrigger> InStorageTriggers => _inStorageTriggers;
    public Dictionary<ResId, PlayerTrigger> OutStorageTriggers => _outStorageTriggers;
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