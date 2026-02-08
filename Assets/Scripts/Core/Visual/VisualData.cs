using System.Collections.Generic;
using Core.Res;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Visual
{
  [CreateAssetMenu(fileName = "VisualData", menuName = "StaticData/VisualData")]
  public class VisualData : SerializedScriptableObject
  {
    [SerializeField] private float _animationDuration;
    [SerializeField] private Dictionary<ResId, ResourceView> _resPrefabs = new();
    
    public float AnimationDuration => _animationDuration;
    
    public bool TryGetResourceView(ResId resId, out ResourceView prefab)
    {
      return _resPrefabs.TryGetValue(resId, out prefab);
    }
  }
}