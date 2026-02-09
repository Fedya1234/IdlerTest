using System.Collections.Generic;
using Core.Res;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Core.UI
{
  public interface IAlertsMenu
  {
    bool TryGetAlertIcon(ResId resId, out IAlertIcon alertIcon);
  }

  public class AlertsMenu : SerializedMonoBehaviour, IAlertsMenu
  {
    [OdinSerialize] private Dictionary<ResId, AlertIcon> _alertIcons = new();
    
    public bool TryGetAlertIcon(ResId resId, out IAlertIcon alertIcon)
    {
      if (!_alertIcons.ContainsKey(resId))
      {
        alertIcon = null;
        return false;
      }
      
      alertIcon = _alertIcons[resId];
      return true;
    }
  }
}