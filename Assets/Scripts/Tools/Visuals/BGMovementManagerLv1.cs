using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BGMovementManagerLv1 : IPausable
{
    [SerializeField]
    private SpriteRenderer[] tiles;
    BGColumnMovementsLv1[] columnManagers;
    Transform trailingPosition;
    float horizontalSpriteSize;
    float screenBoundPosition;
    [Header("Dependencies")]
    [SerializeField]
    private Conductor conductor;
    [Header("Sprite Tiles")]
    [SerializeField]
    List<Sprite> levelProgressionSprites;
    [Header("Movement Attributes")]
    [SerializeField]
    float movementSpeed;
    int levelProgressionSpritesIndex;
    protected override void OnEnable()
    {
        base.OnEnable();
        levelProgressionSpritesIndex = 0;
        tiles = GetComponentsInChildren<SpriteRenderer>();
        columnManagers = GetComponentsInChildren<BGColumnMovementsLv1>();
        //Size of sprite times the scale in x.
        horizontalSpriteSize = tiles[0].sprite.bounds.max.x * tiles[0].transform.localScale.x;
        Debug.Log(horizontalSpriteSize);
        screenBoundPosition = SetInitialPosition(horizontalSpriteSize);
        FindInitialTrailingPosition(columnManagers, screenBoundPosition);
        PatternSpawner.OnReachedNewRange += StartRotationAnimationOnTiles;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (BGColumnMovementsLv1 manager in columnManagers)
        {
            manager.onReachBorder.RemoveAllListeners();
        }
        PatternSpawner.OnReachedNewRange -= StartRotationAnimationOnTiles;
    }
    void Update()
    {
        if (isGamePaused) return;
        this.transform.position = new Vector3(this.transform.position.x - movementSpeed * Time.deltaTime,
        transform.position.y,
        0);

    }
    float SetInitialPosition(float spriteSize)
    {
        Vector2 borderScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
        this.transform.position = new Vector3(borderScreenPosition.x - (horizontalSpriteSize * 2),
        this.transform.position.y, 0);
        return -borderScreenPosition.x - (spriteSize * 2);
    }
    void FindInitialTrailingPosition(BGColumnMovementsLv1[] columnManagers, float screenBoundPosition)
    {
        float lesserPosition = float.MinValue;
        foreach (BGColumnMovementsLv1 manager in columnManagers)
        {
            if (manager.gameObject.transform.localPosition.x > lesserPosition)
            {
                trailingPosition = manager.gameObject.transform;
                lesserPosition = manager.gameObject.transform.localPosition.x;
            }
            manager.ScreenBorder = screenBoundPosition;
            manager.onReachBorder.AddListener(OnReachedBorder);
        }
    }
    private void OnReachedBorder(Transform newTrailingTransform, BGColumnMovementsLv1 reference)
    {
        newTrailingTransform.position = new Vector3(trailingPosition.position.x + (horizontalSpriteSize * 2), newTrailingTransform.position.y, 0);
        trailingPosition = newTrailingTransform;
    }
    private void StartRotationAnimationOnTiles()
    {
        if (!conductor) return;
        if (levelProgressionSpritesIndex >= levelProgressionSprites.Count)
        {
            Debug.LogError("Sprites Index in the Background Movement Manager is going out of bounds...");
            return;
        }
        foreach (BGColumnMovementsLv1 manager in columnManagers)
        {
            manager.TransitionToSprite(conductor, levelProgressionSprites[levelProgressionSpritesIndex], 0);
        }
        levelProgressionSpritesIndex++;

    }
}
