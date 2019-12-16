using Photon.Pun;
using TMPro;
using UnityEngine;

namespace alternatereality
{
    public class SplashView : BaseView
    {
        [SerializeField] private TMP_Text _labelIntro;

        private bool _isConnecting;


        protected override void Start()
        {
            base.Start();

            _isConnecting = false;

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "0.0.1";
        }


        private void Update()
        {
            if (_isConnecting)
                return;

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                _isConnecting = true;
                _labelIntro.text = "Connecting...";

                PhotonNetwork.ConnectUsingSettings();
            }
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            _labelIntro.text = "Connected!";
            //change scene
        }
    }
}