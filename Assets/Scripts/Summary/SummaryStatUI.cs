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
        stat1.SetData("hp", pokemon.MaxHP, pokemon.EV.HP, pokemon.IV.HP);
        statList.Add(stat1);

        // ATTACK
        var stat2 = Instantiate(statDetailPrefab, statDetail.transform);
        stat2.SetData("attack", pokemon.Stats[Stat.Attack], pokemon.EV.Attack, pokemon.IV.Attack);
        if (pokemon.Nature.Attack == 1.1f){ stat2.SetBackgroundImage(GlobalSettings.i.HighNatureStat); }
        else if (pokemon.Nature.Attack == 0.9f) { stat2.SetBackgroundImage(GlobalSettings.i.LowNatureStat); }
        else { stat2.SetBackgroundImage(GlobalSettings.i.NormalNatureStat); }
        statList.Add(stat2);

        // DEFENSE
        var stat3 = Instantiate(statDetailPrefab, statDetail.transform);
        stat3.SetData("defense", pokemon.Stats[Stat.Defense], pokemon.EV.Defense, pokemon.IV.Defense);
        if (pokemon.Nature.Defense == 1.1f) { stat3.SetBackgroundImage(GlobalSettings.i.HighNatureStat); }
        else if (pokemon.Nature.Defense == 0.9f) { stat3.SetBackgroundImage(GlobalSettings.i.LowNatureStat); }
        else { stat3.SetBackgroundImage(GlobalSettings.i.NormalNatureStat); }
        statList.Add(stat3);

        // SPECIAL ATTACK
        var stat4 = Instantiate(statDetailPrefab, statDetail.transform);
        stat4.SetData("special attack", pokemon.Stats[Stat.SpAttack], pokemon.EV.SpAttack, pokemon.IV.SpAttack);
        if (pokemon.Nature.SpAttack == 1.1f) { stat4.SetBackgroundImage(GlobalSettings.i.HighNatureStat); }
        else if (pokemon.Nature.SpAttack == 0.9f) { stat4.SetBackgroundImage(GlobalSettings.i.LowNatureStat); }
        else { stat4.SetBackgroundImage(GlobalSettings.i.NormalNatureStat); }
        statList.Add(stat4);

        // SPECIAL DEFENSE
        var stat5 = Instantiate(statDetailPrefab, statDetail.transform);
        stat5.SetData("special defense", pokemon.Stats[Stat.SpDefense], pokemon.EV.SpDefense, pokemon.IV.SpDefense);
        if (pokemon.Nature.SpDefense == 1.1f) { stat5.SetBackgroundImage(GlobalSettings.i.HighNatureStat); }
        else if (pokemon.Nature.SpDefense == 0.9f) { stat5.SetBackgroundImage(GlobalSettings.i.LowNatureStat); }
        else { stat5.SetBackgroundImage(GlobalSettings.i.NormalNatureStat); }
        statList.Add(stat5);

        // SPEED
        var stat6 = Instantiate(statDetailPrefab, statDetail.transform);
        stat6.SetData("speed", pokemon.Stats[Stat.Speed], pokemon.EV.Speed, pokemon.IV.Speed);
        if (pokemon.Nature.Speed == 1.1f) { stat6.SetBackgroundImage(GlobalSettings.i.HighNatureStat); }
        else if (pokemon.Nature.Speed == 0.9f) { stat6.SetBackgroundImage(GlobalSettings.i.LowNatureStat); }
        else { stat6.SetBackgroundImage(GlobalSettings.i.NormalNatureStat); }
        statList.Add(stat6);

        // display radar chart image
        radarChart.Setup(pokemon);
    }
}
