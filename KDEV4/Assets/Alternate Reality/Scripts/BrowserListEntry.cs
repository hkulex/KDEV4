using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class BrowserListEntry : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _labelName;
        [SerializeField] private TMP_Text _labelPlayers;
        [SerializeField] private Button _buttonJoin;

        private RoomInfo _roomInfo;


        public void Initialize(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;

            _labelName.text = _roomInfo.Name.Length > 12 ? _roomInfo.Name.Substring(0, 12) + "..." : _roomInfo.Name;
            _labelPlayers.text = _roomInfo.PlayerCount + "/" + _roomInfo.MaxPlayers;
            _buttonJoin.onClick.AddListener(OnButtonJoinClicked);
        }


        private void OnButtonJoinClicked()
        {
            PhotonNetwork.JoinRoom(_roomInfo.Name);

            // show joining overlay popup 
        }


        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            // hide popup
            _buttonJoin.onClick.RemoveAllListeners();

            ViewManagement.Instance.SetView(Views.ROOM_VIEW);
        }


        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            // hide popup
            // show error popup
        }
    }
}