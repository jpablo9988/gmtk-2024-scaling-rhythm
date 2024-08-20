using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private List<string> resultsQuotes;
    [SerializeField] private List<Sprite> images;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private int nextIndex;
    public void ComputeResults(RhythmTrack track)
    {
        if (ScoreTally.Meh == 0 && (ScoreTally.Perfect > ScoreTally.Good))
        {
            text.text = resultsQuotes[0];
            image.sprite = images[0];
        }
        else if (ScoreTally.Meh <= 30 && (ScoreTally.Good + ScoreTally.Perfect) > ScoreTally.Meh)
        {
            text.text = resultsQuotes[1];
            image.sprite = images[1];
        }
        else if (ScoreTally.TotalScore >= (track.Map.BeatList.Count / 2))
        {
            text.text = resultsQuotes[2];
            image.sprite = images[2];
        }
        else
        {
            text.text = resultsQuotes[3];
            image.sprite = images[3];
        }
        panel.SetActive(true);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(nextIndex);
    }
}
