using UnityEngine;

namespace Tools
{
    public class RotateToCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        
        private void Awake()
        {
            _cameraTransform = Camera.main != null ? Camera.main.transform : FindFirstObjectByType<Camera>().transform;
        }

        private void LateUpdate()
        {
            transform.forward = _cameraTransform.forward;
        }
    }
}
