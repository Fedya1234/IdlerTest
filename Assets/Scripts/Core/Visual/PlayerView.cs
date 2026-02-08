using Core.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Visual
{
  public class PlayerView : MonoBehaviour, IPlayerUnitView
  {
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] private Transform _toRotate;
    [SerializeField] private InventoryView _inventoryView;
    public Vector3 Position => transform.position;
    public Vector3 Direction => _toRotate.forward;
    public InventoryView InventoryView => _inventoryView;

    private void Awake()
    {
      _agent.updateRotation = false;
    }

    public virtual void Move(Vector3 direction)
    {
      if (_agent.isActiveAndEnabled)
        _agent.Move(direction);
    }

    public void SetForward(Vector3 forward)
    {
      _toRotate.forward = forward;
    }
  }
}