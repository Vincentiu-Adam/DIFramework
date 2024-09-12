#if UNITY_EDITOR
using System.Collections;
using UnityEngine;

public class LoadMainContext : MonoBehaviour
{
    [SerializeField]
    private MainContextState m_MainContextState = MainContextState.UI_INIT;

    private bool m_LoadAtStartup = true;

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        if (m_LoadAtStartup)
        {
            //just load main context scene with correct state and this will launch the combat instead
            yield return SceneLoadSystem.LoadMainContextSceneWithStateAsyncAdditive(m_MainContextState);
            yield break;
        }
    }

    public void StopLoad()
    {
        m_LoadAtStartup = false;
    }
}
#endif
