using System;
using System.Collections.Generic;
using Core.UI;
using UnityEngine;

namespace Core.Converter
{
  public class ConvertersStatesController : IDisposable
  {
    private readonly List<ConverterController> _converters;
    private readonly IAlertsMenu _alertsMenu;

    public ConvertersStatesController(List<ConverterController> converters, IAlertsMenu alertsMenu)
    {
      _converters = converters;
      _alertsMenu = alertsMenu;

      foreach (var converterController in _converters)
        converterController.EventStateDataChanged += OnConverterStateChanged;
    }

    public void Dispose()
    {
      foreach (var converterController in _converters)
        converterController.EventStateDataChanged -= OnConverterStateChanged;
    }

    private void OnConverterStateChanged(ConverterStateData converterStateData)
    {
      if (_alertsMenu.TryGetAlertIcon(converterStateData.MainOutputId, out var alertIcon) == false)
      {
        Debug.LogError($"Alert icon for resource {converterStateData.MainOutputId} not found!");
        return;
      }

      var inputMessage = converterStateData.EmptyInputs.Count > 0
        ? "\n Empty inputs: " + string.Join(", ", converterStateData.EmptyInputs)
        : string.Empty;

      var outputMessage = converterStateData.FullOutputs.Count > 0
        ? "\n Full outputs: " + string.Join(", ", converterStateData.FullOutputs)
        : string.Empty;

      var alertMessage = $"Converter stopped!{inputMessage}{outputMessage}";

      alertIcon
        .SetText(alertMessage)
        .SetActive(converterStateData.IsStopped);
    }
  }
}