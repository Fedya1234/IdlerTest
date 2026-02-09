using UnityEngine;

namespace Core.UI
{
  public class UIContainer : MonoBehaviour
  {
    [SerializeField] private AlertsMenu _alertsMenu;
    
    public AlertsMenu AlertsMenu => _alertsMenu;
  }
}