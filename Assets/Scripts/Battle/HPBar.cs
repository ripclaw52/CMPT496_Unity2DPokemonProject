using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to manage the HP bar of the Pokemon.
/// </summary>
public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public bool IsUpdating { get; private set; }

    /// <summary>
    /// Sets the health of the object by transforming the local scale of the health object.
    /// </summary>
    /// <param name="hpNormalized">The normalized health value to set.</param>
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    /// <summary>
    /// Smoothly updates the health bar to the new health value.
    /// </summary>
    /// <param name="newHp">The new health value.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    public IEnumerator SetHPSmooth(float newHp)
    {
        IsUpdating = true;

        float curHp = health.transform.localScale.x;
        float changeAmt = curHp - newHp;

        while (curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHp, 1f);

        IsUpdating = false;
    }
}
