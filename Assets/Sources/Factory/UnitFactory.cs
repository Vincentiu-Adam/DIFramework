using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public uint ID;
    public UnitStatData UnitStat;
    public UnitVisualData VisualData;
}

public class UnitFactory
{
    private static uint idCounter = 1;

    public static Unit CreateUnit(IList<ArchetypeDataScriptableObject> archetypeDatas)
    {
        ArchetypeDataScriptableObject archetypeData = archetypeDatas[Random.Range(0, archetypeDatas.Count)];

        float unitHealth = Random.Range(archetypeData.Health.x, archetypeData.Health.y);
        float unitArmor = Random.Range(archetypeData.Armor.x, archetypeData.Armor.y);
        float unitDamage = Random.Range(archetypeData.Damage.x, archetypeData.Damage.y);

        Unit newUnit = new Unit()
        {
            ID = idCounter,
            UnitStat = new UnitStatData() { ID = idCounter, ArchetypeType = archetypeData.Type, Health = unitHealth, Armor = unitArmor, Dodge = 0f, Damage = unitDamage },
            VisualData = new UnitVisualData() { UnitObject = null }
        };

        idCounter++;
        return newUnit;
    }

    public static void Reset()
    {
        idCounter = 1;
    }
}
