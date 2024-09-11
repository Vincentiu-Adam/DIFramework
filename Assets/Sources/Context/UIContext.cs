using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIContext : MonoBehaviour
{
    [SerializeField]
    private Button m_FirstButton;

    [SerializeField]
    private Button m_SecondButton;

    public bool Finished { get; private set; } = false;

    private IEnumerator Start()
    {
        enabled = false;

        //launch update only after construct and init finished
        yield return Construct();
        Init();

        enabled = true;
    }

    private void Update()
    {
    }

    private IEnumerator Construct()
    {
        m_FirstButton.onClick.AddListener(FirstButtonClick);
        m_SecondButton.onClick.AddListener(SecondButtonClick);

        yield return null;
    }

    private void FirstButtonClick()
    {
        Debug.Log("Clicked on button");
    }

    private void SecondButtonClick()
    {
        Debug.Log("Clicked on second button");
        End();
    }

    private void Init()
    {
        Finished = false;

        m_SecondButton.Select();
    }

    private void End()
    {
        Finished = true;

        Finish();
    }

    //avoid using destroy to not conflict with other interfaces?
    private void Finish()
    {
        m_FirstButton = null;
        m_SecondButton = null;
    }
}
