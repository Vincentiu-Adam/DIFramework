using System.Collections;
using UnityEngine;

public class CombatContext : MonoBehaviour
{
    [SerializeField]
    private Transform m_SpawnParent;
    [SerializeField]
    private bool m_LoadMainContext;

    public bool Finished { get; private set; } = false;

    private CombatSystem m_CombatSystem = null;
    private SpawnSystem m_SpawnSystem = null;
    private UnitRepository m_UnitRepository = null;

    public void ResetLoadContext()
    {
        m_LoadMainContext = false;
    }

    private IEnumerator Start()
    {
        enabled = false;

        //we might want to reset state of combat context after a scene load, before starting, so wait half a second when doing so
        yield return new WaitForSeconds(0.5f);

        //to do (fix) : find a way to not let this load itself recursively :upside_down_face
        if (m_LoadMainContext)
        {
            //just load main context scene with correct state and this will launch the combat instead
            yield return SceneLoadSystem.LoadMainContextSceneWithStateAsyncAdditive(MainContextState.COMBAT_INIT);
            yield break;
        }

        //launch update only after construct and init finished
        yield return Construct();
        yield return Init();

        enabled = true;

        yield return m_CombatSystem.Update();

        Finish();
    }

    private IEnumerator Construct()
    {
        m_UnitRepository = new UnitRepository();
        m_SpawnSystem = new SpawnSystem(m_SpawnParent, m_UnitRepository);

        m_CombatSystem = new CombatSystem(m_UnitRepository, new AttackSystem());
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
}
