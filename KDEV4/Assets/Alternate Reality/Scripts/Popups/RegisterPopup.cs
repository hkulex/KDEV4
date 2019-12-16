using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace alternatereality
{
    public class RegisterPopup : BasePopup
    {
        [SerializeField] private Button _buttonConfirm;
        [SerializeField] private Button _buttonCancel;
        [SerializeField] private TMP_InputField _inputName;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private Image _image;
        [SerializeField] private List<Sprite> _sprites;


        private void Start()
        {
            _buttonConfirm.onClick.AddListener(OnButtonConfirmClicked);
            _buttonCancel.onClick.AddListener(OnButtonCancelClicked);
            _inputName.onValueChanged.AddListener(OnInputNameValueChanged);

            //_inputName.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
            _inputPassword.characterValidation = TMP_InputField.CharacterValidation.EmailAddress;
        }


        private void OnButtonConfirmClicked()
        {
            if (_inputName.text.Length < 4 || _inputName.text.Length > 12)
                return;

            StartCoroutine(Register());
        }


        private IEnumerator Register()
        {
            WWWForm form = new WWWForm();

            form.AddField("username", _inputName.text);
            form.AddField("password", Encryption.Md5Sum(_inputPassword.text));

            UnityWebRequest request = UnityWebRequest.Post(Data.SERVER_URL + "register.php", form);

            yield return request.SendWebRequest();

            InformationPopup popup = PopupManagement.Instance.Add("PopupInformation", true) as InformationPopup;

            if (request.error != null)
            {
                popup.Initialize("Oops!", "Something went wrong...\r\nError: " + request.error);
            }
            else
            {
                if (request.downloadHandler.text == "1")
                {
                    PopupManagement.Instance.Remove(this);
                    popup.Initialize("Success", "You registered your username!");
                }
                else
                {
                    popup.Initialize("Unavailable", "Username is already in use.");
                }
            }
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
            _buttonCancel.onClick.RemoveAllListeners();
            _inputName.onValueChanged.RemoveAllListeners();

            StopAllCoroutines();
        }
    }
}