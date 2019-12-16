using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class GamePlayerListEntry : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Image _host;
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _labelName;

        private Player _player;


        public void Initialize(Player player)
        {
            _player = player;

            _labelName.text = _player.NickName;
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
        }
    }
}