using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace alternatereality
{
    public class ScorePopup : BasePopup
    {
        [SerializeField] private Button _buttonSubmit;
        [SerializeField] private Button _buttonQuit;
        [SerializeField] private TMP_Text _labelDescription;
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _sprites;

        private int _score;

        public override void Open()
        {
            base.Open();

            _buttonSubmit.onClick.AddListener(OnButtonSubmitClicked);
            _buttonQuit.onClick.AddListener(OnButtonQuitClicked);

            _score = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("score")
                ? (int) PhotonNetwork.CurrentRoom.CustomProperties["score"]
                : 0;

            _labelDescription.text = "Your team has done " + _score + " keepie ups! With " + PhotonNetwork.CurrentRoom.PlayerCount + " player(s) in the room that counts as a score of " + _score * PhotonNetwork.CurrentRoom.PlayerCount + "!";

            _score *= PhotonNetwork.CurrentRoom.PlayerCount;
        }


        private void OnButtonSubmitClicked()
        {
            StartCoroutine(Post());
        }


        private IEnumerator Post()
        {
            WWWForm form = new WWWForm();

            form.AddField("username", PhotonNetwork.LocalPlayer.NickName);
            form.AddField("score", _score);
            form.AddField("token", PhotonNetwork.LocalPlayer.CustomProperties["token"] as string);

            UnityWebRequest request = UnityWebRequest.Post(Data.SERVER_URL + "submit.php", form);

            yield return request.SendWebRequest();

            InformationPopup popup = PopupManagement.Instance.Add("PopupInformation", true) as InformationPopup;

            if (request.error != null)
            {
                popup.Initialize("Oops!", "Something went wrong...\r\nError: " + request.error);
            }
            else
            {
                if (request.downloadHandler.text != "-1")
                {
                    PopupManagement.Instance.Remove(this);
                    popup.Initialize("Success", "Your score was submitted!");

                    PhotonNetwork.Disconnect();
                }
                else
                {
                    popup.Initialize("Error", "Could not submit score.");
                    Debug.Log(request.downloadHandler.text);
                }
            }
        }


        private void OnButtonQuitClicked()
        {
            PhotonNetwork.Disconnect();
        }


        private void OnInputNameValueChanged(string value)
        {
            _image.sprite = value.Length >= 4 && value.Length <= 12 ? _sprites[0] : _sprites[1];
        }
    }
}