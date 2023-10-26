using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryStatUI : MonoBehaviour
{
    [SerializeField] GameObject statDetail;
    [SerializeField] StatDetailUI statDetailPrefab;

    [SerializeField] UIStatsRadarChart radarChart;

    List<StatDetailUI> statList;

    public void Init(Pokemon pokemon)
    {
        if (statList != null) statList.Clear();

        foreach (Transform child in statDetail.transform)
            Destroy(child.gameObject);

        statList = new List<StatDetailUI>();
        // HEALTH
        var stat1 = Instantiate(statDetailPrefab, statDetail.transform);
        stat1.SetData("hp", pokemon.MaxHP);
        statList.Add(stat1);

        // ATTACK
        var stat2 = Instantiate(statDetailPrefab, statDetail.transform);
        stat2.SetData("attack", pokemon.Stats[Stat.Attack]);
        statList.Add(stat2);

        // DEFENSE
        var stat3 = Instantiate(statDetailPrefab, statDetail.transform);
        stat3.SetData("defense", pokemon.Stats[Stat.Defense]);
        statList.Add(stat3);

        // SPECIAL ATTACK
        var stat4 = Instantiate(statDetailPrefab, statDetail.transform);
        stat4.SetData("special attack", pokemon.Stats[Stat.SpAttack]);
        statList.Add(stat4);

        // SPECIAL DEFENSE
        var stat5 = Instantiate(statDetailPrefab, statDetail.transform);
        stat5.SetData("special defense", pokemon.Stats[Stat.SpDefense]);
        statList.Add(stat5);

        // SPEED
        var stat6 = Instantiate(statDetailPrefab, statDetail.transform);
        stat6.SetData("speed", pokemon.Stats[Stat.Speed]);
        statList.Add(stat6);

        // display radar chart image
        radarChart.Setup(pokemon);
    }
}
