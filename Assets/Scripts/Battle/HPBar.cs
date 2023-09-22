using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject healthTop;
    [SerializeField] GameObject healthBottom;

    public bool IsUpdating { get; private set; }

    public void SetHP(float hpNormalized)
    {
        healthTop.transform.localScale = new Vector3(hpNormalized, 1f);
        healthBottom.transform.localScale = new Vector3(hpNormalized, 1f);

        healthTop.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientTop.Evaluate(hpNormalized);
        healthBottom.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientBottom.Evaluate(hpNormalized);
    }

    public IEnumerator SetHPSmooth(float newHp)
    {
        IsUpdating = true;

        float curHp = healthTop.transform.localScale.x;
        float changeAmt = curHp - newHp;

        while (curHp - newHp > Mathf.Epsilon)
        {
            curHp -= changeAmt * Time.deltaTime;
            healthTop.transform.localScale = new Vector3(curHp, 1f);
            healthBottom.transform.localScale = new Vector3(curHp, 1f);
            healthTop.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientTop.Evaluate(curHp);
            healthBottom.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientBottom.Evaluate(curHp);
            yield return null;
        }
        healthTop.transform.localScale = new Vector3(newHp, 1f);
        healthBottom.transform.localScale = new Vector3(newHp, 1f);
        healthTop.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientTop.Evaluate(newHp);
        healthBottom.GetComponent<Image>().color = GlobalSettings.i.HealthbarGradientBottom.Evaluate(newHp);

        IsUpdating = false;
    }
}
