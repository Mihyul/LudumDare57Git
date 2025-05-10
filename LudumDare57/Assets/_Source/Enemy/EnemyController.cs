using PerceptionSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EnemySystem
{
    public class EnemyController : MonoBehaviour, IPerciever
    {
        public List<Transform> route = new();
        private int _currentRouteIndex = -1;

        [SerializeField] private EnemyMovement enemyMovement;

        // Perception
        private readonly HashSet<APercievedObject> _percievedObjects = new();
        private APercievedObject _currentPercievedObject;

        // Weapon
        [Space]
        [SerializeField] private MeleeWeapon.WeaponReferences weaponReferences;
        [SerializeField] private MeleeWeapon.WeaponParameters weaponParameters;

        [Space]
        [SerializeField] private AudioSource effectsSource;
        [SerializeField] private AudioClip alertSound;

        [Space]
        [SerializeField] private List<Light2D> lights = new();
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color alertColor;

        private MeleeWeapon _weapon;

        private void Start()
        {
            enemyMovement.OnEndOfPathReached += SelectNextTarget;

            _weapon = new(weaponReferences, weaponParameters);

            SelectNextTarget();
        }

        private void FixedUpdate()
        {
            if (_currentPercievedObject != null && !_percievedObjects.Contains(_currentPercievedObject))
            {
                _currentPercievedObject = null;
            }

            if (_currentPercievedObject == null && _percievedObjects.Count > 0)
            {
                _currentPercievedObject = _percievedObjects.ToArray()[0];
            }

            if (_currentPercievedObject != null)
            {
                enemyMovement.MoveToPosition(_currentPercievedObject.transform.position);
            }

            _weapon.Use();
        }

        public void StartPercieving(APercievedObject percievedObject)
        {
            if (_percievedObjects.Add(percievedObject))
            {
                PlayAlertEffects();
            }
        }

        public void StopPercieving(APercievedObject percievedObject)
        {
            _percievedObjects.Remove(percievedObject);
        }

        private void PlayAlertEffects()
        {
            effectsSource.PlayOneShot(alertSound);

            foreach (var light in lights)
            {
                light.color = alertColor;
            }
        }

        private void CancelAlertEffects()
        {
            foreach (var light in lights)
            {
                light.color = defaultColor;
            }
        }

        private void SelectNextTarget()
        {
            if (_currentPercievedObject != null)
                return;

            CancelAlertEffects();
            _currentRouteIndex++;

            if (_currentRouteIndex >= route.Count)
            {
                _currentRouteIndex = 0;
            }

            enemyMovement.MoveToPosition(route[_currentRouteIndex].position);
        }
    }
}
