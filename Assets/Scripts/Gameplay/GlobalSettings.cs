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
    [SerializeField] BoxImageData[] boxes;

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
    public BoxImageData[] Boxes => boxes;

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

    public TypeBase GetMoveType(MoveBase move)
    {
        foreach (var item in Type)
        {
            if (item.Type == move.Type)
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

    public BoxImageData GetBoxImageData(BoxName name)
    {
        foreach (var item in Boxes)
        {
            if (item.BoxType == name)
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

[System.Serializable]
public class BoxImageData
{
    [SerializeField] BoxName boxType;
    [SerializeField] Sprite boxHeaderImage;
    [SerializeField] Sprite boxImage;

    public BoxName BoxType => boxType;
    public Sprite BoxHeader => boxHeaderImage;
    public Sprite BoxImage => boxImage;

    public string GetBoxNameString()
    {
        return BoxType.ToString();
    }
}

public enum BoxName
{
    None,
    Plain,
    City,
    Desert,
    Savannah,
    Mountain,
    Volcano,
    Tundra,
    Cave,
    Beach,
    Sea,
    River,
    Sky,
    Bubble,
    Poke,
    PC,
    Empty,
}