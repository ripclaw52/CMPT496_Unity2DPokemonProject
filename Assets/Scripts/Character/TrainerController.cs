using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a MonoBehaviour that implements the Interactable and ISavable interfaces. It is used to control the Trainer game object.
/// </summary>
public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    [SerializeField] AudioClip trainerAppearsClip;

    // State
    bool battleLost = false;

    Character character;

    /// <summary>
    /// Gets the Character component on Awake. 
    /// </summary>
    private void Awake()
    {
        character = GetComponent<Character>();
    }

    /// <summary>
    /// Sets the field of view rotation to the character's default direction.
    /// </summary>
    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    /// <summary>
    /// Updates the character.
    /// </summary>
    private void Update()
    {
        character.HandleUpdate();
    }

    /// <summary>
    /// Interacts with the trainer, either starting a battle or displaying a dialog after a battle has been lost.
    /// </summary>
    /// <param name="initiator">The transform of the initiator of the interaction.</param>
    /// <returns>An IEnumerator for the interaction.</returns>
    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!battleLost)
        {
            AudioManager.i.PlayMusic(trainerAppearsClip);

            yield return DialogManager.Instance.ShowDialog(dialog);
            GameController.Instance.StartTrainerBattle(this);
        }
        else
        {
            yield return DialogManager.Instance.ShowDialog(dialogAfterBattle);
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        GameController.Instance.StateMachine.Push(CutsceneState.i);

        AudioManager.i.PlayMusic(trainerAppearsClip);

        // Show Exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // Walk towards the player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // Show dialog
        yield return DialogManager.Instance.ShowDialog(dialog);

        GameController.Instance.StateMachine.Pop();

        GameController.Instance.StartTrainerBattle(this);
    }

    /// <summary>
    /// Sets the battleLost boolean to true and deactivates the fov gameObject.
    /// </summary>
    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the field of view rotation based on the given direction.
    /// </summary>
    /// <param name="dir">The direction to set the field of view rotation to.</param>
    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    /// <summary>
    /// Captures the state of the battle and returns it.
    /// </summary>
    /// <returns>The state of the battle.</returns>
    public object CaptureState()
    {
        return battleLost;
    }

    /// <summary>
    /// Restores the state of the game based on the given state object. 
    /// If the battle was lost, the field of view is disabled.
    /// </summary>
    /// <param name="state">The state object to restore the game from.</param>
    public void RestoreState(object state)
    {
        battleLost = (bool)state;

        if (battleLost)
            fov.gameObject.SetActive(false);
    }

    public string Name => name;
    public Sprite Sprite => sprite;
}
