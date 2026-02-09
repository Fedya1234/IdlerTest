using System.Collections.Generic;
using Core.Converter;
using Core.Player;
using Core.UI;
using Core.Visual;
using Idler.Tools;
using InputManager;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UniversalPool;

namespace Core.Initialization
{
  public class SceneInitializer : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ConverterStaticData, ConverterView> _converters = new();
    [SerializeField] private PlayerStaticData _playerStaticData;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private InputController _inputController;
    [SerializeField] private VisualData _visualData;
    [SerializeField] private UIContainer _uiContainer;

    private Disposables _disposables = new();

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
      foreach (var (prefab, count) in _visualData.PoolSizes)
        PoolManager.FillPool(prefab, count);

      var playerMoveController = new PlayerMoveController(_playerView, _inputController, _playerStaticData);
      _disposables.Register(playerMoveController);

      var inventoryState = new PlayerInventoryState(); // LoadFrom save data in future
      var inventoryController =
        new InventoryController(_playerStaticData.InventorySize, inventoryState, _playerView.InventoryView);

      var converters = new List<ConverterController>();
      foreach (var (settings, converterView) in _converters)
      {
        var converterState = new ConverterState(); // LoadFrom save data in future
        var converter = new ConverterController(settings, converterState, converterView, inventoryController);
        converters.Add(converter);

        _disposables.Register(converter);
      }

      var convertersStatesController = new ConvertersStatesController(converters, _uiContainer.AlertsMenu);
      _disposables.Register(convertersStatesController);

      foreach (var converterController in converters)
        converterController.TryStartConversion();
    }
  }
}