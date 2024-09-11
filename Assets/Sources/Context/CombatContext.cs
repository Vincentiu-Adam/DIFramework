using System.Collections;
using UnityEngine;

public class CombatContext : MonoBehaviour
{
    public bool Finished { get; private set; } = false;

    private CombatSystem m_CombatSystem = null;

    private IEnumerator Start()
    {
        enabled = false;

        //launch update only after construct and init finished
        yield return Construct();
        Init();

        enabled = true;

        yield return m_CombatSystem.Update();

        Finish();
    }

    private IEnumerator Construct()
    {
        m_CombatSystem = new CombatSystem(new UnitRepository(), new AttackSystem());
        yield return null;
    }

    private void Init()
    {
        Debug.Log("Combat started");
    }

    //avoid using destroy to not conflict with other interfaces?
    private void Finish()
    {
        Debug.Log("Combat finished");

        m_CombatSystem.Destroy();
        m_CombatSystem = null;

        Finished = true;
        enabled = false;
    }
}
