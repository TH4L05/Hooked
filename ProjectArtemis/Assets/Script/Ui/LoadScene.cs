using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene: MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadSlider;

    private int sceneIndex = -1;
    private bool loadingStarted = false;

    private void Awake()
    {
        loadingStarted = false;
    }

    public void PlayDirectorLoadSpecificScene(int _sceneIndex)
    {
        if (!loadingStarted)
        {
            loadingStarted = true;
            sceneIndex = _sceneIndex;
            director.Play();
        }
    }

    public void PlayDirector()
    {
        if (!loadingStarted)
        {
            loadingStarted = true;
            director.Play();
        }
    }

    public void LoadAScene()
    {
        if (sceneIndex > -1)
        {
            //Debug.Log("load scene : " + SceneManager.GetSceneAt(sceneIndex).name);
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void LoadASceneAsync()
    {
        if (sceneIndex > -1)
        {
            //Debug.Log("load scene : " + SceneManager.GetSceneAt(sceneIndex).name);
            StartCoroutine(LoadAsynchron(sceneIndex));
        }

    }

    public void LoadSpecificScene(int sceneIndex)
    {
        //Debug.Log("load scene : " + SceneManager.GetSceneAt(sceneIndex).name);
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSpecificScene(string name)
    {
        //Debug.Log("load scene : " + name);
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadAsynchron(sceneIndex));
    }

    IEnumerator LoadAsynchron(int index)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);

        while (!load.isDone)
        {
            float progress = Mathf.Clamp01(load.progress / 0.9f);
            loadSlider.value = progress;
            //Debug.Log(progress);
            yield return null;
        }
    }

    public void SetLevelIndex(int idx)
    {
        sceneIndex = idx;
    }
}