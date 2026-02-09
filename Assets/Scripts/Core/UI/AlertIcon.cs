using TMPro;
using UnityEngine;

namespace Core.UI
{
  public interface IAlertIcon
  {
    AlertIcon SetText(string text);
    void SetActive(bool isActive);
  }
  
  public class AlertIcon : MonoBehaviour, IAlertIcon
  {
    [SerializeField] private TMP_Text _alertText;
    
    public AlertIcon SetText(string text)
    {
      _alertText.text = text;
      return this;
    }
    
    public void SetActive(bool isActive)
    {
      gameObject.SetActive(isActive);
    }
  }
}