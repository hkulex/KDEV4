using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class RoomView : BaseView
    {
        [SerializeField] private TMP_Text _labelName;
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Transform _content;

        private List<Transform> _entries;


        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            _labelName.text = PhotonNetwork.CurrentRoom.Name;
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            newPlayer.SetCustomProperties(new Hashtable { { "can_play", "false" } } );

            UpdateInterface();
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            UpdateInterface();
        }


        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            ViewManagement.Instance.SetView(Views.MAIN_VIEW);
        }


        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(target, changedProps);

            if (changedProps.ContainsKey("connected"))
                if ((string) changedProps["connected"] == "room")
                    UpdateInterface();
        }


        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            // update game room properties
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            Debug.Log("disconnected with reason: " + cause);
        }


        private void UpdateInterface()
        {
            foreach (Transform entry in _entries)
                Destroy(entry.gameObject); // call destroy in the entry instead to remove listeners

            _entries.Clear();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(Resources.Load<GameObject>("UIPlayerListEntry"), _content);

                entry.GetComponent<RoomPlayerListEntry>().Initialize(player);

                _entries.Add(entry.transform);
            }
        }


        private void OnButtonStartClicked()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            //check if all players are ready

            Hashtable properties = new Hashtable
            {
                { "order", PhotonNetwork.PlayerList },
                { "turn_index", 0 }
            };

            ViewManagement.Instance.SetView("");

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("SceneGame");
        }



        public override void SetVisible()
        {
            base.SetVisible();

            _entries = new List<Transform>();

            _buttonBack.onClick.AddListener(OnButtonBackClicked);
            _buttonStart.onClick.AddListener(OnButtonStartClicked);

            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "connected", "room" } } );
        }


        public override void SetHidden()
        {
            base.SetHidden();

            foreach (Transform entry in _entries)
                Destroy(entry.gameObject); // call destroy in the entry instead to remove listeners

            _entries.Clear();
            _entries = null;

            _buttonBack.onClick.RemoveAllListeners();
            _buttonStart.onClick.RemoveAllListeners();
        }


        private void OnButtonBackClicked()
        {
            PhotonNetwork.LeaveRoom();
            ViewManagement.Instance.SetView(Views.MAIN_VIEW);
        }
    }
}