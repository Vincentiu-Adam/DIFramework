using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnSystem
{
    private Transform m_SpawnParent = null;
    private UnitRepository m_UnitRepository = null;

    private const string UnitPrefabName = "Assets/Prefabs/Cube";

    private AsyncOperationHandle<GameObject> m_LoadOperation;

    public SpawnSystem(Transform spawnParent, UnitRepository unitRepository)
    {
        m_SpawnParent = spawnParent;
        m_UnitRepository = unitRepository;
    }

    public IEnumerator Spawn()
    {
        m_LoadOperation = Addressables.LoadAssetAsync<GameObject>(UnitPrefabName);

        // yielding when already done still waits until the next frame; so don't yield if done.
        if (!m_LoadOperation.IsDone)
        {
            yield return m_LoadOperation;
        }

        //spawn a new unit on each position
        uint i = 0;
        foreach (Transform child in m_SpawnParent)
        {
            GameObject unit = Object.Instantiate(m_LoadOperation.Result, child, false);
            unit.name = "Unit_" + (i + 1);

            //set unit data to unit from repository
            m_UnitRepository[i].VisualData.UnitObject = unit;

            i++;
        }
    }

    public void Destroy()
    {
        m_SpawnParent = null;
        m_UnitRepository = null;

        //release handle since we don't need it anymore
        Addressables.Release(m_LoadOperation);
    }
}
