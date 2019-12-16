using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class BrowserView : BaseView
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonCreate;
        [SerializeField] private TMP_InputField _inputSearch;

        private Dictionary<string, RoomInfo> _roomList;


        protected override void Start()
        {
            base.Start();

            _roomList = new Dictionary<string, RoomInfo>();
        }

        public override void SetVisible()
        {
            base.SetVisible();

            if (!PhotonNetwork.InLobby)
                PhotonNetwork.JoinLobby();

            _buttonBack.onClick.AddListener(() => ViewManagement.Instance.SetView(Views.MAIN_VIEW));
            _buttonCreate.onClick.AddListener(() => ViewManagement.Instance.SetView(Views.CREATE_VIEW));
            _inputSearch.onValueChanged.AddListener(OnInputSearchValueChanged);
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            foreach (RoomInfo roomInfo in roomList)
            {
                if (!roomInfo.IsOpen || !roomInfo.IsVisible || roomInfo.RemovedFromList)
                {
                    if (_roomList.ContainsKey(roomInfo.Name))
                        _roomList.Remove(roomInfo.Name);

                    continue;
                }

                if (_roomList.ContainsKey(roomInfo.Name))
                    _roomList[roomInfo.Name] = roomInfo;
                else
                    _roomList.Add(roomInfo.Name, roomInfo);
            }


            foreach (Transform child in _content)
                Destroy(child.gameObject);


            foreach (KeyValuePair<string, RoomInfo> roomInfo in _roomList)
            {
                GameObject entry = Instantiate(Resources.Load<GameObject>("UIBrowserListEntry"), _content);

                entry.GetComponent<BrowserListEntry>().Initialize(roomInfo.Value);
            }
        }


        private void OnInputSearchValueChanged(string value)
        {
            
        }


        public override void SetHidden()
        {
            base.SetHidden();

            _buttonBack.onClick.RemoveAllListeners();
            _buttonCreate.onClick.RemoveAllListeners();
        }
    }
}