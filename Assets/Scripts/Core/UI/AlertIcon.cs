using TMPro;
using UnityEngine;

namespace Core.UI
{
  public class AlertIcon : MonoBehaviour
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