using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GConsoleOGUI : MonoBehaviour {

	public OGConsoleFeed textList;
	public OGConsoleInput input;
	public GConsole gconsole;
    public GConsoleOGUISuggestion[] suggestions;

    public bool clearOnSubmit = true;
    public bool reselectOnSubmit = true;

    public int minCharBeforeSuggestions;

	void Start () {
        //Register the "OnOutput" method as a listener for console output.
	    GConsole.OnOutput += OnOutput;
		gconsole.useColoredText = false;
        
		input = this.transform.FindChild ("Container").transform.FindChild ("Input").GetComponent<OGConsoleInput> ();

	}

    void OnOutput(string line)
    {
       //textList.Add(line);
		textList.AddFirst (line);
    }

    public void OnInput()
    {
		input.submit = false;
        string cmd = input.text;

        if (string.IsNullOrEmpty (cmd)) {
						return;
				}

        //Send command to the console
        GConsole.Eval(cmd);

        if (clearOnSubmit) {
						input.text = string.Empty;
				}

        if (reselectOnSubmit) //Has to be done in next frame for NGUI reasons (quirk in NGUI)..
            StartCoroutine(SelectInputNextFrame());
    }

    IEnumerator SelectInputNextFrame()
    {
        yield return null;
        input.listening = true;

    }
	
    IEnumerator ClearInputNextFrame()
    {
        yield return null;
        input.text = string.Empty;

    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
		*/
		if (input.close) {
						gameObject.SetActive (false);
						input.close = false;
				}
		if (input.submit) {
						OnInput ();
				}
				
		if (input.changed) {
			OnChange ();
		}
    }


    void OnEnable()
    {
        //Setting the input as selected when the console is opened.
        input.listening = true;
        StartCoroutine(ClearInputNextFrame()); //Necessary because otherwise the input field will contain the letter used to open the UI.
        //Another NGUI quirk!
    }


    void OnChange()
    {
        LoadSuggestions();
    }

    private void LoadSuggestions()
    {
        List<String> sugstrings;

        //Not enough characters typed yet, no suggestions to be shown!
        if (minCharBeforeSuggestions != 0 && input.text.Length < minCharBeforeSuggestions)
        {
            sugstrings = new List<String>();
        }
        else
        {
            //Ask GConsole for suggestions, true because we want to have the description too.
            sugstrings = GConsole.GetSuggestions(input.text, true);
        }

        //Display suggestions (and hide unused suggestion boxes by passing null).
        for (int i = 0; i < suggestions.Length; i++)
        {
            if (i < sugstrings.Count)
                suggestions[i].ShowSuggestion(sugstrings[i]);
            else
                suggestions[i].ShowSuggestion(null);
        }
        
    }
    
    public void pickSuggestion(string cmd)
    {
        // TODO OpenGUI suggestion to command here
		Debug.Log ("pickSuggestion(" + cmd + ")");
		GConsole.Eval( cmd );
        //Reselect the input the next frame (quirk in NGUI, can't do it the current).
      StartCoroutine(SelectInputNextFrame());
    }
}
