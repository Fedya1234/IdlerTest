using UnityEngine;

namespace Core.Player
{
  public interface IPlayerUnitView
  {
    Vector3 Position { get; }
    Vector3 Direction { get; }
    void Move(Vector3 direction);
    void SetForward(Vector3 forward);
  }
}