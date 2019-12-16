using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


namespace alternatereality
{
    public class GameManagement : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static GameManagement Instance;

        [SerializeField] private Transform _list;
        [SerializeField] private TMP_Text _labelScore;


        private void Awake()
        {
            if (!Instance)
                Instance = this;

            IsPlaying = false;
        }


        public override void OnEnable()
        {
            base.OnEnable();

            PhotonNetwork.AddCallbackTarget(this);

            PhotonEvents.Send(PhotonEvents.ON_ENTERED_GAME, new object[] { PhotonNetwork.LocalPlayer }, ReceiverGroup.All);
        }


        public override void OnDisable()
        {
            base.OnDisable();

            PhotonNetwork.RemoveCallbackTarget(this);
        }


        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
        }


        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (properties.ContainsKey("complete"))
                if ((string)properties["complete"] == "true")
                    return;

            Player[] order = properties["order"] as Player[];
            int index = (int)properties["index"];

            List<Player> list = new List<Player>();

            foreach (Player player in order)
                if (player.UserId != otherPlayer.UserId)
                    list.Add(player);

            if (index >= list.Count)
                index = 0;

            properties["order"] = list.ToArray();
            properties["index"] = index;

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

            UpdateInterface();
        }



        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            PhotonNetwork.LoadLevel("SceneMain");
        }
        

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            if (propertiesThatChanged.ContainsKey("complete"))
            {
                PopupManagement.Instance.Add("PopupScore");
            }


            if (propertiesThatChanged.ContainsKey("score"))
            {
                _labelScore.text = "" + propertiesThatChanged["score"];
            }
        }


        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(target, changedProps);

            if (!PhotonNetwork.IsMasterClient)
                return;

            if (!changedProps.ContainsKey("connected"))
                return;

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.ContainsKey("connected"))
                    continue;

                if (!player.CustomProperties["connected"].Equals("game"))
                    return;
            }
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "turn_index", 0 } } );

            GameObject go = PhotonNetwork.InstantiateSceneObject("GameController", new Vector2(), Quaternion.identity);

            go.GetPhotonView().TransferOwnership(GetActivePlayer());
        }


        private Player GetActivePlayer()
        {
            Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;
            Player[] order = properties["order"] as Player[];
            int index = (int)properties["turn_index"];

            return order[index];
        }

        


        private void UpdateInterface()
        {
            foreach (Transform child in _list)
                Destroy(child.gameObject); // call destroy in the entry instead to remove listeners
            

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(Resources.Load<GameObject>("UIPlayerGameEntry"), _list);

                entry.GetComponent<UIPlayerGameEntry>().Initialize(player);
            }
        }
        


        public void OnEvent(EventData photonEvent)
        {
            object[] data = photonEvent.CustomData as object[];

            switch (photonEvent.Code)
            {
                case PhotonEvents.ON_ENTERED_GAME:
                    Player player = data[0] as Player;

                    if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                        player.SetCustomProperties(new Hashtable { { "connected", "game" } } );

                    UpdateInterface();
                    break;


                default:
                    
                    break;
            }
        }

        public bool IsPlaying { get; set; }
    }
}