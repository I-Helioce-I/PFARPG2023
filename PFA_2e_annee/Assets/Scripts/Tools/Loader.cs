using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene
    {
        Loading,
        MainMenu,
        Scene_Test_Bart,
        CombatScene_Test_Bart,
        //Fill this with the Scene names
    }

    private static Action OnLoaderCallBack;
    private static AsyncOperation loadingAsyncOperation;

    public static void LoadSingle(Scene scene)
    {
        OnLoaderCallBack = () => {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            LoadSceneAsync(scene);
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
        GameManager.instance.CurrentScenes.Clear();
        GameManager.instance.CurrentScenes.Add(scene);
    }

    public static void LoadAdditive(Scene scene)
    {
        OnLoaderCallBack = () => {
            UnityEngine.SceneManagement.Scene createdScene = SceneManager.GetSceneByName(scene.ToString());
            SceneManager.SetActiveScene(createdScene);
        };

        SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
        GameManager.instance.CurrentScenes.Add(scene);
    }

    public static void UnloadScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene.ToString());
        GameManager.instance.CurrentScenes.Remove(scene);
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }

    public static void LoaderCallBack()
    {
        if (OnLoaderCallBack != null)
        {
            OnLoaderCallBack();
            OnLoaderCallBack = null;
        }
    }
}
