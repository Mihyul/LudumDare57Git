using UnityEngine;

namespace ParallaxSystem
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [Space]
        [SerializeField] private float horizontalParallax;
        [SerializeField] private float verticalParallax;

        private Vector2 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            float distanceX = cameraTransform.position.x * (1.0f - horizontalParallax);
            float distanceY = cameraTransform.position.y * (1.0f - verticalParallax);
            transform.position = new Vector3(_startPosition.x + distanceX, _startPosition.y + distanceY, transform.position.z);
        }
    }
}