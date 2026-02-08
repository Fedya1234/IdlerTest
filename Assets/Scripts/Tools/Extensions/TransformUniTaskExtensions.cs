using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tools.Extensions
{
  public static class TransformUniTaskExtensions
  {
    /// <summary>
    /// Linearly moves transform from current position to targetPosition over duration seconds.
    /// </summary>
    public static async UniTask MoveToAsync(
      this Transform transform,
      Vector3 targetPosition,
      Vector3 forwardDirection,
      float duration,
      CancellationToken cancellationToken = default)
    {
      if (transform == null)
        throw new ArgumentNullException(nameof(transform));

      if (duration <= 0f)
      {
        transform.position = targetPosition;
        return;
      }

      var start = transform.position;
      var forwardStart = transform.forward;
      var t = 0f;

      while (t < 1f && !cancellationToken.IsCancellationRequested)
      {
        t += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(start, targetPosition, t);
        transform.forward = Vector3.Lerp(forwardStart, forwardDirection, t);

        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
      }

      if (!cancellationToken.IsCancellationRequested)
        transform.position = targetPosition;
    }

    public static async UniTask LocalMoveToAsync(
      this Transform transform,
      Vector3 targetLocalPosition,
      Vector3 forwardDirection,
      float duration,
      CancellationToken cancellationToken = default)
    {
      if (transform == null)
        throw new ArgumentNullException(nameof(transform));

      if (duration <= 0f)
      {
        transform.localPosition = targetLocalPosition;
        transform.forward = forwardDirection;
        return;
      }

      var start = transform.localPosition;
      var forwardStart = transform.forward;
      var t = 0f;

      while (t < 1f && !cancellationToken.IsCancellationRequested)
      {
        t += Time.deltaTime / duration;
        transform.localPosition = Vector3.Lerp(start, targetLocalPosition, t);
        transform.forward = Vector3.Lerp(forwardStart, forwardDirection, t);

        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
      }

      if (!cancellationToken.IsCancellationRequested)
        transform.localPosition = targetLocalPosition;
    }
  }
}