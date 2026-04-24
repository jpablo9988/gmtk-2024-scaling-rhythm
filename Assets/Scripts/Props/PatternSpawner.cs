using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pattern;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Conductor conductor;
    [Tooltip("Sprite Information")]
    [SerializeField] private Sprite[] tiny;
    [SerializeField] private Sprite[] small;
    [SerializeField] private Sprite[] medium;
    [SerializeField] private Sprite[] large;
    [SerializeField] private Sprite[] huge;
    [SerializeField] private int[] ranges;
    [SerializeField] private CinemachineSmoothPath pathUntilHit;
    [SerializeField] private CinemachineSmoothPath pathFail;
    private float currentIndexOnRange = 0;

    public delegate void ReachedNewRange();
    public static event ReachedNewRange OnReachedNewRange;
    [SerializeField]
    private List<PatternScript> patternsInScene = new();
    private bool isDestroyingObjects = false;
    void OnEnable()
    {
        PatternManager.beatTelegraph += SpawnPattern;
        PatternScript.OnPatternDestroy += RemovePatternFromList;
    }
    private void OnDisable()
    {
        PatternManager.beatTelegraph -= SpawnPattern;
        PatternScript.OnPatternDestroy -= RemovePatternFromList;
    }
    private void RemovePatternFromList(PatternScript toRemove)
    {
        if (isDestroyingObjects || patternsInScene.Count <= 0) return;
        if (!patternsInScene.Contains(toRemove)) return;
        patternsInScene.Remove(toRemove);
    }
    public void RemoveAllPatternsFromScene(bool willFade)
    {
        //BUG HERE: If triggered on same frame as RemovePatternFromList -> NullPointerException.
        isDestroyingObjects = true;
        foreach (PatternScript pattern in patternsInScene)
        {
            pattern.DestroyPatternObject(willFade);
        }
        patternsInScene.Clear();
        isDestroyingObjects = false;
    }
    public void SpawnPattern(PatternManager.TelegraphPackage pck)
    {
        if (isDestroyingObjects) return;
        GameObject newPattern = Instantiate(pattern, spawnPoint.transform.position, spawnPoint.transform.rotation);
        PatternScript patterAnim = newPattern.GetComponentInChildren<PatternScript>();
        List<Sprite[]> spriteList = new() { tiny, small, medium, large, huge };
        bool isHalfNote = false;
        patterAnim.pathUntilPerfectHit = pathUntilHit;
        patterAnim.pathFail = pathFail;
        int spriteToAddIndex = 0;
        if (ranges.Length < 4)
        {
            Debug.LogWarning("Ranges are not complete. Destroying object. ");
            patterAnim.DestroyPatternObject(false);
        }
        else
        {
            foreach (int range in ranges)
            {
                if (ScoreTally.SessionScore > range)
                {
                    spriteToAddIndex++;
                }
                else
                {
                    break;
                }
            }
            if (currentIndexOnRange != spriteToAddIndex)
            {
                OnReachedNewRange?.Invoke();
            }
            isHalfNote = IsPatternHalfNote(pck);
            currentIndexOnRange = spriteToAddIndex;
            patternsInScene.Add(patterAnim);
            patterAnim.InitiatePattern(pck.beatsUntilHit, conductor, spriteList[spriteToAddIndex], isHalfNote);
        }
    }
    private bool IsPatternHalfNote(PatternManager.TelegraphPackage pck)
    {
        if (pck.distanceToNext != null)
        {
            if (pck.distanceToNext == 0.5f) return true;
        }
        if (pck.distanceToPrevious != null)
        {
            if (pck.distanceToPrevious == 0.5f) return true;
        }
        return false;
    }
}
