
public class UnitRepository
{
    private Unit[] m_Units = new Unit[100];

    public Unit this[uint index] => index < 99 ? m_Units[index] : null;

    public void AddUnitAtIndex(Unit unit, int index)
    {
        if (index > 99)
        {
            return;
        }

        m_Units[index] = unit;
    }

    public Unit GetUnitByID(uint id)
    {
        return this[id];
    }
}
