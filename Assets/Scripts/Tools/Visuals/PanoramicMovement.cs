using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PanoramicMovement : MonoBehaviour
{
    public enum PanoramaType
    {
        FRONT,
        FRONTMIDDLE,
        MIDDLE,
        MIDDLEDISTANT,
        DISTANT,
        FRONTISSIMO
    }
    private PanoramicObj[] objs;
    public float movementTime = 0.2f;
    public int moveStarter = 4;
    public int bigMoveStarter = 8;
    public static readonly Dictionary<PanoramaType, float> panoramaSpeedValues = new()
    {
      { PanoramaType.FRONT, -6f},
      { PanoramaType.FRONTMIDDLE, -4f},
      { PanoramaType.MIDDLE, -2.5f},
      { PanoramaType.MIDDLEDISTANT, 2f},
      { PanoramaType.DISTANT, 3f },
      { PanoramaType.FRONTISSIMO, -8f}
    };
    void OnEnable()
    {
        ScoreObserver.OnGainScore += MoveObjects;
    }
    void Start()
    {
        objs = GetComponentsInChildren<PanoramicObj>();
    }

    // Update is called once per frame
    public void MoveObjects(ScoreType type)
    {
        if (!ScoreTally.IsTrackingScore) return;
        if (ScoreTally.TotalScore >= moveStarter && (int)type <= (int)ScoreType.Good)
        {
            foreach (PanoramicObj obj in objs)
            {
                if (obj.panoramaType == PanoramaType.DISTANT && ScoreTally.TotalScore >= bigMoveStarter)
                {
                    obj.MoveTowards(movementTime);
                }
                else if (obj.panoramaType != PanoramaType.DISTANT)
                {
                    obj.MoveTowards(movementTime);
                }
            }
        }
    }
}
