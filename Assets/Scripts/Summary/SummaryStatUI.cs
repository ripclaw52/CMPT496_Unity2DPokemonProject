using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummaryStatUI : MonoBehaviour
{
    [SerializeField] GameObject statDetail;
    [SerializeField] StatDetailUI statDetailPrefab;

    [SerializeField] UIStatsRadarChart radarChart;

    [Header("Radar Graph Text")]
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI defText;
    [SerializeField] TextMeshProUGUI spdText;
    [SerializeField] TextMeshProUGUI spDefText;
    [SerializeField] TextMeshProUGUI spAtkText;

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
        hpText.color = GlobalSettings.i.NormalNatureStat;
        statList.Add(stat1);

        // ATTACK
        var stat2 = Instantiate(statDetailPrefab, statDetail.transform);
        stat2.SetData("attack", pokemon.Stats[Stat.Attack], pokemon.EV.Attack, pokemon.IV.Attack);
        if (pokemon.Nature.Attack == 1.1f)
        {
            stat2.SetBackgroundImage(GlobalSettings.i.HighNatureStat);
            atkText.color = GlobalSettings.i.HighNatureStat;
            atkText.text = $"ATK \u2191";
        }
        else if (pokemon.Nature.Attack == 0.9f)
        {
            stat2.SetBackgroundImage(GlobalSettings.i.LowNatureStat);
            atkText.color = GlobalSettings.i.LowNatureStat;
            atkText.text = $"ATK \u2193";
        }
        else
        {
            stat2.SetBackgroundImage(GlobalSettings.i.NormalNatureStat);
            atkText.color = GlobalSettings.i.NormalNatureStat;
            atkText.text = $"ATK";
        }
        statList.Add(stat2);

        // DEFENSE
        var stat3 = Instantiate(statDetailPrefab, statDetail.transform);
        stat3.SetData("defense", pokemon.Stats[Stat.Defense], pokemon.EV.Defense, pokemon.IV.Defense);
        if (pokemon.Nature.Defense == 1.1f)
        {
            stat3.SetBackgroundImage(GlobalSettings.i.HighNatureStat);
            defText.color = GlobalSettings.i.HighNatureStat;
            defText.text = $"DEF \u2191";
        }
        else if (pokemon.Nature.Defense == 0.9f)
        {
            stat3.SetBackgroundImage(GlobalSettings.i.LowNatureStat);
            defText.color = GlobalSettings.i.LowNatureStat;
            defText.text = $"DEF \u2193";
        }
        else
        {
            stat3.SetBackgroundImage(GlobalSettings.i.NormalNatureStat);
            defText.color = GlobalSettings.i.NormalNatureStat;
            defText.text = $"DEF";
        }
        statList.Add(stat3);

        // SPECIAL ATTACK
        var stat4 = Instantiate(statDetailPrefab, statDetail.transform);
        stat4.SetData("special attack", pokemon.Stats[Stat.SpAttack], pokemon.EV.SpAttack, pokemon.IV.SpAttack);
        if (pokemon.Nature.SpAttack == 1.1f)
        {
            stat4.SetBackgroundImage(GlobalSettings.i.HighNatureStat);
            spAtkText.color = GlobalSettings.i.HighNatureStat;
            spAtkText.text = $"SP.ATK \u2191";
        }
        else if (pokemon.Nature.SpAttack == 0.9f)
        {
            stat4.SetBackgroundImage(GlobalSettings.i.LowNatureStat);
            spAtkText.color = GlobalSettings.i.LowNatureStat;
            spAtkText.text = $"SP.ATK \u2193";
        }
        else
        {
            stat4.SetBackgroundImage(GlobalSettings.i.NormalNatureStat);
            spAtkText.color = GlobalSettings.i.NormalNatureStat;
            spAtkText.text = $"SP.ATK";
        }
        statList.Add(stat4);

        // SPECIAL DEFENSE
        var stat5 = Instantiate(statDetailPrefab, statDetail.transform);
        stat5.SetData("special defense", pokemon.Stats[Stat.SpDefense], pokemon.EV.SpDefense, pokemon.IV.SpDefense);
        if (pokemon.Nature.SpDefense == 1.1f)
        {
            stat5.SetBackgroundImage(GlobalSettings.i.HighNatureStat);
            spDefText.color = GlobalSettings.i.HighNatureStat;
            spDefText.text = $"SP.DEF \u2191";
        }
        else if (pokemon.Nature.SpDefense == 0.9f)
        {
            stat5.SetBackgroundImage(GlobalSettings.i.LowNatureStat);
            spDefText.color = GlobalSettings.i.LowNatureStat;
            spDefText.text = $"SP.DEF \u2193";
        }
        else
        {
            stat5.SetBackgroundImage(GlobalSettings.i.NormalNatureStat);
            spDefText.color = GlobalSettings.i.NormalNatureStat;
            spDefText.text = $"SP.DEF";
        }
        statList.Add(stat5);

        // SPEED
        var stat6 = Instantiate(statDetailPrefab, statDetail.transform);
        stat6.SetData("speed", pokemon.Stats[Stat.Speed], pokemon.EV.Speed, pokemon.IV.Speed);
        if (pokemon.Nature.Speed == 1.1f)
        {
            stat6.SetBackgroundImage(GlobalSettings.i.HighNatureStat);
            spdText.color = GlobalSettings.i.HighNatureStat;
            spdText.text = $"SPD \u2191";
        }
        else if (pokemon.Nature.Speed == 0.9f)
        {
            stat6.SetBackgroundImage(GlobalSettings.i.LowNatureStat);
            spdText.color = GlobalSettings.i.LowNatureStat;
            spdText.text = $"SPD \u2193";
        }
        else
        {
            stat6.SetBackgroundImage(GlobalSettings.i.NormalNatureStat);
            spdText.color = GlobalSettings.i.NormalNatureStat;
            spdText.text = $"SPD";
        }
        statList.Add(stat6);

        // display radar chart image
        radarChart.Setup(pokemon);
    }
}
