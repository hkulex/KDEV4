using ExitGames.Client.Photon;
using UnityEngine;

namespace alternatereality
{
    public class LoadingPopup : BasePopup
    {
        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);

            if (propertiesThatChanged.ContainsKey("thumbnail_image"))
                PopupManagement.Instance.Remove(this);
        }
    }
}