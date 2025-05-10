using UnityEngine;

namespace GameSystem
{
    public abstract class AMenu : MonoBehaviour
    {
        public void OpenMenu()
        {
            OnMenuOpened();

            if (gameObject != null)
                gameObject.SetActive(true);
        }

        public void CloseMenu()
        {
            OnMenuclosed();

            if (gameObject != null)
                gameObject.SetActive(false);
        }

        protected abstract void OnMenuOpened();

        protected abstract void OnMenuclosed();
    }
}
