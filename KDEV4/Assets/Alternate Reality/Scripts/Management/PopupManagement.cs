using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class PopupManagement : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Image _overlay;

        public static PopupManagement Instance;

        private Queue<BasePopup> _queue;
        private List<BasePopup> _popups;


        private void Awake()
        {
            if (!Instance)
                Instance = this;

            _queue = new Queue< BasePopup>();
            _popups = new List< BasePopup>();

            _overlay.gameObject.SetActive(false);
        }


        public BasePopup Add(string name, bool force = false)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>(name), transform); go.SetActive(false);
            BasePopup popup = go.GetComponent<BasePopup>();

            if (_popups.Count > 0 && !force)
                _queue.Enqueue(popup);
            else
                Open(popup);
            
            return popup;
        }


        public void Remove(BasePopup popup)
        {
            if (!_popups.Contains(popup))
                return;

            Close(popup);

            UpdateQueue();
        }


        private void Open(BasePopup popup)
        {
            _overlay.gameObject.SetActive(true);

            _popups.Add(popup);

            popup.Open();
        }


        private void Close(BasePopup popup)
        {
            _popups.Remove(popup);

            popup.Hide();
            popup.Dispose();
        }


        private void UpdateQueue()
        {
            if (_queue.Count == 0 && _popups.Count == 0)
            {
                _overlay.gameObject.SetActive(false);
                return;
            }
            
            if (_queue.Count > 0)
                Open(_queue.Dequeue());
        }


        private void Dispose()
        {
            //clear everything
        }
    }
}