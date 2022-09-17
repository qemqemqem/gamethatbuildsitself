using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelInfo
{
    public int num;
    public string author;
    public string info;
    public string details;
}

public class SplashScreens : MonoBehaviour
{
    public UnityEngine.UI.Text welcomeScreen;
    public UnityEngine.UI.Text levelIntroScreen;
    public UnityEngine.UI.Text gameEndScreen;
    public UnityEngine.UI.Image mainPanel;
    public UnityEngine.UI.Text deathScreen;
    public UnityEngine.UI.Text levelNotImplementedScreen;
    public UnityEngine.UI.Text nextLevelQuestionsScreen;

    UnityEngine.UI.Text currentScreen = null;
    float currentScreenRenderStartTime = -1;
    float renderDuration = 0;
    string fullCurrentScreenText = "";

    List<LevelInfo> levels = new List<LevelInfo>();
    int currentLevel = -1;

    string currentPlayer = "";

    World world = new World();

    public static SplashScreens splashScreens; // Singleton

    void StartRenderScreen(UnityEngine.UI.Text screen, float duration)
    {
        mainPanel.gameObject.SetActive(true);
        currentScreen = screen;
        currentScreenRenderStartTime = UnityEngine.Time.time;
        renderDuration = duration;
        fullCurrentScreenText = screen.text;
        screen.text = "";
        currentScreen.gameObject.SetActive(true);
    }

    void UpdateRenderScreen()
    {
        if (currentScreen == null) return;

        // This adds lines 1 at a time with even spacing
        //string[] lines = fullCurrentScreenText.Split('\n');
        //int numLines = (int)(lines.Length * ((Time.time - currentScreenRenderStartTime) / renderDuration));
        //currentScreen.text = string.Join("\n", lines.Take<string>(numLines)) + new string('\n', lines.Length - numLines);

        // This adds lines one at a time with pacing proportionate to their length. I think it looks better.
        int extraPacing = 10;
        string[] lines = fullCurrentScreenText.Split('\n');
        int numCharsToUse = (int)((fullCurrentScreenText.Length + lines.Length * extraPacing) * ((Time.time - currentScreenRenderStartTime) / renderDuration));
        int numLines = 0;
        int ccount = 0;
        foreach (string line in lines)
        {
            ccount += line.Length + extraPacing;
            numLines++;
            if (ccount >= numCharsToUse) break;
        }
        currentScreen.text = string.Join("\n", lines.Take<string>(numLines)) + new string('\n', lines.Length - numLines);

        if (UnityEngine.Time.time >= currentScreenRenderStartTime + renderDuration)
        {
            currentScreen.gameObject.SetActive(false);
            UnityEngine.UI.Text finishedScreen = currentScreen;
            currentScreen = null;
            mainPanel.gameObject.SetActive(false);

            // TODO These should probably be callbacks or something.
            if (finishedScreen == welcomeScreen)
            {
                StartLevelScreen(1);
            }
            if (finishedScreen == levelIntroScreen)
            {
                PlayLevel(currentLevel);
            }
            if (finishedScreen == deathScreen)
            {
                StartRenderScreen(gameEndScreen, 6f);
            }
            if (finishedScreen == gameEndScreen || finishedScreen == levelNotImplementedScreen)
            {
                QuitGame();
            }
        }
    }

    public static void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }

    void AskNextLevelQuestions()
    {
        mainPanel.gameObject.SetActive(true);
        nextLevelQuestionsScreen.gameObject.SetActive(true);
    }

    void StartLevelScreen(int levelNum)
    {
        currentLevel = levelNum;
        levelIntroScreen.text = "LEVEL " + levelNum + "\n\n" + levels[currentLevel].info + "\n\n" + levels[currentLevel].details + "\n\nfrom " + levels[currentLevel].author;
        StartRenderScreen(levelIntroScreen, 3f);
    }

    void PlayLevel(int levelNum)
    {
        world.currentLevel = currentLevel;
        World.world = world;

        if (currentLevel >= 1) Level1.WorldCreation(world);
        if (currentLevel >= 2) Level2.WorldCreation(world);

        // TODO Code generation for future levels

        if (world.levelNotImplemented) StartRenderScreen(levelNotImplementedScreen, 3f);
    }

    public void TryStartLevel(int levelNum)
    {
        if (levelNum >= levels.Count)
        {
            AskNextLevelQuestions();
        } else
        {
            StartLevelScreen(levelNum);
        }
    }

    public void ShowDeathScreen()
    {
        StartRenderScreen(deathScreen, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        splashScreens = this;

        // Load level information
        string[] levelStrings = ((TextAsset)Resources.Load("levels")).text.Split('\n');
        foreach (string levelDetails in levelStrings)
        {
            LevelInfo info = new LevelInfo();
            string[] levelParts = levelDetails.Split('\t'); // It's a tsv
            if (levelParts.Length < 4) continue;
            info.num = int.Parse(levelParts[0]);
            info.author = levelParts[1];
            info.info = levelParts[2];
            info.details = levelParts[3];
            levels.Add(info);
        }

        // TODO Set random seed for replayability

        StartRenderScreen(welcomeScreen, 2.8f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRenderScreen();
    }
}
