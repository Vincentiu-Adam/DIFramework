
public class Unit
{
    public uint ID;
    public UnitStatData UnitStat;
}


public class UnitFactory
{
    private static uint idCounter = 1;

    public static Unit CreateUnit()
    {
        Unit newUnit = new Unit() {ID = idCounter, UnitStat = new UnitStatData() { ID = idCounter, Health = 40, Armor = 2f, Dodge = 0f } } ;
        idCounter++;

        return newUnit;
    }

    public static void Reset()
    {
        idCounter = 1;
    }
}
