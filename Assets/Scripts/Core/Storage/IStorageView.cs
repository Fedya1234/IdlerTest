using Core.Res;
using Core.Visual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Storage
{
  public interface IStorageView
  {
    public PositionForward TopElementPosition();
    public void Initialize(ResId resId);
    public void UpdateCount(int count);
    public UniTask Add(Vector3 fromPosition, Vector3 fromForward);
    public UniTask Move(Vector3 position, Vector3 forward);
    public void Remove();
  }
}