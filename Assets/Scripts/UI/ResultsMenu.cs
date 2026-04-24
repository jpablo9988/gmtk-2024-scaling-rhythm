using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button toNextButton;
    [SerializeField] private TextMeshProUGUI toNextButtonText;
    [SerializeField] private List<string> resultsQuotes;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private List<TextMeshProUGUI> individualScores;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Scenes nextIndex;

    public ScoreType ComputeResults(RhythmTrack track)
    {
        ScoreType finalResult;
        switch (ScoreTally.ScoreResults(track.Map.BeatList.Count))
        {
            case ScoreType.Perfect:
                text.text = resultsQuotes[0];
                image.sprite = images[0];
                finalResult = ScoreType.Perfect;
                break;
            case ScoreType.Good:
                text.text = resultsQuotes[1];
                image.sprite = images[1];
                finalResult = ScoreType.Good;
                break;
            case ScoreType.Meh:
                text.text = resultsQuotes[2];
                image.sprite = images[2];
                finalResult = ScoreType.Meh;
                break;
            default:
                text.text = resultsQuotes[3];
                image.sprite = images[3];
                finalResult = ScoreType.Miss;
                break;
        }
        Assert.AreEqual(individualScores.Count, 4);
        individualScores[0].text = ScoreTally.Perfect.ToString();
        individualScores[1].text = ScoreTally.Good.ToString();
        individualScores[2].text = ScoreTally.Meh.ToString();
        individualScores[3].text = (track.Map.BeatList.Count - ScoreTally.TotalScore).ToString();
        ScoreTally.ResetScore(addToUniversal: true);
        PersistProgressionResults(finalResult, ScoreTally.GotPerfectResults(track.Map.BeatList.Count));
        panel.SetActive(true);
        if (finalResult >= ScoreType.Meh || nextIndex == Scenes.MAIN_MENU) //Only can go to next if ScoreType is Good or Perfect
        {
            toNextButton.interactable = false;
            toNextButtonText.alpha = 0.4f;
            toNextButtonText.text = "Blocked";

        }
        return finalResult;
    }
    private void PersistProgressionResults(ScoreType finalResult, bool perfectedEverything)
    {
        Debug.Log(finalResult <= ScoreType.Good);
        ProgressionManager.ChangeLevelProgress(GetCurrentScene(),
        finalResult <= ScoreType.Good,
        perfectedEverything
        );
    }
    public Scenes GetCurrentScene()
    {
        return (Scenes)SceneManager.GetActiveScene().buildIndex;
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene((int)nextIndex);
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene((int)Scenes.MAIN_MENU);
    }
}
