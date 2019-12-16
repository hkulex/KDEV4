namespace alternatereality
{
    public class ConnectionPopup : BasePopup
    {
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            PopupManagement.Instance.Remove(this);
        }
    }
}