using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyTeamId : MonoBehaviour
{

    public Text input;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int getTeamId()
    {
        try
        {
            return int.Parse(input.text.ToString());
        }
        catch
        {
            return 0;
        }
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public int hideAndGetTeamId()
    {
        hide();
        return getTeamId();
    }
}
