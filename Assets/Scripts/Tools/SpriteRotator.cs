using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : IPausable
{
    [Header("Attributes")]
    [SerializeField]
    float rotationSpeed;
    void Update()
    {
        if (isGamePaused) return;
        this.transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
    }
}
