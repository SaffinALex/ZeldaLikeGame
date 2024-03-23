using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleBehavior : MonoBehaviour
{
    public abstract void InitializeModule();
    public abstract void UpdateModule();
}
