
public class UnitRepository
{
    private Unit[] m_Units = new Unit[100];

    public Unit this[uint index] => index < 99 ? m_Units[index] : null;

    private int m_Count = 0;

    public void AddUnitAtIndex(Unit unit, int index)
    {
        if (index > 99)
        {
            return;
        }

        m_Units[index] = unit;
        m_Count++;
    }

    public Unit GetUnitByID(uint id)
    {
        //id starts at 1
        return this[id - 1];
    }

    public void Destroy()
    {
        for (int i = 0; i < m_Count; i++)
        {
            m_Units[i] = null;
        }
    }
}
