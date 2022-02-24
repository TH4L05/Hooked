using UnityEngine;

public class MenuMain : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void Start()
    {
        Level.instance.gameData.activePlayerProfile = null;
        Level.instance.audioEvents.PlayAudioEvent("DampMusicStop", gameObject);
    }


    public void Quit()
    {

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    }
    #else
        Application.Quit();
    }
    #endif
}
