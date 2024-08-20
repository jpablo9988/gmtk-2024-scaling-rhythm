using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pattern;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Conductor conductor;
    [Tooltip ("Sprite Information")]
    [SerializeField] private Sprite[] tiny;
    [SerializeField] private Sprite[] small;
    [SerializeField] private Sprite[] medium;
    [SerializeField] private Sprite[] large;
    [SerializeField] private Sprite[] huge;
    [SerializeField] private int[] ranges;
    void OnEnable()
    {
        PatternManager.beatTelegraph += SpawnPattern;
    }
    private void OnDisable()
    {
        PatternManager.beatTelegraph -= SpawnPattern;
    }
    public void SpawnPattern(float beatType)
    {
        GameObject newPattern = Instantiate(pattern, spawnPoint.transform.position, spawnPoint.transform.rotation);
        PatternScript patterAnim = newPattern.GetComponentInChildren<PatternScript>();
        List<Sprite[]> spriteList = new() { tiny, small, medium, large, huge };
        int spriteToAddIndex = 0;
        if (ranges.Length < 4)
        {
            Debug.LogWarning("Ranges are not complete");
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
            patterAnim.InitiatePattern(beatType, conductor, spriteList[spriteToAddIndex]);
        }
    }
}
