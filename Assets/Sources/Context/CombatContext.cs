using System.Collections;
using UnityEngine;

public class CombatContext : MonoBehaviour
{
    public bool Finished { get; private set; } = false;

    private IEnumerator Start()
    {
        enabled = false;

        //launch update only after construct and init finished
        yield return Construct();
        Init();

        enabled = true;

        yield return new WaitForSeconds(2f);

        Finish();
    }

    private IEnumerator Construct()
    {
        yield return null;
    }

    private void Init()
    {
        Debug.Log("Combat started");
    }

    //avoid using destroy to not conflict with other interfaces?
    private void Finish()
    {
        Debug.Log("Combat finished");

        Finished = true;
        enabled = false;
    }
}
