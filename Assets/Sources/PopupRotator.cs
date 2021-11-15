using UnityEngine;

namespace Sources
{
    public class PopupRotator : MonoBehaviour
    {
        private void Update()
        {
            if (gameObject.activeSelf)
            {
                if (Camera.main != null)
                {
                    transform.LookAt(Camera.main.transform);
                }
            }
        }
    }
}