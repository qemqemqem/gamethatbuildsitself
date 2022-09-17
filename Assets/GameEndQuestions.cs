using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameEndQuestions : MonoBehaviour
{
    public UnityEngine.UI.Text mainText;

    public UnityEngine.UI.InputField inputField;

    int stage = 0;

    float endTime = 0;

    List<string> ideaSuggestions = new List<string>();

    string chosenIdea = "";
    string details = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    void ShowTaskOptions()
    {
        string[] ideas = ((TextAsset)Resources.Load("ideas")).text.Split('\n');
        foreach (string idea in ideas)
        {
            string[] parts = idea.Split('\t');
            if (parts.Length < 2) continue;
            if (parts[0] == "_") ideaSuggestions.Add(parts[1]);
            if (ideaSuggestions.Count >= 3) break;
        }
        mainText.text = "Great!\n\nNow let's design the next level.\n\nChoose one:\n\n    ";
        string keys = "abc";
        for (int i = 0; i < ideaSuggestions.Count; i++)
        {
            mainText.text = mainText.text + keys[i].ToString() + ": " + ideaSuggestions[i] + "    ";
        }
    }

    void WriteNextLevel()
    {
        // Load level information
        string levelsText = "";
        string[] levelStrings = ((TextAsset)Resources.Load("levels")).text.Split('\n');
        int nextLevelNum = 0;
        string lastAuthor = "andrew";
        foreach (string levelDetails in levelStrings)
        {
            string[] levelParts = levelDetails.Split('\t'); // It's a tsv
            if (levelParts.Length < 4) continue;
            lastAuthor = levelParts[1];
            nextLevelNum++;
            levelsText = levelsText + levelDetails + "\n";
        }
        string nextAuthor = lastAuthor == "andrew" ? "chris" : "andrew"; // Alternate in a not very data driven way.

        string levelsFile = Directory.GetCurrentDirectory() + "/Assets/Resources/levels.txt"; // TODO Uh this is going to fail on Windows
        print("curdir " + levelsFile);
        File.WriteAllText(levelsFile, levelsText + nextLevelNum.ToString() + "\t" + nextAuthor + "\t" + chosenIdea + "\t" + details);

        // TODO Also update ideas.txt
    }

    // Update is called once per frame
    void Update()
    {
        if (endTime > 0 && Time.time > endTime)
        {
            SplashScreens.QuitGame();
        }
        if (stage == 0)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                mainText.text = "Okay, keep working on it!";
                endTime = Time.time + 3f;
                stage = -1;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                ShowTaskOptions();
                stage = 1;
            }
        }
        if (stage == 1)
        {
            if (Input.GetKeyDown(KeyCode.A)) chosenIdea = ideaSuggestions[0];
            if (Input.GetKeyDown(KeyCode.B)) chosenIdea = ideaSuggestions[1];
            if (Input.GetKeyDown(KeyCode.C)) chosenIdea = ideaSuggestions[2];
            if (chosenIdea != "")
            {
                stage = 2;
                mainText.text = "OK cool, give some details, 30 characters or less, then press enter:";
                inputField.gameObject.SetActive(true);
                inputField.Select();
                inputField.ActivateInputField();
            }
        }
        if (stage == 2)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                details = inputField.text;
                print("NEXT LEVEL: " + chosenIdea + " " + details);
                WriteNextLevel();
                mainText.text = "Great! This next level is going to be good!";
                inputField.gameObject.SetActive(false);
                endTime = Time.time + 3f;
            }
        }
    }
}
