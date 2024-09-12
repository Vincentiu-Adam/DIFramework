using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneData<T> where T : Object
{
    public SceneInstance LoadedScene;
    public T SceneContext;
}

public class SceneLoadSystem
{
    //scene data here is a ref (construction is done outside); in Ienumerator ref keyword is not allowed
    public static IEnumerator LoadSceneAsyncAdditive<T>(string sceneName, SceneData<T> sceneData) where T : Object
    {
        var loadOperation = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOperation;

        sceneData.LoadedScene = loadOperation.Result;
        sceneData.SceneContext = Object.FindAnyObjectByType<T>();

        //extra wait to make sure that data is set
        yield return null;
    }

    public static IEnumerator LoadMainContextSceneWithStateAsyncAdditive(MainContextState state)
    {
        var loadOperation = Addressables.LoadSceneAsync("Assets/Scenes/Main", LoadSceneMode.Additive);
        yield return loadOperation;

        MainContext mainContext = Object.FindAnyObjectByType<MainContext>();
        mainContext.SetState(state);

        //also unload current scene since once main scene is loaded this will construct all the other scenes
        //start a coroutine on main context because this call is usually called from the active scene that will unload itself :/
        mainContext.StartCoroutine(UnloadActiveSceneAndEnableMainSceneContext(mainContext));
    }

    public static void UnloadSceneAsync(SceneInstance sceneInstance)
    {
        Addressables.UnloadSceneAsync(sceneInstance);
    }

    private static IEnumerator UnloadActiveSceneAndEnableMainSceneContext(MainContext mainContext)
    {
        mainContext.enabled = false; //we don't want main scene to start yet until we give it the go

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //now we can start in the correct state
        mainContext.enabled = true;
    }
}
