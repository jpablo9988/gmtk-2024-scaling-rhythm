using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PulseTransparency : IPausable
{
    [Header("Settings")]
    [SerializeField]
    private ReferenceType typeOfReference;
    [Range(0f, 1f)]
    public float maxAlphaValue = 1.0f;
    [Range(0f, 1f)]
    public float minAlphaValue = 1.0f;
    public float timeBetweenPulses = 2.0f;
    public enum ReferenceType
    {
        NONE,
        TEXT,
        SPRITE,
        UI_IMAGE
    }
    public ReferenceType TypeOfReference
    {
        get
        {
            return typeOfReference;
        }
        set
        {
            typeOfReference = value;
            GetDependencies(typeOfReference);
        }
    }
    //DEPENDENCIES
    private TextMeshProUGUI textDependecy;
    private SpriteRenderer spriteDependency;
    private UnityEngine.UI.Image imageDependency;

    protected override void OnEnable()
    {
        base.OnEnable();
        GetDependencies(typeOfReference);
    }
    private void GetDependencies(ReferenceType isText)
    {
        switch (isText)
        {
            case ReferenceType.TEXT:
                textDependecy = FetchDependency<TextMeshProUGUI>();
                return;
            case ReferenceType.SPRITE:
                spriteDependency = FetchDependency<SpriteRenderer>();
                return;
            case ReferenceType.UI_IMAGE:
                imageDependency = FetchDependency<UnityEngine.UI.Image>();
                return;
        }
    }
    private T FetchDependency<T>()
    {
        T auxObject = GetComponent<T>();
        if (auxObject == null)
        {
            Debug.LogError("Dependency missing from gameObject in Pulse Transparency");
            typeOfReference = ReferenceType.NONE;
        }
        return auxObject;
    }
    private float currTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (isGamePaused) return;
        if (timeBetweenPulses <= 0.0f)
        {
            Debug.LogError("Time is negative on PulseTransparency");
            return;
        }
        currTime += Time.deltaTime / timeBetweenPulses;
        ChangeAlpha(typeOfReference, Mathf.Lerp(minAlphaValue, maxAlphaValue, Mathf.Sin(currTime % timeBetweenPulses * 180 * Mathf.Deg2Rad)));
    }
    private void ChangeAlpha(ReferenceType referenceType, float value)
    {
        switch (referenceType)
        {
            case ReferenceType.TEXT:
                Assert.IsNotNull(textDependecy);
                textDependecy.alpha = value;
                return;
            case ReferenceType.SPRITE:
                Assert.IsNotNull(spriteDependency);
                Color spriteColor = spriteDependency.color;
                spriteColor.a = value;
                spriteDependency.color = spriteColor;
                return;
            case ReferenceType.UI_IMAGE:
                Assert.IsNotNull(imageDependency);
                Color imageColor = imageDependency.color;
                imageColor.a = value;
                imageDependency.color = imageColor;
                return;
        }
    }
}
