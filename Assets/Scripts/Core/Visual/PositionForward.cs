using UnityEngine;

namespace Core.Visual
{
  public struct PositionForward
  {
    public Vector3 Position;
    public Vector3 Forward;
    
    public PositionForward(Vector3 position, Vector3 forward)
    {
      Position = position;
      Forward = forward;
    }
  }
}