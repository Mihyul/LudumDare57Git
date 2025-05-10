using System;
using UnityEngine;

namespace EnemySystem
{
    public class MeleeWeapon
    {
        private readonly WeaponReferences _references;
        private readonly WeaponParameters _parameters;

        public MeleeWeapon(WeaponReferences references, WeaponParameters parameters)
        {
            _references = references ?? throw new ArgumentNullException(nameof(references));
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public float AttackDistance { get { return _references.WeaponOrigin.localPosition.magnitude + _parameters.AttackRadius; } }

        public void Use()
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_references.WeaponOrigin.position, _parameters.AttackRadius, _references.TargetLayers);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.TryGetComponent(out ITarget target))
                {
                    target.DealDamage(_parameters.AttackDamage);
                    _references.AttackingSource.PlayOneShot(_references.AttackingClip);
                }
            }
        }

        [Serializable]
        public class WeaponParameters
        {
            [field: SerializeField] public float AttackRadius { get; private set; }
            [field: SerializeField] public int AttackDamage { get; private set; }
        }

        [Serializable]
        public class WeaponReferences
        {
            [field: SerializeField] public Transform WeaponOrigin { get; private set; }
            [field: SerializeField] public AudioSource AttackingSource { get; private set; }
            [field: SerializeField] public AudioClip AttackingClip { get; private set; }
            [field: SerializeField] public LayerMask TargetLayers { get; private set; }
        }
    }
}
