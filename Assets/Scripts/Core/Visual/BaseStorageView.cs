using System;
using System.Collections.Generic;
using Core.Res;
using Core.Storage;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniversalPool;

namespace Core.Visual
{
  public class BaseStorageView : MonoBehaviour, IStorageView
  {
    [SerializeField] private Transform _container;
    [SerializeField] private VisualData _visualData;
    [SerializeField] private float _yOffset = 0.2f;
    
    private ResId _resId;

    private ResourceView _viewTemplate;

    private List<ResourceView> _activeViews = new();

    public Transform Container => _container;

    public void Initialize(ResId resId)
    {
      _resId = resId;
      
      if (!_visualData.TryGetResourceView(_resId, out _viewTemplate))
      {
        throw new Exception($"Prefab for {_resId} not found");
      }
    }
    
    public PositionForward TopElementPosition()
    {
      var position = _container.position + LocalPositionByIndex(_activeViews.Count);
      return new PositionForward(position, _container.forward);
    }

    public void UpdateCount(int count)
    {
      while (_activeViews.Count < count)
      {
        AddView();
      }

      while (_activeViews.Count > count)
      {
        RemoveLastView();
      }
    }

    public UniTask Add(Vector3 fromWorldPosition, Vector3 fromForward)
    {
      var view = AddView();
      if (view == null)
        return UniTask.CompletedTask;

      view.SetPosition(fromWorldPosition);
      view.SetForward(fromForward);
      var targetLocalPosition = LocalPositionByIndex(_activeViews.Count);
      return view.MoveToLocalPoint(targetLocalPosition, _container.forward, _visualData.AnimationDuration);
    }

    public async UniTask Move(Vector3 position, Vector3 forward)
    {
      var view = _activeViews[^1];
      _activeViews.Remove(view);

      await view.MoveToPoint(position, forward, _visualData.AnimationDuration);
      
      view.gameObject.SetActive(false);
    }

    public void Remove()
    {
      RemoveLastView();
    }

    private ResourceView AddView()
    {
      var position = _container.position + LocalPositionByIndex(_activeViews.Count);
      if (PoolManager.TryGetInstance(out var instance, _viewTemplate, position, _container.forward, _container))
      {
        _activeViews.Add(instance);
        return instance;
      }

      Debug.Log($"Please Fill the pool for {_resId}");
      return null;
    }

    private Vector3 LocalPositionByIndex(int index)
    {
      return Vector3.up * _yOffset * index;
    }

    private void RemoveLastView()
    {
      if (_activeViews.Count == 0)
      {
        Debug.LogWarning($"No views to remove in storage {_resId}");
        return;
      }

      var view = _activeViews[^1];
      _activeViews.Remove(view);

      view.gameObject.SetActive(false);
    }
  }
}