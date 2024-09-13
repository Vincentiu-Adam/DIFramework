using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
    private UnitRepository m_UnitRepository = null;
    private AttackSystem m_AttackSystem = null;

    public bool Finished { get; private set; } = false;

    private AttackData m_FirstUnitAttack = null;
    private AttackData m_SecondUnitAttack = null;

    public CombatSystem(UnitRepository unitRepository, AttackSystem attackSystem, IList<ArchetypeDataScriptableObject> archetypeDatas)
    {
        m_UnitRepository = unitRepository;
        m_AttackSystem = attackSystem;

        m_UnitRepository.AddUnitAtIndex(UnitFactory.CreateUnit(archetypeDatas), 0);
        m_UnitRepository.AddUnitAtIndex(UnitFactory.CreateUnit(archetypeDatas), 1);

        m_FirstUnitAttack = new AttackData() { ID = 0, Accuracy = 1f, Damage = m_UnitRepository[0].UnitStat.Damage };
        m_SecondUnitAttack = new AttackData() { ID = 1, Accuracy = 1f, Damage = m_UnitRepository[1].UnitStat.Damage };
    }

    public IEnumerator Update()
    {
        Unit firstUnit = m_UnitRepository[0];
        Unit secondUnit = m_UnitRepository[1];

        while (firstUnit.UnitStat.Health > 0 && secondUnit.UnitStat.Health > 0)
        {
            yield return new WaitForSeconds(1f);

            float health = m_AttackSystem.GetHealthAfterAttack(firstUnit.UnitStat, m_SecondUnitAttack);
            firstUnit.UnitStat.Health = health;

            Debug.LogFormat("First unit {0} attacked for {1}", firstUnit.ID, m_SecondUnitAttack.Damage);
            Debug.LogFormat("Unit has archetype {0} and visual {1} and health {2}", firstUnit.UnitStat.ArchetypeType, firstUnit.VisualData.UnitObject.name, firstUnit.UnitStat.Health);

            yield return new WaitForSeconds(1f);

            health = m_AttackSystem.GetHealthAfterAttack(secondUnit.UnitStat, m_FirstUnitAttack);
            secondUnit.UnitStat.Health = health;

            Debug.LogFormat("Second unit {0} attacked for {1}", secondUnit.ID, m_FirstUnitAttack.Damage);
            Debug.LogFormat("Unit has archetype {0} and visual {1} and health {2}", secondUnit.UnitStat.ArchetypeType, secondUnit.VisualData.UnitObject.name, secondUnit.UnitStat.Health);

            yield return new WaitForEndOfFrame();
        }

        Unit winningUnit = firstUnit.UnitStat.Health > secondUnit.UnitStat.Health ? firstUnit : secondUnit;
        Debug.LogFormat("Unit {0} won :D {1} {2}", winningUnit.ID, winningUnit.VisualData.UnitObject.name, winningUnit.UnitStat.ArchetypeType);

        Finished = true;
    }

    public void Destroy()
    {
        m_UnitRepository = null;
        m_AttackSystem = null;

        m_FirstUnitAttack = null;
        m_SecondUnitAttack = null;

        UnitFactory.Reset();
    }
}
