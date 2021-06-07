using DeltaSky.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeltaSky.Controllers.UI
{
    public class InGameUI : MonoBehaviour
    {
        #region Instance
        public static InGameUI instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        #endregion
        
        [Header("Game Over")] 
        public GameObject gameOverPanel;

        [Header("Win")]
        public GameObject winPanel;

        [Header("HUD")]
        public GameObject miniMapUI;
        public GameObject playerHealth;
        
        // Start is called before the first frame update
        void Start()
        {
            playerHealth.SetActive(true);
            gameOverPanel.SetActive(false);
            winPanel.SetActive(false);
        }

        public void GameOver() {
            Cursor.visible = true;
            playerHealth.SetActive(false);
            gameOverPanel.SetActive(true);
            miniMapUI.SetActive(false);
            Time.timeScale = 0;
        }

        public void Retry(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
            gameOverPanel.SetActive(false);
            Time.timeScale = 1;
        }

        public void WinGame()
        {
            Cursor.visible = true;
            playerHealth.SetActive(false);
            winPanel.SetActive(true);
            miniMapUI.SetActive(false);
            Time.timeScale = 0;
        }

        public void QuitGame()
        {
            MainUIManager.instance.QuitGame();
        }

        public void StartGame() {
            Cursor.visible = false;
            miniMapUI.SetActive(true);
            playerHealth.SetActive(true);
        }
        
    }
}
