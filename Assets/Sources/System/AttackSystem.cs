using UnityEngine;

public class AttackSystem
{
    public float GetHealthAfterAttack(UnitStatData unitStat, AttackData attack)
    {
        bool hit = Random.value < (attack.Accuracy - unitStat.Dodge);
        if (!hit)
        {
            return 0f;
        }

        return Mathf.Max(0f, unitStat.Health + unitStat.Armor - attack.Damage);
    }
}
