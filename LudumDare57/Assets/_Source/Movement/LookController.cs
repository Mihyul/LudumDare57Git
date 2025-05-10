using UnityEngine;

namespace MovementSystem
{
    public class LookController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform lookTransform;

        private Vector2 _lastMouseScreenPosition;

        private void FixedUpdate()
        {
            Rotate();
        }

        public void LookAt(Vector2 mouseScreenPosition)
        {
            _lastMouseScreenPosition = mouseScreenPosition;
            Rotate();
        }

        private void Rotate()
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(_lastMouseScreenPosition);
            mouseWorldPosition.z = 0f;
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            lookTransform.eulerAngles = new(0f, 0f, angle);
        }
    }
}
