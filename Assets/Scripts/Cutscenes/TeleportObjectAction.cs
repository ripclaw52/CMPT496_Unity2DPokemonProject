using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObjectAction : CutsceneAction
{
    [SerializeField] GameObject go;
    [SerializeField] Vector2 position;

    public override IEnumerator Play()
    {
        go.transform.position = position;
        yield break;
    }
}
