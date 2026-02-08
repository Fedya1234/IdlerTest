using Core.Res;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Storage
{
  public interface IStorageView
  {
    public void Initialize(ResId resId);
    public void UpdateCount(int count);
    public UniTask Add(Vector3 fromPosition);
    public UniTask Move(Vector3 position, Vector3 forward);
    public void Remove();
  }
}