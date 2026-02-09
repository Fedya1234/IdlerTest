using System.Collections.Generic;
using Core.Res;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Core.UI
{
  public class AlertsMenu : SerializedMonoBehaviour
  {
    [OdinSerialize] private Dictionary<ResId, AlertIcon> _alertIcons = new();
    
    public bool TryGetAlertIcon(ResId resId, out AlertIcon alertIcon) => 
      _alertIcons.TryGetValue(resId, out alertIcon);
    
  }
}