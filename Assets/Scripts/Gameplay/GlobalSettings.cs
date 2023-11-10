using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettings : MonoBehaviour
{
    [SerializeField] Color highlightedColor;
    [SerializeField] Gradient healthbarGradientTop;
    [SerializeField] Gradient healthbarGradientBottom;

    [SerializeField] Color normalNatureStat;
    [SerializeField] Color lowNatureStat;
    [SerializeField] Color highNatureStat;

    [SerializeField] CategoryBase[] category;
    [SerializeField] StatusBase[] status;
    [SerializeField] TypeBase[] type;

    [SerializeField] int maximumPokemonLevel = 100;

    public Color HighlightedColor => highlightedColor;
    public Gradient HealthbarGradientTop => healthbarGradientTop;
    public Gradient HealthbarGradientBottom => healthbarGradientBottom;

    public Color NormalNatureStat => normalNatureStat;
    public Color LowNatureStat => lowNatureStat;
    public Color HighNatureStat => highNatureStat;

    public CategoryBase[] Category => category;
    public StatusBase[] Status => status;
    public TypeBase[] Type => type;

    public int MaximumPokemonLevel => maximumPokemonLevel;

    public static GlobalSettings i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    public TypeBase GetPokemonType(PokemonType type)
    {
        foreach (var item in GlobalSettings.i.Type)
        {
            if (type == item.Type)
                return item;
        }
        return null;
    }

    public CategoryBase GetCategory(MoveBase move)
    {
        foreach (var item in Category)
        {
            if (item.Category == move.Category)
                return item;
        }
        return null;
    }

    public StatusBase GetStatusCondition(ConditionID condition)
    {
        foreach (var item in Status)
        {
            if (item.Condition == condition)
                return item;
        }
        return null;
    }

    public TypeBase GetMoveType(MoveBase move)
    {
        foreach (var item in Type)
        {
            if (item.Type == move.Type)
                return item;
        }
        return null;
    }
}

[System.Serializable]
public class CategoryBase
{
    [SerializeField] MoveCategory category;
    [SerializeField] Sprite categoryIcon;
    public MoveCategory Category => category;
    public Sprite CategoryIcon => categoryIcon;
}

[System.Serializable]
public class StatusBase
{
    [SerializeField] ConditionID condition;
    [SerializeField] Sprite conditionIcon;
    [SerializeField] Color conditionColor;
    public ConditionID Condition => condition;
    public Sprite ConditionIcon => conditionIcon;
    public Color ConditionColor => conditionColor;
}

[System.Serializable]
public class TypeBase
{
    [SerializeField] PokemonType type;
    [SerializeField] Sprite typeIcon;
    [SerializeField] Color typeColor;
    public PokemonType Type => type;
    public Sprite TypeIcon => typeIcon;
    public Color TypeColor => typeColor;
}
