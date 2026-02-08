using Cysharp.Threading.Tasks;
using Tools.Extensions;
using UnityEngine;

namespace Core.Visual
{
  public class ResourceView : MonoBehaviour
  {
    public void SetPosition(Vector3 position)
    {
      transform.position = position;
    }
    
    public void SetForward(Vector3 forward)
    {
      transform.forward = forward;
    }

    public UniTask MoveToLocalPoint(Vector3 localPoint, Vector3 forward, float duration)
    {
      return transform.LocalMoveToAsync(localPoint, forward, duration, this.GetCancellationTokenOnDestroy());
    }

    public UniTask MoveToPoint(Vector3 point, Vector3 forward, float duration)
    {
      return transform.MoveToAsync(point, forward, duration, this.GetCancellationTokenOnDestroy());
    }
  }
}