using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace alternatereality
{
    public class SettingsView : BaseView
    {
        [SerializeField] private Button _buttonBack;
        [SerializeField] private TMP_Text _labelUsername;
        [SerializeField] private TMP_Text _labelScore;


        public override void SetVisible()
        {
            base.SetVisible();

            _buttonBack.onClick.AddListener(() => ViewManagement.Instance.SetView("MainView"));

            if (!PhotonNetwork.IsConnected)
            {
                _labelUsername.text = "Not logged in.";
                _labelScore.text = "-";

                return;
            }

            StartCoroutine(GetData());
        }


        private IEnumerator GetData()
        {
            WWWForm form = new WWWForm();

            form.AddField("username", PhotonNetwork.LocalPlayer.NickName);

            UnityWebRequest request = UnityWebRequest.Post(Data.SERVER_URL + "get_account.php", form);

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                InformationPopup popup = PopupManagement.Instance.Add("PopupInformation", true) as InformationPopup;

                popup.Initialize("Oops!", "Something went wrong...\r\nError: " + request.error);
            }
            else
            {
                if (request.downloadHandler.text != "-1")
                {
                    _labelUsername.text = PhotonNetwork.LocalPlayer.NickName;
                    _labelScore.text = request.downloadHandler.text;
                }
            }
        }


        public override void SetHidden()
        {
            base.SetHidden();

            _buttonBack.onClick.RemoveAllListeners();
            StopAllCoroutines();
        }
    }
}