using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace alternatereality
{
    public class NamePopup : BasePopup
    {
        [SerializeField] private Button _buttonConfirm;
        [SerializeField] private Button _buttonRegister;
        [SerializeField] private Button _buttonCancel;
        [SerializeField] private TMP_InputField _inputName;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _sprites;


        private void Start()
        {
            _buttonConfirm.onClick.AddListener(OnButtonConfirmClicked);
            _buttonRegister.onClick.AddListener(OnButtonRegisterClicked);
            _buttonCancel.onClick.AddListener(OnButtonCancelClicked);
            _inputName.onValueChanged.AddListener(OnInputNameValueChanged);

            //_inputName.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            _inputPassword.characterValidation = TMP_InputField.CharacterValidation.EmailAddress;
        }


        private void OnButtonConfirmClicked()
        {
            if (_inputName.text.Length < 4 || _inputName.text.Length > 12)
                return;

            StartCoroutine(Login());
        }


        private IEnumerator Login()
        {
            WWWForm form = new WWWForm();

            form.AddField("username", _inputName.text);
            form.AddField("password", Encryption.Md5Sum(_inputPassword.text));

            UnityWebRequest request = UnityWebRequest.Post(Data.SERVER_URL + "login.php", form);

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
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "token", request.downloadHandler.text } });

                    PopupManagement.Instance.Remove(this);
                    PopupManagement.Instance.Add("PopupConnection", true);

                    PhotonNetwork.LocalPlayer.NickName = _inputName.text;
                    PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    InformationPopup popup = PopupManagement.Instance.Add("PopupInformation", true) as InformationPopup;

                    popup.Initialize("Invalid credentials", "Username and/or password is incorrect.");
                }
            }
        }


        private void OnButtonRegisterClicked()
        {
            PopupManagement.Instance.Add("PopupRegister", true);
        }


        private void OnButtonCancelClicked()
        {
            PopupManagement.Instance.Remove(this);
        }


        private void OnInputNameValueChanged(string value)
        {
            _image.sprite = value.Length >= 4 && value.Length <= 12 ? _sprites[0] : _sprites[1];
        }


        public override void Dispose()
        {
            base.Dispose();

            _buttonConfirm.onClick.RemoveAllListeners();
            _buttonRegister.onClick.RemoveAllListeners();
            _buttonCancel.onClick.RemoveAllListeners();
            _inputName.onValueChanged.RemoveAllListeners();
        }
    }
}