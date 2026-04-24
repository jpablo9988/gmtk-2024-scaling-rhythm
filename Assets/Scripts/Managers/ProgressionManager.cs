using System;
using System.Diagnostics;
using System.Linq;

public static class ProgressionManager
{
    [Serializable]
    public struct LevelProgress
    {
        public Scenes level;
        public bool isCompleted;
        public bool isPerfected;
    }
    //Update on new levels
    private static LevelProgress[] levelProgresses =
    {
        new() {
            level = Scenes.MICROBE_MUNCHIN,
            isCompleted = false,
            isPerfected = false
        },
        new() {
            level = Scenes.ROCK_TOSSER,
            isCompleted = false,
            isPerfected = false
        }
    };
    public static void ChangeLevelProgress(Scenes scene, bool isCompleted, bool isPerfected)
    {
        for (int i = 0; i < levelProgresses.Length; i++)
        {
            if (scene == levelProgresses[i].level)
            {
                levelProgresses[i].isCompleted = isCompleted;
                levelProgresses[i].isPerfected = isPerfected;
                break;
            }
        }
    }
    public static LevelProgress GetLevelProgress(Scenes scene)
    {
        return levelProgresses.FirstOrDefault(i => i.level == scene);
    }
}
