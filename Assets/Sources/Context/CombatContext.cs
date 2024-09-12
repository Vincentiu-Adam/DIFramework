﻿using System.Collections;
using UnityEngine;

public class CombatContext : MonoBehaviour
{
    [SerializeField]
    private Transform m_SpawnParent;

    public bool Finished { get; private set; } = false;

    private CombatSystem m_CombatSystem = null;
    private SpawnSystem m_SpawnSystem = null;
    private UnitRepository m_UnitRepository = null;

    private IEnumerator Start()
    {
        enabled = false;

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
