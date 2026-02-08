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
    [OdinSerialize] private Dictionary<ResId, ConverterStorageView> _inputStorages = new();
    [OdinSerialize] private Dictionary<ResId, ConverterStorageView> _outputStorages = new();
    [SerializeField] private Transform _buildingTransform;
    public Dictionary<ResId, ConverterStorageView> InputStorages => _inputStorages;
    public Dictionary<ResId, ConverterStorageView> OutputStorages => _outputStorages;
    public Transform BuildingTransform => _buildingTransform;


    public void Initialize()
    {
    }

    public ConverterStorageView GetInputStorageView(ResId resId)
    {
      return _inputStorages[resId];
    }

    public ConverterStorageView GetOutputStorageView(ResId resId)
    {
      return _outputStorages[resId];
    }
  }
}