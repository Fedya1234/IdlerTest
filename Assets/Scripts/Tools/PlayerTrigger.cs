using System;
using UnityEngine;

namespace Tools
{
  public class PlayerTrigger : MonoBehaviour
  {
    public event Action<bool> EventStateChanged;
    private bool _isPlayerInside;

    public bool IsPlayerInside => _isPlayerInside;
    private void OnTriggerEnter(Collider other)
    {
      _isPlayerInside = true;
      EventStateChanged?.Invoke(_isPlayerInside);
    }

    private void OnTriggerExit(Collider other)
    {
      _isPlayerInside = false;
      EventStateChanged?.Invoke(_isPlayerInside);
    }
  }
}