using Photon.Pun;
using UnityEngine;

namespace alternatereality
{
    public class BasePopup : MonoBehaviourPunCallbacks
    {
        protected bool active;

        public virtual void Initialize()
        {

        }


        public virtual void Open()
        {
            active = true;

            gameObject.SetActive(true);
        }


        public virtual void Hide()
        {
            active = false;

            gameObject.SetActive(false);
        }


        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}