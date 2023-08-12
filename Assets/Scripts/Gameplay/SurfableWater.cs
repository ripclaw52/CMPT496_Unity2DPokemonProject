using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfableWater : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.Instance.ShowDialogText("The water is dyed a deep blue...");

        var pokemonWithSurf = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Surf"));

        if (pokemonWithSurf != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText("Would you like to SURF on it?",
                choices: new List<string>() { "YES", "NO" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                yield return DialogManager.Instance.ShowDialogText($"{pokemonWithSurf.Base.Name} used SURF!");

                var animator = initiator.GetComponent<CharacterAnimator>();
                var dir = new Vector3(animator.MoveX, animator.MoveY);
                var targetPos = initiator.position + dir;

                yield return initiator.DOJump(targetPos, 0.3f, 1, 0.5f).WaitForCompletion();
                animator.IsSurfing = true;
            }
        }
    }
}
