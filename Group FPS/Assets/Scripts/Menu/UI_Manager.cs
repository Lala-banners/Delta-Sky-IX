using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UI_Manager : MonoBehaviour
{
    #region UI Elements
    [Header("Settings")]
    public Toggle fullscreenToggle;
    public Resolution[] resolutions;
    public TMP_Dropdown resolution;

    [Header("Audio")]
    public Toggle muteToggle;
    public AudioMixer masterAudio;
    public Slider musicSlider;
    public Slider SFXSlider;

    [Header("Menu Panels")]
    public GameObject optionsMenu;
    public GameObject mainMenu;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        SetUpResolution();
        LoadPlayerPrefs();

        #region Fullscreen Prefs
        if (!PlayerPrefs.HasKey("fullscreen"))
        {
            PlayerPrefs.SetInt("fullscreen", 0); //PlayerPrefs cant save bools, so give int (0) false, (1) true
            Screen.fullScreen = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
        #endregion
    }

    public void StartGame(int sceneIndex)
    {
        //Change scene to lobby/gamemode selection
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }

    #region Change Settings
    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    //This changes the quality 
    public void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    //This changes volume in options
    public void SetMusicVolume(float MusicVol)
    {
        masterAudio.SetFloat("MusicVol", MusicVol);
    }
    //This changes sound effects volume 
    public void SetSFXVolume(float SFXVol)
    {
        masterAudio.SetFloat("SFXVol", SFXVol);
    }

    //Function to mute volume when toggle is active
    public void ToggleMute(bool isMuted)
    {
        //string reference isMuted connects to the AudioMixer master group Volume and isMuted parameters in Unity
        if (isMuted)
        {
            //-40 is the minimum volume
            masterAudio.SetFloat("isMutedVolume", -40);
        }
        else
        {
            //20 is the maximum volume
            masterAudio.SetFloat("isMutedVolume", 0);
        }
    }

    public void SetUpResolution()
    {
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) //Go through every resolution
        {
            //Build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                //We have found the current screen resolution, save that number.
                currentResolutionIndex = i;
            }
        }
        //Set up our dropdown
        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
    }

    public void SetResolution(int resolutionindex)
    {
        Resolution res = resolutions[resolutionindex];
        Screen.SetResolution(res.width, res.height, false);
    }
    #endregion

    #region Save Prefs
    public void SavePlayerPrefs()
    {
        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        //save audio sliders
        float musicVol;
        if (masterAudio.GetFloat("MusicVol", out musicVol))
        {
            PlayerPrefs.SetFloat("MusicVol", musicVol);
        }
        float SFXVol;
        if (masterAudio.GetFloat("SFXVol", out SFXVol))
        {
            PlayerPrefs.SetFloat("SFXVol", SFXVol);
        }

        PlayerPrefs.Save();
    }
    #endregion

    #region Load Prefs
    public void LoadPlayerPrefs()
    {
        //load fullscreen
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)
            {
                fullscreenToggle.isOn = false;
            }
            else
            {
                fullscreenToggle.isOn = true;
            }
        }
        //load audio Sliders
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVol");
            musicSlider.value = musicVol;
            masterAudio.SetFloat("MusicVol", musicVol);
        }
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float SFXVol = PlayerPrefs.GetFloat("SFXVol");
            SFXSlider.value = SFXVol;
            masterAudio.SetFloat("SFXVol", SFXVol);
        }
    }
    #endregion
}
