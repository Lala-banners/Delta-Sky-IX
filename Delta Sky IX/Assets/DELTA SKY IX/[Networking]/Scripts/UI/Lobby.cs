using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DeltaSkyIX.Networking;

namespace DeltaSkyIX.UI
{
    public class Lobby : MonoBehaviour
    {
        private List<LobbyPlayerSlot> leftTeamSlots = new List<LobbyPlayerSlot>();
        private List<LobbyPlayerSlot> rightTeamSlots = new List<LobbyPlayerSlot>();

        [Header("Lobby")] 
        [SerializeField] private GameObject lobbyMenu;
        [SerializeField] private int requiredPlayerCount;
        [SerializeField] private GameObject leftTeamHolder;
        [SerializeField] private GameObject rightTeamHolder;
        [SerializeField] private GameObject tempPlane;
        
        [Header("Lobby Buttons")]
        [SerializeField] private Button readyUpButton;
        [SerializeField] private Button startGameButton;
        
        [Space]
        
        [Header("Player Match Settings")]
        [SerializeField] private GameObject matchSettingsMenu;
        [SerializeField] private Button goToMatchSettings;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button saveGameMode;
        [SerializeField] private Toggle pvpToggle;
        [SerializeField] private Toggle teamToggle;

        // Flipping bool that determines which column the connected player will be added to
        private bool assigningToLeft = true;

        private DeltaSkyIXPlayerNet localPlayer;

        public void AssignPlayerToSlot(DeltaSkyIXPlayerNet _player, bool _left, int _slotId)
        {
            Debug.Log("Assigning player to slot");
            // Get the correct slot list depending on the left param
            List<LobbyPlayerSlot> slots = _left ? leftTeamSlots : rightTeamSlots;
            // Assign the player to the relevant slot in this list
            slots[_slotId].AssignPlayer(_player);
        }

        public void OnPlayerConnected(DeltaSkyIXPlayerNet _player)
        {
            Debug.Log("Player connected");
            bool assigned = false;

            // If the player is the localplayer, assign it
            if(_player.isLocalPlayer && localPlayer == null) 
            {
                localPlayer = _player;
                localPlayer.onMatchStarted.AddListener(OnMatchStarted);
            }

            List<LobbyPlayerSlot> slots = assigningToLeft ? leftTeamSlots : rightTeamSlots;

            // Loop through each item in the list and run a lambda with the item at that index
            slots.ForEach(slot =>
            {
                // If we have assigned the value already, return from the lambda
                if (assigned)
                {
                    return;
                }
                else if (!slot.IsTaken)
                {
                    // If we haven't already assigned the player to a slot and this slot
                    // hasn't been taken, assign the player to this slot and flag 
                    // as slot been assigned
                    slot.AssignPlayer(_player);
                    slot.SetSide(assigningToLeft);
                    assigned = true;
                }
            });

            for(int i = 0; i < leftTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = leftTeamSlots[i];
                if(slot.IsTaken)
                    localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId); 
            }

            for (int i = 0; i < rightTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = rightTeamSlots[i];
                if (slot.IsTaken)
                    localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
            }

            // Flip the flag so that the next one will end up in the other list
            assigningToLeft = !assigningToLeft;
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Fill the two lists with their slots
            leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
            rightTeamSlots.AddRange(rightTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());

            readyUpButton.onClick.AddListener(() =>
            {
                DeltaSkyIXPlayerNet player = DeltaSkyIxNetworkManager.Instance.LocalPlayer; 
                player.SetReady(!player.ready);
            });

            startGameButton.onClick.AddListener(() => localPlayer.StartMatch());
            
            //Set Lobby menu to inactive when click on match settings
            goToMatchSettings.onClick.AddListener(() => 
            {
                lobbyMenu.SetActive(false);
                matchSettingsMenu.SetActive(true);
            });

            //Return button goes back to lobby menu
            returnButton.onClick.AddListener(() => 
            {
                lobbyMenu.SetActive(true);
                matchSettingsMenu.SetActive(false);
            });

            //Get the Toggle components
            pvpToggle.GetComponent<Toggle>();
            teamToggle.GetComponent<Toggle>();
            
            //Toggles between PvP and Teams
            pvpToggle.onValueChanged.AddListener(delegate {
                if (pvpToggle.isOn)
                {
                    GameModePvP();
                }
            });
            
            teamToggle.onValueChanged.AddListener(delegate {
                if (teamToggle.isOn)
                {
                    GameModeTeam();
                }
            });
            
            //Save button saves the choices
            saveGameMode.onClick.AddListener(() => 
            {
                //Save preferences
            });
        }
        
        public void GameModePvP() {
            Debug.Log("PvP Mode Active");
        }

        public void GameModeTeam() {
            Debug.Log("Team V Team Mode Active");
        }
        
        public void OnMatchStarted()
        {
            this.gameObject.SetActive(false);
            tempPlane.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            startGameButton.interactable = AllPlayersReady();
        }

        private bool AllPlayersReady()
        {
            int playerCount = 0;

            foreach(LobbyPlayerSlot slot in leftTeamSlots)
            {
                if(slot.Player == null)
                    continue;

                playerCount++;

                if(!slot.Player.ready)
                    return false;
            }

            foreach(LobbyPlayerSlot slot in rightTeamSlots)
            {
                if(slot.Player == null)
                    continue;

                playerCount++;

                if(!slot.Player.ready)
                    return false;
            }

            return playerCount >= requiredPlayerCount && DeltaSkyIxNetworkManager.Instance.IsHost;
        }
    }
}