using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class CreateView : BaseView
    {
        [Header("Buttons:")]
        [SerializeField] private Button _buttonCreate;
        [SerializeField] private Button _buttonBack;
        [Header("InputFields:")]
        [SerializeField] private TMP_InputField _inputName;
        [Header("Dropdowns:")]
        [SerializeField] private TMP_Dropdown _dropdownPlayers;


        protected override void Start()
        {
            base.Start();

            _inputName.characterLimit = 12;
        }


        private void OnButtonCreateClicked()
        {
            _buttonCreate.interactable = false;

            Hashtable roomProperties = new Hashtable
            {
                { "team_index", "" }
            };

            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = byte.Parse(_dropdownPlayers.options[_dropdownPlayers.value].text),
                CustomRoomProperties = roomProperties
            };

            PhotonNetwork.CreateRoom(_inputName.text, roomOptions);

            // show creating room popup
        }


        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();

            // hide creating room popup
            ViewManagement.Instance.SetView(Views.ROOM_VIEW);
        }


        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);

            // show failed popup
        }


        private void OnDropdownValueChanged(int value)
        {
            
        }



        public override void SetVisible()
        {
            base.SetVisible();

            _buttonCreate.onClick.AddListener(OnButtonCreateClicked);
            _buttonBack.onClick.AddListener(() => ViewManagement.Instance.SetView(Views.BROWSER_VIEW));
            _dropdownPlayers.onValueChanged.AddListener(OnDropdownValueChanged);

            _buttonCreate.interactable = true;
        }


        public override void SetHidden()
        {
            base.SetHidden();

            _buttonCreate.onClick.RemoveAllListeners();
            _buttonBack.onClick.RemoveAllListeners();
            _dropdownPlayers.onValueChanged.RemoveAllListeners();

            _buttonCreate.interactable = false;
        }
    }
}