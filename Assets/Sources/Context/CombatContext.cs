using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CombatContext : MonoBehaviour
{
    [SerializeField]
    private Transform m_SpawnParent;

    public bool Finished { get; private set; } = false;

    private CombatSystem m_CombatSystem = null;
    private SpawnSystem m_SpawnSystem = null;
    private UnitRepository m_UnitRepository = null;

    private const string ArchetypeLabel = "archetypes";

    private IEnumerator Start()
    {
        enabled = false;

#if UNITY_EDITOR
        //we might want to reset state of combat context after a scene load, before starting, so wait half a second when doing so
        yield return new WaitForSeconds(0.5f);

        if (m_LoadMainContext)
        {
            //just load main context scene with correct state and this will launch the combat instead
            yield return SceneLoadSystem.LoadMainContextSceneWithStateAsyncAdditive(MainContextState.COMBAT_INIT);
            yield break;
        }
#endif

        //launch update only after construct and init finished
        yield return Construct();
        yield return Init();

        enabled = true;

        yield return m_CombatSystem.Update();

        Finish();
    }

    private IEnumerator Construct()
    {
        var loadOp = Addressables.LoadAssetsAsync<ArchetypeDataScriptableObject>(ArchetypeLabel);
        yield return loadOp;

        m_UnitRepository = new UnitRepository();
        m_CombatSystem = new CombatSystem(m_UnitRepository, new AttackSystem(), loadOp.Result);

        m_SpawnSystem = new SpawnSystem(m_SpawnParent, m_UnitRepository);

        Addressables.Release(loadOp);

        yield return null;
    }

    private IEnumerator Init()
    {
        Debug.Log("Combat started");
        yield return m_SpawnSystem.Spawn();
    }

    //avoid using destroy to not conflict with other interfaces?
    private void Finish()
    {
        Debug.Log("Combat finished");

        m_CombatSystem.Destroy();
        m_CombatSystem = null;

        m_SpawnSystem.Destroy();
        m_SpawnSystem = null;

        m_UnitRepository.Destroy();
        m_UnitRepository = null;

        Finished = true;
        enabled = false;
    }

#if UNITY_EDITOR
    [SerializeField]
    private bool m_LoadMainContext;
    public void ResetLoadContext()
    {
        m_LoadMainContext = false;
    }
#endif
}
