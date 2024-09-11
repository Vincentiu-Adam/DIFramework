using System.Collections;
using UnityEngine;

public class CombatSystem
{
    private UnitRepository m_UnitRepository = null;
    private AttackSystem m_AttackSystem = null;

    public bool Finished { get; private set; } = false;

    private AttackData m_FirstUnitAttack = null;
    private AttackData m_SecondUnitAttack = null;

    public CombatSystem(UnitRepository unitRepository, AttackSystem attackSystem)
    {
        m_UnitRepository = unitRepository;
        m_AttackSystem = attackSystem;

        m_UnitRepository.AddUnitAtIndex(UnitFactory.CreateUnit(), 0);
        m_UnitRepository.AddUnitAtIndex(UnitFactory.CreateUnit(), 1);

        m_FirstUnitAttack = new AttackData() { ID = 0, Accuracy = 1f, Damage = 5 };
        m_SecondUnitAttack = new AttackData() { ID = 1, Accuracy = 1f, Damage = 10 };
    }

    public IEnumerator Update()
    {
        Unit firstUnit = m_UnitRepository[0];
        Unit secondUnit = m_UnitRepository[1];

        while (firstUnit.UnitStat.Health > 0)
        {
            yield return new WaitForSeconds(1f);

            float health = m_AttackSystem.GetHealthAfterAttack(firstUnit.UnitStat, m_SecondUnitAttack);
            Debug.LogFormat("First unit {0} attacked for {1}", firstUnit.ID, m_SecondUnitAttack.Damage);

            firstUnit.UnitStat.Health = health;
            yield return new WaitForSeconds(1f);

            health = m_AttackSystem.GetHealthAfterAttack(secondUnit.UnitStat, m_FirstUnitAttack);
            secondUnit.UnitStat.Health = health;

            Debug.LogFormat("Second unit {0} attacked for {1}", secondUnit.ID, m_FirstUnitAttack.Damage);

            yield return new WaitForEndOfFrame();
        }

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
