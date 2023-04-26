using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new pokeball")]
public class PokeballItem : ItemBase
{
    [SerializeField] float catchRateModifier = 1;

    public override bool Use(Pokemon pokemon)
    {
        if (GameController.Instance.State == GameState.Battle)
            return true;

        return false;
    }

    public float CatchRateModifier => catchRateModifier;
}
