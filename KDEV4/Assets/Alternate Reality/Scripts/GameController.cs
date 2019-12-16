using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace alternatereality
{
	public class GameController : MonoBehaviourPun
    {
        private Rigidbody2D _rigidbody2D;
        private int _force;
        private bool _active;

		private void Awake()
        {
            /*PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 60;*/

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;

            _force = 350;
            _active = true;
        }


        private void Update()
        {
            if (!_active)
                return;

            if (transform.position.y < -6 || transform.position.x > 10 || transform.position.x < -10)
            {
                _active = false;

                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "complete", "true" } } );

                return;
            }


            if (!photonView.IsMine)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 point = new Vector2(world.x, world.y);

                if (Mathf.Sqrt((point.x - transform.position.x) * (point.x - transform.position.x) +
                               (point.y - transform.position.y) * (point.y - transform.position.y)) > 1f)
                    return;

                Vector2 distance = new Vector2(transform.position.x - point.x, transform.position.y - point.y);

                if (distance.y < 0.1f)
                    distance.y = 0.1f;

                photonView.RPC("RpcTest", RpcTarget.All, distance);

                ChangeTurn();
                UpdateScore();
            }
        }


        private void ChangeTurn()
        {
            Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (properties.ContainsKey("complete"))
                if ((string)properties["complete"] == "true")
                    return;

            Player[] order = properties["order"] as Player[];
            int index = (int)properties["turn_index"];

            index++;

            if (index >= order.Length)
                index = 0;

            properties["turn_index"] = index;

            photonView.TransferOwnership(order[index]);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }


        private void UpdateScore()
        {
            Hashtable properties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (!properties.ContainsKey("score"))
            {
                properties["score"] = 1;
                return;
            }

            properties["score"] = (int) properties["score"] + 1;

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }


        [PunRPC]
        public void RpcTest(Vector2 force)
        {
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = new Vector2();
            _rigidbody2D.angularVelocity = 0f;
            _rigidbody2D.AddForceAtPosition(force * _force, _rigidbody2D.position);
            _rigidbody2D.AddTorque(force.x * -150f);
        }
    }
}