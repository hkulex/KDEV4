using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class MainView : BaseView
    {
        [SerializeField] private Button _buttonPlay;
        [SerializeField] private Button _buttonHighscores;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private Button _buttonQuit;


        private void OnButtonPlayClicked()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PopupManagement.Instance.Add("PopupName");

                PhotonNetwork.AutomaticallySyncScene = true;
            }
            else
            {
                ViewManagement.Instance.SetView(Views.BROWSER_VIEW);
            }
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            _buttonPlay.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            _buttonPlay.interactable = true;
        }


        public override void SetVisible()
        {
            base.SetVisible();

            _buttonPlay.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.IsConnected ? "Play" : "Connect";
            _buttonPlay.onClick.AddListener(OnButtonPlayClicked);
            _buttonHighscores.onClick.AddListener(() => ViewManagement.Instance.SetView(Views.HIGHSCORES_VIEW));
            _buttonSettings.onClick.AddListener(() => ViewManagement.Instance.SetView(Views.SETTINGS_VIEW));
            _buttonQuit.onClick.AddListener(Application.Quit);

            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }


        public override void SetHidden()
        {
            base.SetHidden();

            _buttonPlay.onClick.RemoveAllListeners();
            _buttonHighscores.onClick.RemoveAllListeners();
            _buttonSettings.onClick.RemoveAllListeners();
            _buttonQuit.onClick.RemoveAllListeners();
        }
    }
}