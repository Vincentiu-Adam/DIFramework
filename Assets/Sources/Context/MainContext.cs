﻿#if UNITY_EDITOR
#define RELOAD //for conditional method
#endif

using System.Collections;
using UnityEngine;

public enum MainContextState : uint
{
    UI_INIT,
    UI_UPDATE,
    COMBAT_INIT,
    COMBAT_UPDATE
}

public class MainContext : MonoBehaviour
{
    private SceneData<UIContext> m_UISceneData;
    private SceneData<CombatContext> m_CombatSceneData;

    private const string UISceneName = "Assets/Scenes/UI";
    private const string CombatSceneName = "Assets/Scenes/Combat";

    private MainContextState m_State = MainContextState.UI_INIT;

    public void SetState(MainContextState state)
    {
        m_State = state;
    }

    private IEnumerator Start()
    {
        //we might want to change state of main context after a scene load, before starting, so wait half a second when doing so
        yield return new WaitForSeconds(0.5f);

        yield return UpdateCoroutine();
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            switch (m_State)
            {
                case MainContextState.UI_INIT:
                    yield return ConstructUI();
                    InitUI();
                    break;

                case MainContextState.UI_UPDATE:
                    if (m_UISceneData.SceneContext.Finished)
                    {
                        //unload ui and maybe go to the combat scene?
                        DeconstructUI();
                    }
                    break;

                case MainContextState.COMBAT_INIT:
                    yield return ConstructCombat();
                    InitCombat();
                    break;

                case MainContextState.COMBAT_UPDATE:
                    if (m_CombatSceneData.SceneContext.Finished)
                    {
                        DeconstructCombat();
                    }
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ConstructUI()
    {
        m_UISceneData = new SceneData<UIContext>();
        yield return ConstructScene(UISceneName, m_UISceneData);
    }

    private IEnumerator ConstructCombat()
    {
        m_CombatSceneData = new SceneData<CombatContext>();
        yield return ConstructScene(CombatSceneName, m_CombatSceneData);
    }

    private void InitUI()
    {
        m_State = MainContextState.UI_UPDATE;
    }

    private void InitCombat()
    {
        m_State = MainContextState.COMBAT_UPDATE;
    }

    //avoid using destroy to not conflict with other interfaces?
    private void DeconstructUI()
    {
        UnloadUIScene();
        m_State = MainContextState.COMBAT_INIT;
    }

    private void DeconstructCombat()
    {
        UnloadCombatScene();
        m_State = MainContextState.UI_INIT;
    }

    private void UnloadUIScene()
    {
        SceneLoadSystem.UnloadSceneAsync(m_UISceneData.LoadedScene);
        m_UISceneData = null;
    }

    private void UnloadCombatScene()
    {
        SceneLoadSystem.UnloadSceneAsync(m_CombatSceneData.LoadedScene);
        m_CombatSceneData = null;
    }

    private IEnumerator ConstructScene<T>(string sceneName, SceneData<T> sceneData) where T : Object
    {
        yield return new WaitForSeconds(2f);
        yield return SceneLoadSystem.LoadSceneAsyncAdditive(sceneName, sceneData);

        ResetLoadMainSceneContext();
    }

    [System.Diagnostics.Conditional("RELOAD")]
    private void ResetLoadMainSceneContext()
    {
        Debug.Log("Reset called");
#if UNITY_EDITOR
        if (m_CombatSceneData?.SceneContext != null)
        {
            m_CombatSceneData.SceneContext.ResetLoadContext();
        }

        LoadMainContext loadMainContext = FindAnyObjectByType<LoadMainContext>();
        if (loadMainContext != null)
        {
            loadMainContext.StopLoad();
        }
#endif
    }
}
