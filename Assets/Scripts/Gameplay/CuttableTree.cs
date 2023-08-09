using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.Instance.ShowDialogText("This tree looks like it can be CUT down!");

        var pokemonWithCut = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));

        if (pokemonWithCut != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText("Would you like to CUT it?",
                choices: new List<string>() { "YES", "NO" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                yield return DialogManager.Instance.ShowDialogText($"{pokemonWithCut.Base.Name} used CUT!");
                // Yes
                // play animation while cutting tree?
                gameObject.SetActive(false);
            }
        }
    }
}
