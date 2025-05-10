using UnityEngine;

namespace MovingDecorationsSystem
{
    public class MovingDecoration : MonoBehaviour
    {
        [SerializeField] private WaypointsProvider waypointsProvider;
        [Space]
        [SerializeField] private float movementSpeed;
        [SerializeField] private bool moveRandomly;
        [Space]
        [SerializeField] private float rotationSpeed;
        [SerializeField] private bool rotate;

        private Vector3 _currentWaypoint;
        private int _currentWaypointIndex;

        private void Start()
        {
            SelectNextWaypoint();
        }

        private void FixedUpdate()
        {
            if (_currentWaypoint == null)
                return;

            Vector3 targetPosition = _currentWaypoint;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.fixedDeltaTime);

            Vector3 targetDirection = (targetPosition - transform.position).normalized;

            if (targetDirection.x <= -0.01f)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            else if (targetDirection.x >= 0.01f)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            if (rotate)
            {
                float targetRotation = Mathf.Atan2(targetDirection.x, targetDirection.y) * Mathf.Rad2Deg - 90.0f;
                float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                transform.eulerAngles = new Vector3(0, 0, rotation);
            }

            if (transform.position == targetPosition)
                SelectNextWaypoint();
        }

        private void SelectNextWaypoint()
        {
            if (waypointsProvider.Waypoints.Count <= 0)
                return;

            if (moveRandomly)
            {
                _currentWaypointIndex = Random.Range(0, waypointsProvider.Waypoints.Count);
            }
            else
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypointsProvider.Waypoints.Count)
                    _currentWaypointIndex = 0;
            }

            _currentWaypoint = waypointsProvider.Waypoints[_currentWaypointIndex];
        }
    }
}
