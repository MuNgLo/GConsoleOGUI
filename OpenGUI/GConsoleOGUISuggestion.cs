using UnityEngine;
using System.Collections;


[AddComponentMenu("Scripts/GConsoleOGUISuggestion")]
public class GConsoleOGUISuggestion : MonoBehaviour
{
	public OGButton label;
	public GConsoleOGUI consoleOGUI;

	void Start()
	{
		label.target = consoleOGUI.transform.gameObject;
		label.message = "pickSuggestion";
	}

    public void ShowSuggestion(string s)
    {
        if (string.IsNullOrEmpty(s))
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            label.text = s;
			label.argument = s;
        }
    }
}
