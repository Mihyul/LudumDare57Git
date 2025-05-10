using UnityEngine;

namespace CoverSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class Cover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out ICoverable coverable))
                coverable.TakeCover(this);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out ICoverable coverable))
                coverable.LeaveCover(this);
        }
    }
}
