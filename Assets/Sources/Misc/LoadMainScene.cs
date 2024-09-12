using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return SceneLoadSystem.LoadSceneAsyncAdditive("Assets/Scenes/Main", new SceneData<MainContext>());

        //load main scene and immediately remove this useless scene that is just here because an addressable scene cannot be added to the unity scene list :upside_down_face
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
