using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JPA_DialogueSystem.Utils
{
    [Serializable]
    public struct NullableObject<T>
    {
        public T value;
        public readonly bool IsNull
        {
            get => value == null;
        }
    }
}
