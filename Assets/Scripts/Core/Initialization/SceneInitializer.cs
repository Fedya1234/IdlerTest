using System.Collections.Generic;
using Core.Converter;
using Core.Player;
using Core.Visual;
using Idler.Tools;
using InputManager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Core.Initialization
{
  public class SceneInitializer : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ConverterStaticData, ConverterView> _converters = new();
    [SerializeField] private PlayerStaticData _playerStaticData;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private InputController _inputController;
    
    private Disposables _disposables = new ();
    
    private void Start()
    {
      Initialize();
    }

    private void OnDestroy()
    {
      _disposables.Dispose();
    }

    private void Initialize()
    {
      var playerMoveController = new PlayerMoveController(_playerView, _inputController, _playerStaticData);
      _disposables.Register(playerMoveController);
      
      var inventoryState = new PlayerInventoryState(); // LoadFrom save data in future
      var inventoryController = new InventoryController(_playerStaticData.InventorySize, inventoryState, _playerView.InventoryView);
      
      foreach (var (settings, converterView) in _converters)
      {
        var converterState = new ConverterState(); // LoadFrom save data in future
        var converter = new ConverterController(settings, converterState, converterView, inventoryController);
        
        converter.TryStartConversion();
        
        _disposables.Register(converter);
      }
    }
  }
}