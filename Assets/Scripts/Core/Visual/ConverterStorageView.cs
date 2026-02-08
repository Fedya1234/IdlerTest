using Tools;
using UnityEngine;

namespace Core.Visual
{
  public class ConverterStorageView : MonoBehaviour
  {
    [SerializeField] private BaseStorageView _storageView;
    [SerializeField] private PlayerTrigger _playerTrigger;
    
    public BaseStorageView StorageView => _storageView;
    public PlayerTrigger PlayerTrigger => _playerTrigger;
  }
}