using UnityEngine;

public enum ArchetypeType
{
    LIGHT,
    MEDIUM,
    HEAVY
}

[CreateAssetMenu(fileName = "unit_archetype_", menuName = "Scriptable Objects/ArchetypeDataScriptableObject")]
public class ArchetypeDataScriptableObject : ScriptableObject
{
    public ArchetypeType Type;
    public Vector2 Damage;
    public Vector2 Health;
    public Vector2 Armor;
}
