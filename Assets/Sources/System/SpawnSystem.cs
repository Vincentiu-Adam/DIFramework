using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpawnSystem
{
    private Transform m_SpawnParent = null;
    private UnitRepository m_UnitRepository = null;

    private const string UnitPrefabName = "Assets/Prefabs/Cube";

    public SpawnSystem(Transform spawnParent, UnitRepository unitRepository)
    {
        m_SpawnParent = spawnParent;
        m_UnitRepository = unitRepository;
    }

    public IEnumerator Spawn()
    {
        AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>(UnitPrefabName);

        // yielding when already done still waits until the next frame; so don't yield if done.
        if (!loadOp.IsDone)
        {
            yield return loadOp;
        }

        //spawn a new unit on each position
        uint i = 0;
        foreach (Transform child in m_SpawnParent)
        {
            GameObject unit = Object.Instantiate(loadOp.Result, child, false);
            unit.name = "Unit_" + i;

            //set unit data to unit from repository
            m_UnitRepository[i].VisualData.UnitObject = unit;

            i++;
        }
    }

    public void Destroy()
    {
        m_SpawnParent = null;
        m_UnitRepository = null;
    }
}
