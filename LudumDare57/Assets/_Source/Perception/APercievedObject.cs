using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PerceptionSystem
{
    public abstract class APercievedObject : MonoBehaviour
    {
        [SerializeField] private LayerMask percievingLayers;

        protected abstract float VisibilityDistance { get; }

        private readonly HashSet<IPerciever> _currentPercievers = new();

        private void FixedUpdate()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, VisibilityDistance, percievingLayers);
            HashSet<IPerciever> percievers = new();

            foreach (Collider2D collider in colliders)
                if (collider.TryGetComponent(out IPerciever perciever))
                {
                    percievers.Add(perciever);

                    if (!_currentPercievers.Contains(perciever))
                        perciever.StartPercieving(this);
                }

            foreach (IPerciever perciever in _currentPercievers.ToArray())
                if (!percievers.Contains(perciever))
                {
                    perciever.StopPercieving(this);
                    _currentPercievers.Remove(perciever);
                }

            _currentPercievers.UnionWith(percievers);
        }
    }
}
