using System.Collections;
using UnityEngine;
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
        var loadOperation = Addressables.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        yield return loadOperation;

        sceneData.LoadedScene = loadOperation.Result;
        sceneData.SceneContext = Object.FindAnyObjectByType<T>();

        //extra wait to make sure that data is set
        yield return null;
    }

    public static void UnloadSceneAsync(SceneInstance sceneInstance)
    {
        Addressables.UnloadSceneAsync(sceneInstance);
    }
}
