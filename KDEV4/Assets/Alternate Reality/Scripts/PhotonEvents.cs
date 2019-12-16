using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace alternatereality
{
    public class PhotonEvents
    {
        public const byte ON_ENTERED_GAME = 1;


        public static void Send(byte eventCode, object[] content, ReceiverGroup receivers)
        {
            PhotonNetwork.RaiseEvent(eventCode, content, new RaiseEventOptions { Receivers = receivers }, new SendOptions { Reliability = true });
        }
    }
}