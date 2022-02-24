using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class IngameMenu : MonoBehaviour
{
    [Header("General")]
    public static bool GamePaused;
    public bool pauseMenuActive;
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public IngameUI ingameUI;

    public UnityEvent TriggerEventOnMenuOpen;
    public UnityEvent TriggerEventOnMenuClose;

    [Header("Playables")]
    public PlayableDirector showPauseMenu;
    public PlayableDirector hidePauseMenu;
    public PlayableDirector showOptionsMenu;
    public PlayableDirector hideOptionsMenu;

    

    public void Awake()
    {
        GamePaused = false;
        var options = optionMenu.GetComponent<OptionsMenu>();
        if (!options) return;
        options.Setup();
        options.SetSensitivityInPlayer();
        //ingameUI.UpdatePullText();
    }

    public void ToggleMenu()
    {
        if (GamePaused && !pauseMenuActive)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void ToggleOptionMenu(bool active)
    {
        pauseMenuActive = active;
        optionMenu.SetActive(active);
        pauseMenu.SetActive(!active);
    }

    public void Pause()
    {
        GamePaused = true;
        ChangeCursorVisibility(true);
        showPauseMenu?.Play(); 
        TriggerEventOnMenuOpen?.Invoke();
        Level.instance.audioEvents.PlayAudioEvent("OpenPauseMenu", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("DampMusicStart", gameObject);
        Level.instance.inputHandler.InputHandlerIsPaused(true);
        Level.instance.inputHandler.EnableInputActions(false);
        //pauseMenu.SetActive(true);
        //SetTimeScale(0f);
        
        //Debug.Log(GamePaused);
    }
    public void Resume()
    {
        GamePaused = false;
        //SetTimeScale(1f);
        //pauseMenu.SetActive(false);
        hidePauseMenu.Play();
        Level.instance.audioEvents.PlayAudioEvent("ClosePauseMenu", gameObject);
        Level.instance.audioEvents.PlayAudioEvent("DampMusicStop", gameObject);
        Level.instance.inputHandler.EnableInputActions(true);
        Level.instance.inputHandler.InputHandlerIsPaused(false);
        TriggerEventOnMenuClose?.Invoke();
        ChangeCursorVisibility(false);
        //Debug.Log(GamePaused);
        ingameUI.UpdatePullText();
    }

    public void ChangeCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        //Debug.Log(Time.timeScale);
    }

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    
    #else
        Application.Quit();
    
    #endif
    }

    #region Audio

    public void Play_Button_Click_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("ButtonClick", gameObject);
    }

    public void Play_Button_Hover_Audio()
    {
        Level.instance.audioEvents.PlayAudioEvent("ButtonHover", gameObject);
    }

    public void PlayStopMusicDampAudio()
    {
        Level.instance.audioEvents.PlayAudioEvent("DampMusicStop", gameObject);
    }


    #endregion Audio
}
