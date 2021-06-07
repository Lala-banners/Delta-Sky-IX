using DeltaSky.Controllers.UI;
using DeltaSkyIX.Player;
using DeltaSkyIX.UI;
using Mirror;
using Mirror.Experimental;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DeltaSkyIX.Networking
{
    public class DeltaSkyIXPlayerNet : NetworkBehaviour
    {
        [SyncVar] public byte playerId;
        [SyncVar] public string username = "";
        [SyncVar] public bool ready = false;

        [SerializeField] private Camera camera;
        [SerializeField] private GameObject[] matchObjects;
        [SerializeField] private FirstPersonMovement movement;
        public UnityEvent onMatchStarted = new UnityEvent();

        private Lobby lobby;
        private bool hasJoinedLobby = false;

        public void StartMatch()
        {
            if(isLocalPlayer)
                CmdStartMatch();
        }

        public void SetUsername(string _name)
        {
            if(isLocalPlayer)
            {
                // Only localplayers can call Commands as localplayers are the only
                // ones who have the authority to talk to the server
                CmdSetUsername(_name);
            }
        }

        public void SetReady(bool _ready)
        {
            if(isLocalPlayer)
            {
                // Only localplayers can call Commands as localplayers are the only
                // ones who have the authority to talk to the server
                CmdSetReady(_ready);
            }
        }

        public void AssignPlayerToSlot(bool _left, int _slotId, byte _playerId)
        {
            Debug.Log("Assign player to slot");
            if(isLocalPlayer)
                CmdAssignPlayerToLobbySlot(_left, _slotId, _playerId);
            else Debug.Log("Not local player");
        }

        #region Commands

        [Command]
        public void CmdSetUsername(string _name) => username = _name;

        [Command]
        public void CmdSetReady(bool _ready) => ready = _ready;

        [Command]
        public void CmdAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId) =>
            RpcAssignPlayerToLobbySlot(_left, _slotId, _playerId);

        [Command]
        public void CmdStartMatch() => RpcStartMatch();

        #endregion

        #region RPCs

        [ClientRpc]
        public void RpcAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId)
        {
            Debug.Log("Rpc assign player");
            // If this is running on the host client, we don't need to set the player
            // to the slot, so just ignore this call
            if(DeltaSkyIxNetworkManager.Instance.IsHost)
                return;
            
            Debug.Log("Rpc assign player 2");

            // Find the Lobby in the scene and set the player to the correct slot
            StartCoroutine(AssignPlayerToLobbySlotDelayed(DeltaSkyIxNetworkManager.Instance.GetPlayerForId(_playerId),
                _left, _slotId));
        }

        [ClientRpc]
        public void RpcStartMatch()
        {
            //Activates all players in the match
            foreach (DeltaSkyIXPlayerNet p in DeltaSkyIxNetworkManager.Instance.Players)
            {
                foreach(GameObject matchObject in p.matchObjects)
                    matchObject.SetActive(true);
            }

            LevelManager.LoadLevel("Gameplay");
            
            DeltaSkyIXPlayerNet player = DeltaSkyIxNetworkManager.Instance.LocalPlayer;
            FindObjectOfType<Lobby>().OnMatchStarted();
            player.movement.Enable();
            player.GetComponentInChildren<Camera>().enabled = true;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<NetworkRigidbody>().enabled = true;
        }

        #endregion

        #region Coroutines

        private IEnumerator AssignPlayerToLobbySlotDelayed(DeltaSkyIXPlayerNet _player, bool _left, int _slotId)
        {
            // Keep trying to get the lobby until it's not null
            Lobby lobby = FindObjectOfType<Lobby>();
            Debug.Log("Looking for lobby");
            while(lobby == null)
            {
                yield return null;

                lobby = FindObjectOfType<Lobby>();
            }
            Debug.Log("Found lobby");

            // Lobby successfully got, so assign the player
            lobby.AssignPlayerToSlot(_player, _left, _slotId);
        }

        #endregion

        private void Awake()
        {
            foreach(GameObject matchObject in matchObjects)
                matchObject.SetActive(false);
        }

        // Start is called before the first frame update
        private void Start()
        {
            SetUsername(DeltaSkyIxNetworkManager.Instance.PlayerName);
            SetUsername(DeltaSkyIxNetworkManager.Instance.GameName);
        }

        // Update is called once per frame
        private void Update()
        {
            Debug.Log("Update");
            // Determine if we are on the host client
            if(DeltaSkyIxNetworkManager.Instance.IsHost)
            {
                Debug.Log("Is host");
                // Attempt to get the lobby if we haven't already joined a lobby
                if(lobby == null && !hasJoinedLobby)
                    lobby = FindObjectOfType<Lobby>();

                // Attempt to join the lobby if we haven't already and the lobby is set
                if(lobby != null && !hasJoinedLobby)
                {
                    hasJoinedLobby = true;
                    lobby.OnPlayerConnected(this); 
                }
            }
        }

        public override void OnStartClient()
        {
            DeltaSkyIxNetworkManager.Instance.AddPlayer(this);
        }

        // Runs only when the object is connected is the local player
        public override void OnStartLocalPlayer()
        {
            
        }

        // Runs when the client is disconnected from the server
        public override void OnStopClient()
        {
            // Remove the playerID from the server
            DeltaSkyIxNetworkManager.Instance.RemovePlayer(playerId);
        }
    }
}