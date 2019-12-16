using Photon.Pun;
using TMPro;

namespace alternatereality
{
    public class BaseView : MonoBehaviourPunCallbacks
    {

        protected virtual void Awake()
        {

        }


        protected virtual void Start()
        {

        }


        public virtual void SetVisible()
        {
            gameObject.SetActive(true);
        }


        public virtual void SetHidden()
        {
            gameObject.SetActive(false);
        }
    }
}