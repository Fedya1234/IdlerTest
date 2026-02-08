using UnityEngine;

namespace Core.Player
{
  [CreateAssetMenu(fileName = "PlayerStaticData", menuName = "StaticData/PlayerStaticData")]
  public class PlayerStaticData : ScriptableObject
  {
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _inventorySize;

    public float Speed => _speed;
    public int InventorySize => _inventorySize;
    public float RotationSpeed => _rotationSpeed;
  }
}