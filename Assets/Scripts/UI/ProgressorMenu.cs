using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressorMenu : MonoBehaviour
{
    [Serializable]
    public struct AchievementLevel
    {
        public Scenes level;
        public GameObject starAchievementReference;
        public GameObject crownAchievementReference;
        public GameObject lockObject;
        public Button levelButton;
    }
    [SerializeField]
    private AchievementLevel[] achievementLevels;
    void OnEnable()
    {
        RefreshAchievedVisuals();
    }
    public void RefreshAchievedVisuals()
    {
        ProgressionManager.LevelProgress? prevLevelStats = null;
        foreach (AchievementLevel achLevel in achievementLevels)
        {
            ProgressionManager.LevelProgress progressionStats = ProgressionManager.GetLevelProgress(achLevel.level);
            if (prevLevelStats.HasValue && achLevel.levelButton != null && achLevel.lockObject != null)
            {
                achLevel.levelButton.interactable = prevLevelStats.Value.isCompleted;
                achLevel.lockObject.SetActive(!prevLevelStats.Value.isCompleted);
            }
            achLevel.starAchievementReference.SetActive(progressionStats.isCompleted);
            achLevel.crownAchievementReference.SetActive(progressionStats.isPerfected);
            prevLevelStats = progressionStats;
        }
    }


}
