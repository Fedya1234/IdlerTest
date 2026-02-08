using System.Collections.Generic;
using Core.Res;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Core.Converter
{
  [CreateAssetMenu(fileName = "ConverterStaticData", menuName = "StaticData/ConverterStaticData")]
  public class ConverterStaticData : SerializedScriptableObject
  {
    [OdinSerialize] private Dictionary<ResId, int> _inputCapacity;
    [OdinSerialize] private Dictionary<ResId, int> _outputCapacity;
    [SerializeField] private float _conversionDuration;
    
    public Dictionary<ResId, int> InputCapacity => _inputCapacity;
    public Dictionary<ResId, int> OutputCapacity => _outputCapacity;
    public float ConversionDuration => _conversionDuration;
  }
}