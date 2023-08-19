using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectAction : CutsceneAction
{
    [SerializeField] GameObject go;

    public override IEnumerator Play()
    {
        go.SetActive(true);
        yield break;
    }
}
