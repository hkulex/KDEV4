using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace alternatereality
{
    public class UIPlayerGameEntry : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _host;
        [SerializeField] private Image _avatar;
        [SerializeField] private Image _border;

        private int _id;


        public void Initialize(Player player)
        {
            _label.text = player.NickName;
            _host.gameObject.SetActive(player.IsMasterClient);
            _id = player.ActorNumber;
        }


        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            if (propertiesThatChanged.ContainsKey("turn_index"))
            {
                Player[] order = PhotonNetwork.CurrentRoom.CustomProperties["order"] as Player[];
                int index = (int)propertiesThatChanged["turn_index"];

                _border.gameObject.SetActive(order[index].ActorNumber == _id);
            }
        }
    }
}