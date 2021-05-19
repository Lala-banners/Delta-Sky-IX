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
        
        // Start is called before the first frame update
        void Start()
        {
            gameOverPanel.SetActive(false);
            winPanel.SetActive(false);
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 1;
        }

        public void Retry(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
            gameOverPanel.SetActive(false);
            Time.timeScale = 0;
        }

        public void WinGame()
        {
            winPanel.SetActive(true);
        }

        public void QuitGame()
        {
            MainUIManager.instance.QuitGame();
        }
        
    }
}
