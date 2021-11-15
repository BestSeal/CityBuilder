using UnityEngine;
using UnityEngine.InputSystem;

namespace Sources
{
    public class CameraInputHandler : MonoBehaviour
    {
        private Mouse _mouse;
        [SerializeField] private float cameraMoveSpeed = 0.25f;
        
        private void Awake()
        {
            _mouse = Mouse.current;
        }

        private void Update()
        {
            if (_mouse.leftButton.isPressed && _mouse.delta.ReadValue().magnitude > 0)
            {
                var value = _mouse.delta.ReadValue();
                var currentPosition = transform.position;
                transform.position = new Vector3(currentPosition.x - value.x * cameraMoveSpeed * Time.deltaTime, currentPosition.y,
                    currentPosition.z - value.y * cameraMoveSpeed * Time.deltaTime);
            }
        }
    }
}
