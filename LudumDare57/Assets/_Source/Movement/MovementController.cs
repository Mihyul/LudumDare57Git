using UnityEngine;

namespace MovementSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private Animator animator;

        private Rigidbody2D _rigidbody;

        private Vector2 _targetVelocity;
        private float _targetAngle;

        private int _animatorSwimmingHash;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animatorSwimmingHash = Animator.StringToHash("IsSwimming");
        }

        private void FixedUpdate()
        {
            if (_targetVelocity == Vector2.zero)
            {
                if (_rigidbody.velocity != Vector2.zero)
                {
                    _rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, Vector2.zero, _deceleration * Time.fixedDeltaTime);
                }
            }
            else
            {
                _rigidbody.rotation = Mathf.MoveTowardsAngle(_rigidbody.rotation, _targetAngle, 200.0f * Time.fixedDeltaTime);

                if (_rigidbody.velocity != _targetVelocity)
                {
                    _rigidbody.velocity = Vector2.MoveTowards(_rigidbody.velocity, _targetVelocity, _acceleration * Time.fixedDeltaTime);
                }
            }
        }

        public void Move(Vector2 direction)
        {
            _targetVelocity = direction * _movementSpeed;
            _targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
            
            if (direction.x < -0.01f)
            {
                transform.localScale = new(-1.0f, 1.0f, 1.0f);
            }
            else if(direction.x > 0.01f)
            {
                transform.localScale = new(1.0f, 1.0f, 1.0f);
            }

            if (direction.magnitude > 0f)
            {
                animator.SetBool(_animatorSwimmingHash, true);
            }
            else
            {
                animator.SetBool(_animatorSwimmingHash, false);
            }
        }
    }
}
