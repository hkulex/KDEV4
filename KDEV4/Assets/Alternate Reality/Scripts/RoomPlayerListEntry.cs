using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class RoomPlayerListEntry : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _labelName;
        [SerializeField] private Button _buttonKick;

        private Player _player;


        public void Initialize(Player player)
        {
            _player = player;

            _labelName.text = _player.NickName;

            if (PhotonNetwork.IsMasterClient)
                _buttonKick.onClick.AddListener(OnButtonKickClicked);

            _buttonKick.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            _image.gameObject.SetActive(_player.IsMasterClient);

            if (player.IsLocal)
                _buttonKick.gameObject.SetActive(false);
        }


        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);

            if (newMasterClient.IsLocal)
                _buttonKick.onClick.AddListener(OnButtonKickClicked);
            else
                _buttonKick.onClick.RemoveAllListeners();

            _buttonKick.gameObject.SetActive(newMasterClient.IsLocal);
            _image.gameObject.SetActive(_player.IsMasterClient);
        }


        private void OnButtonKickClicked()
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CloseConnection(_player);
        }
    }
}