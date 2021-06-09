using DeltaSkyIX.Player;
using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DeltaSkyIX.Networking
{
    public class DeltaSkyIXPlayerSelection : NetworkBehaviour
    {
        [Tooltip("The UI display"), SerializeField] private GameObject characterSelectDisplay;
        [Tooltip("What the character selected will look like"), SerializeField] private Transform characterPreviewParent;
        [Tooltip("Character's name"), SerializeField] private TMP_Text nameText;
        [Tooltip("How fast the model will rotate"), SerializeField] private float turnSpeed = 90f;
        [Tooltip("Amount of characters"), SerializeField] private Character[] characters;

        private int currentCharacterIndex = 0;
        private List<GameObject> characterInstances = new List<GameObject>();
        public GameObject lobbyMenu;

        public override void OnStartClient() {
            base.OnStartClient();
            lobbyMenu.SetActive(false);

            foreach (Character character in characters)
            {
                GameObject characterInstance = Instantiate(character.CharacterPreviewPrefab, characterPreviewParent);
                characterInstance.SetActive(false);
                characterInstances.Add(characterInstance);
            }
            
            characterInstances[currentCharacterIndex].SetActive(true);
            nameText.text = characters[currentCharacterIndex].CharacterName;
            characterSelectDisplay.SetActive(true);
        }

        /// <summary>
        /// This method will manage deactivating the UI and calling the Command Spawn Player function
        /// </summary>
        public void SelectCharacter() {
            CmdSelect(currentCharacterIndex);
            characterSelectDisplay.SetActive(false);
        }

        /// <summary>
        /// This will spawn the chosen character and spawn into the game.
        /// </summary>
        [Command]
        public void CmdSelect(int characterIndex, NetworkConnectionToClient sender = null) {
            GameObject selectedCharacter = Instantiate(characters[characterIndex].GameplayCharacterPrefab);
            NetworkServer.Spawn(selectedCharacter, sender);
        }

        public void Right() {
            characterInstances[currentCharacterIndex].SetActive(false);
            
            currentCharacterIndex = (currentCharacterIndex + 1) % characterInstances.Count;
            
            characterInstances[currentCharacterIndex].SetActive(true);
            nameText.text = characters[currentCharacterIndex].CharacterName;
        }
        
        public void Left() {
            characterInstances[currentCharacterIndex].SetActive(false);

            currentCharacterIndex--;
            if (currentCharacterIndex < 0)
                currentCharacterIndex += characterInstances.Count;
            
            characterInstances[currentCharacterIndex].SetActive(true);
            nameText.text = characters[currentCharacterIndex].CharacterName;
        }
    }
}