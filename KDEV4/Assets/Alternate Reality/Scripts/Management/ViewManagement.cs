using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace alternatereality
{
    public class ViewManagement : MonoBehaviourPunCallbacks
    {
        public static ViewManagement Instance;

        private Dictionary<string, BaseView> _views;
        private BaseView _activeBaseView;


        private void Awake()
        {
            if (!Instance)
                Instance = this;

            _views = new Dictionary<string, BaseView>();

            foreach (RectTransform child in transform)
            {
                BaseView baseView = child.GetComponent<BaseView>();

                if (baseView)
                    _views.Add(child.name, child.GetComponent<BaseView>());

                child.gameObject.SetActive(false);
            }

            SetView(Views.MAIN_VIEW);
        }


        public void SetView(string to)
        {
            if (_activeBaseView)
                _activeBaseView.SetHidden();

            if (!_views.ContainsKey(to))
                return;

            _views[to].SetVisible();
            _activeBaseView = _views[to];
        }
    }
}