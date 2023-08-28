using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enum representing the different states of a battle.
/// </summary>
public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, Bag, PartyScreen, AboutToUse, MoveToForget, BattleOver }

/// <summary>
/// Enum representing the possible actions a player can take in a battle.
/// </summary>
public enum BattleAction { Move, SwitchPokemon, UseItem, Run }

public enum BattleTrigger { LongGrass, Water }

/// <summary>
/// This class is responsible for managing the battle system in the game.
/// </summary>
public class BattleSystem : MonoBehaviour
{
    /// <summary>
    /// This class contains references to the various UI elements used in the battle scene.
    /// </summary>
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;
    [SerializeField] GameObject pokeballSprite;
    [SerializeField] MoveSelectionUI moveSelectionUI;
    [SerializeField] InventoryUI inventoryUI;

    [Header("Audio")]
    [SerializeField] AudioClip wildBattleMusic;
    [SerializeField] AudioClip wildBattleVictoryMusic;

    [SerializeField] AudioClip trainerBattleMusic;
    [SerializeField] AudioClip trainerBattleVictoryMusic;

    [Header("Background Images")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite grassBackground;
    [SerializeField] Sprite waterBackground;

    /// <summary>
    /// Represents the state of a battle between two parties, either a player and a wild pokemon or a player and a trainer.
    /// </summary>
    public event Action<bool> OnBattleOver;

    BattleState state;

    int currentAction;
    int currentMove;
    bool aboutToUseChoice = true;

    PokemonParty playerParty;
    PokemonParty trainerParty;
    Pokemon wildPokemon;

    bool isTrainerBattle = false;
    PlayerController player;
    TrainerController trainer;

    int escapeAttempts;
    MoveBase moveToLearn;

    BattleTrigger battleTrigger;

    /// <summary>
    /// Starts a battle between the player's party and a wild Pokemon. 
    /// </summary>
    /// <param name="playerParty">The player's party of Pokemon.</param>
    /// <param name="wildPokemon">The wild Pokemon.</param>
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon, BattleTrigger trigger = BattleTrigger.LongGrass)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        player = playerParty.GetComponent<PlayerController>();
        isTrainerBattle = false;

        battleTrigger = trigger;

        AudioManager.i.PlayMusic(wildBattleMusic);

        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// Starts a trainer battle between the player and the trainer.
    /// </summary>
    /// <param name="playerParty">The player's party.</param>
    /// <param name="trainerParty">The trainer's party.</param>
    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty, BattleTrigger trigger = BattleTrigger.LongGrass)
    {
        this.playerParty = playerParty;
        this.trainerParty = trainerParty;

        isTrainerBattle = true;
        player = playerParty.GetComponent<PlayerController>();
        trainer = trainerParty.GetComponent<TrainerController>();

        battleTrigger = trigger;

        AudioManager.i.PlayMusic(trainerBattleMusic);

        StartCoroutine(SetupBattle());
    }

    /// <summary>
    /// Sets up a battle between the player and either a wild Pokemon or a trainer.
    /// </summary>
    /// <returns>
    /// An IEnumerator that sets up the battle.
    /// </returns>
    public IEnumerator SetupBattle()
    {
        playerUnit.Clear();
        enemyUnit.Clear();

        // changing background for pokemon battles // so far only deals with longgrass & water
        backgroundImage.sprite = (battleTrigger == BattleTrigger.LongGrass) ? grassBackground : waterBackground;

        if (!isTrainerBattle)
        {
            // Wild Pokemon Battle
            playerUnit.Setup(playerParty.GetHealthyPokemon());
            enemyUnit.Setup(wildPokemon);

            dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
            yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        }
        else
        {
            // Trainer Battle

            // Show trainer and player sprites
            playerUnit.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(false);

            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);
            playerImage.sprite = player.Sprite;
            trainerImage.sprite = trainer.Sprite;

            yield return dialogBox.TypeDialog($"{trainer.Name} wants to battle!");

            // Send out first pokemon of the trainer
            trainerImage.gameObject.SetActive(false);
            enemyUnit.gameObject.SetActive(true);
            var enemyPokemon = trainerParty.GetHealthyPokemon();
            enemyUnit.Setup(enemyPokemon);
            yield return dialogBox.TypeDialog($"{trainer.Name} sent out {enemyPokemon.Base.Name}!");

            // Send out first pokemon of the player
            playerImage.gameObject.SetActive(false);
            playerUnit.gameObject.SetActive(true);
            var playerPokemon = playerParty.GetHealthyPokemon();
            playerUnit.Setup(playerPokemon);
            yield return dialogBox.TypeDialog($"Go {playerPokemon.Base.Name}!");
            dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        }

        escapeAttempts = 0;
        partyScreen.Init();
        ActionSelection();
    }

    /// <summary>
    /// Sets the battle state to BattleOver, calls OnBattleOver for each pokemon in the player's party, and clears the HUD data for both the player and enemy units. 
    /// </summary>
    /// <param name="won">A boolean indicating whether the player won the battle.</param>
    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        playerUnit.Hud.ClearData();
        enemyUnit.Hud.ClearData();
        OnBattleOver(won);
    }

    /// <summary>
    /// Sets the battle state to ActionSelection and displays a dialog box prompting the user to choose an action. Enables the action selector.
    /// </summary>
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action!");
        dialogBox.EnableActionSelector(true);
    }

    /// <summary>
    /// Opens the bag and sets the battle state to Bag. Also activates the inventory UI. 
    /// </summary>
    void OpenBag()
    {
        state = BattleState.Bag;
        inventoryUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// Opens the Party Screen and sets the current state to PartyScreen. 
    /// </summary>
    void OpenPartyScreen()
    {
        // partyScreen.CalledFrom = state;
        state = BattleState.PartyScreen;
        partyScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets the battle state to MoveSelection, disables the action selector, dialog text, and enables the move selector.
    /// </summary>
    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    /// <summary>
    /// Enables the dialog box and sets the battle state to busy when a new pokemon is about to be used.
    /// </summary>
    /// <param name="newPokemon">The new pokemon about to be used.</param>
    /// <returns>An IEnumerator for the dialog box.</returns>
    IEnumerator AboutToUse(Pokemon newPokemon)
    {
        state = BattleState.Busy;
        yield return dialogBox.TypeDialog($"{trainer.Name} is about to send out {newPokemon.Base.Name}. Do you want to change pokemon?");

        state = BattleState.AboutToUse;
        dialogBox.EnableChoiceBox(true);
    }

    /// <summary>
    /// Enables the move selection UI and sets the move data for the Pokemon to choose a move to forget.
    /// </summary>
    /// <param name="pokemon">The Pokemon to choose a move to forget.</param>
    /// <param name="newMove">The new move to learn.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator ChooseMoveToForget(Pokemon pokemon, MoveBase newMove)
    {
        state = BattleState.Busy;
        yield return dialogBox.TypeDialog($"Choose a move you want to forget!");
        moveSelectionUI.gameObject.SetActive(true);
        moveSelectionUI.SetMoveData(pokemon.Moves.Select(x => x.Base).ToList(), newMove);
        moveToLearn = newMove;

        state = BattleState.MoveToForget;
    }

    /// <summary>
    /// Runs the turns of a battle, alternating between the player and enemy units.
    /// </summary>
    /// <param name="playerAction">The action the player has chosen to take.</param>
    /// <returns>An IEnumerator that runs the turns of the battle.</returns>
    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.Moves[currentMove];
            enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.Pokemon.CurrentMove.Base.Priority;
            int enemyMovePriority = enemyUnit.Pokemon.CurrentMove.Base.Priority;

            // Check who goes first
            bool playerGoesFirst = true;
            if (enemyMovePriority > playerMovePriority)
                playerGoesFirst = false;
            else if (enemyMovePriority == playerMovePriority)
                playerGoesFirst = playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed;

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;

            // First Turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver) yield break;

            if (secondPokemon.HP > 0)
            {
                // Second Turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver) yield break;
            }
        }
        else
        {
            if (playerAction == BattleAction.SwitchPokemon)
            {
                var selectedPokemon = partyScreen.SelectedMember;
                state = BattleState.Busy;
                yield return SwitchPokemon(selectedPokemon);
            }
            else if (playerAction == BattleAction.UseItem)
            {
                // This is handled from item screen, so do nothing and skip to enemy move
                dialogBox.EnableActionSelector(false);
            }
            else if (playerAction == BattleAction.Run)
            {
                yield return TryToEscape();
            }

            // Enemy Turn
            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver) yield break;
        }

        if (state != BattleState.BattleOver)
            ActionSelection();
    }

    /// <summary>
    /// Runs the move for the source unit on the target unit.
    /// </summary>
    /// <param name="sourceUnit">The source unit.</param>
    /// <param name="targetUnit">The target unit.</param>
    /// <param name="move">The move to be used.</param>
    /// <returns>An IEnumerator for the move.</returns>
    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.WaitForHPUpdate();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}!");

        if (CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {
            sourceUnit.PlayAttackAnimation();
            AudioManager.i.PlaySfx(move.Base.Sound);

            yield return new WaitForSeconds(1f);
            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.Pokemon, targetUnit.Pokemon, move.Base.Target);
            }
            else
            {
                var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
                yield return targetUnit.Hud.WaitForHPUpdate();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Pokemon.HP > 0)
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rnd = UnityEngine.Random.Range(1, 101);
                    if (rnd <= secondary.Chance)
                        yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, secondary.Target);
                }
            }

            if (targetUnit.Pokemon.HP <= 0)
            {
                yield return HandlePokemonFainted(targetUnit);
            }

        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}'s attack missed!");
        }
    }

    /// <summary>
    /// Applies Move Effects to the source and target Pokemon, including stat boosts, status conditions, and volatile status conditions.
    /// </summary>
    /// <param name="effects">The MoveEffects to apply.</param>
    /// <param name="source">The source Pokemon.</param>
    /// <param name="target">The target Pokemon.</param>
    /// <param name="moveTarget">The MoveTarget.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, MoveTarget moveTarget)
    {
        // Stat Boosting
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }

        // Status Condition
        if (effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status);
        }

        // Volatile Status Condition
        if (effects.VolatileStatus != ConditionID.none)
        {
            target.SetVolatileStatus(effects.VolatileStatus);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    /// <summary>
    /// Runs the logic for a unit after its turn has ended, including applying status effects and handling fainting.
    /// </summary>
    /// <param name="sourceUnit">The unit whose turn has ended.</param>
    /// <returns>An IEnumerator for the logic.</returns>
    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver) yield break;
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        // Statuses like burn or psn will hurt the pokemon after the turn
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.WaitForHPUpdate();
        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return HandlePokemonFainted(sourceUnit);
            yield return new WaitUntil(() => state == BattleState.RunningTurn);
        }
    }

    /// <summary>
    /// Checks if a move hits a target Pokemon based on accuracy and evasion boosts.
    /// </summary>
    /// <param name="move">The move to check.</param>
    /// <param name="source">The source Pokemon.</param>
    /// <param name="target">The target Pokemon.</param>
    /// <returns>True if the move hits, false otherwise.</returns>
    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.AlwaysHits)
            return true;

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    /// <summary>
    /// Iterates through the StatusChanges queue of a Pokemon and displays each message in a dialog box.
    /// </summary>
    /// <param name="pokemon">The Pokemon to display the StatusChanges of.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    /// <summary>
    /// Handles the logic when a Pokemon faints in battle. This includes awarding experience points, checking for level up, and learning new moves.
    /// </summary>
    /// <param name="faintedUnit">The BattleUnit that has fainted.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    IEnumerator HandlePokemonFainted(BattleUnit faintedUnit)
    {
        yield return dialogBox.TypeDialog($"{faintedUnit.Pokemon.Base.Name} Fainted!");
        AudioManager.i.PlaySfx(AudioId.Faint);
        faintedUnit.PlayFaintAnimation();
        yield return new WaitForSeconds(2f);

        if (!faintedUnit.IsPlayerUnit)
        {
            bool battleWon = true;
            if (isTrainerBattle)
                battleWon = trainerParty.GetHealthyPokemon() == null;

            if (battleWon && !isTrainerBattle)
                AudioManager.i.PlayMusic(wildBattleVictoryMusic);
            else if (battleWon && isTrainerBattle)
                AudioManager.i.PlayMusic(trainerBattleVictoryMusic);

            // Exp Gain
            int expYield = faintedUnit.Pokemon.Base.ExpYield;
            int enemyLevel = faintedUnit.Pokemon.Level;
            float trainerBonus = (isTrainerBattle) ? 1.5f : 1f;

            int expGain = Mathf.FloorToInt((expYield * enemyLevel * trainerBonus) / 7);
            playerUnit.Pokemon.Exp += expGain;
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} gained {expGain} exp!");
            yield return playerUnit.Hud.SetExpSmooth();

            // Check Level Up
            while (playerUnit.Pokemon.CheckForLevelUp())
            {
                playerUnit.Hud.SetLevel();
                yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} grew to level {playerUnit.Pokemon.Level}");

                // Try to learn a new Move
                var newMove = playerUnit.Pokemon.GetLearnableMoveAtCurrLevel();
                if (newMove != null)
                {
                    if (playerUnit.Pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
                    {
                        playerUnit.Pokemon.LearnMove(newMove.Base);
                        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} learned {newMove.Base.Name}");
                        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} is trying to learn {newMove.Base.Name}!");
                        yield return dialogBox.TypeDialog($"But, {playerUnit.Pokemon.Base.Name} can't learn more than {PokemonBase.MaxNumOfMoves} moves!");
                        yield return ChooseMoveToForget(playerUnit.Pokemon, newMove.Base);
                        yield return new WaitUntil(() => state != BattleState.MoveToForget);
                        yield return new WaitForSeconds(2f);
                    }
                }

                yield return playerUnit.Hud.SetExpSmooth(true);
            }

            yield return new WaitForSeconds(1f);
        }

        CheckForBattleOver(faintedUnit);
    }

    /// <summary>
    /// Checks if the battle is over after a unit has fainted. If the fainted unit is a player unit, the party screen is opened. If the fainted unit is an enemy unit, the battle is over if it is not a trainer battle. If it is a trainer battle, the next healthy pokemon is used.
    /// </summary>
    /// <param name="faintedUnit">A fainted battle unit</param>
    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
                OpenPartyScreen();
            else
                BattleOver(false);
        }
        else
        {
            if (!isTrainerBattle)
            {
                BattleOver(true);
            }
            else
            {
                var nextPokemon = trainerParty.GetHealthyPokemon();
                if (nextPokemon != null)
                    StartCoroutine(AboutToUse(nextPokemon));
                else
                    BattleOver(true);
            }
        }
    }

    /// <summary>
    /// Displays a dialog box with information about the damage dealt.
    /// </summary>
    /// <param name="damageDetails">The details of the damage dealt.</param>
    /// <returns>An IEnumerator that can be used to iterate through the dialog box.</returns>
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }

        if (damageDetails.TypeEffectiveness > 1f)
        {
            AudioManager.i.PlaySfx(AudioId.HitSuper);
            yield return dialogBox.TypeDialog("It's super effective!");
        }
        else if (damageDetails.TypeEffectiveness < 1f)
        {
            AudioManager.i.PlaySfx(AudioId.HitWeak);
            yield return dialogBox.TypeDialog("It's not very effective...");
        }
        else
        {
            AudioManager.i.PlaySfx(AudioId.HitNormal);
        }
    }

    /// <summary>
    /// Handles the update of the battle state. Depending on the current state, different actions are taken. 
    /// </summary>
    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
        else if (state == BattleState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = BattleState.ActionSelection;
            };

            Action<ItemBase> onItemUsed = (ItemBase usedItem) =>
            {
                StartCoroutine(OnItemUsed(usedItem));
            };

            // inventoryUI.HandleUpdate(onBack, onItemUsed);
        }
        else if (state == BattleState.AboutToUse)
        {
            HandleAboutToUse();
        }
        else if (state == BattleState.MoveToForget)
        {
            Action<int> onMoveSelected = (moveIndex) =>
            {
                moveSelectionUI.gameObject.SetActive(false);
                if (moveIndex == PokemonBase.MaxNumOfMoves)
                {
                    // Don't learn the new move
                    StartCoroutine(dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} did not learn {moveToLearn.Name}!"));
                }
                else
                {
                    // Forget the selected move and learn the new move
                    var selectedMove = playerUnit.Pokemon.Moves[moveIndex].Base;
                    StartCoroutine(dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} forgot {selectedMove.Name} and learned {moveToLearn.Name}!"));

                    playerUnit.Pokemon.Moves[moveIndex] = new Move(moveToLearn);
                }

                moveToLearn = null;
                state = BattleState.RunningTurn;
            };

            moveSelectionUI.HandleMoveSelection(onMoveSelected);
        }
    }

    /// <summary>
    /// Handles the selection of an action in a battle.
    /// </summary>
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                // Fight
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                // Bag
                OpenBag();
            }
            else if (currentAction == 2)
            {
                // Pokemon
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                // Run
                StartCoroutine(RunTurns(BattleAction.Run));
            }
        }
    }

    /// <summary>
    /// Handles the selection of a move by the player.
    /// </summary>
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            --currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var move = playerUnit.Pokemon.Moves[currentMove];
            if (move.PP == 0) return;

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(RunTurns(BattleAction.Move));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            ActionSelection();
        }
    }

    /// <summary>
    /// Handles the selection of a pokemon from the party screen.
    /// </summary>
    void HandlePartySelection()
    {
        Action onSelected = () =>
        {
            var selectedMember = partyScreen.SelectedMember;
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon!");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't switch with the same pokemon!");
                return;
            }

            //partyScreen.gameObject.SetActive(false);

            //if (partyScreen.CalledFrom == BattleState.ActionSelection)
            //{
            //    StartCoroutine(RunTurns(BattleAction.SwitchPokemon));
            //}
            //else
            //{
            //    state = BattleState.Busy;
            //    bool isTrainerAboutToUse = partyScreen.CalledFrom == BattleState.AboutToUse;
            //    StartCoroutine(SwitchPokemon(selectedMember, isTrainerAboutToUse));
            //}

            //partyScreen.CalledFrom = null;
        };

        Action onBack = () =>
        {
            if (playerUnit.Pokemon.HP <= 0)
            {
                partyScreen.SetMessageText("You have to choose a pokemon to continue!");
                return;
            }

            //partyScreen.gameObject.SetActive(false);

            //if (partyScreen.CalledFrom == BattleState.AboutToUse)
            //{
            //    StartCoroutine(SendNextTrainerPokemon());
            //}
            //else
            //    ActionSelection();

            //partyScreen.CalledFrom = null;
        };

        // partyScreen.HandleUpdate(onSelected, onBack);
    }

    /// <summary>
    /// Handles the logic for when the player is about to use a Pokemon.
    /// </summary>
    void HandleAboutToUse()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            aboutToUseChoice = !aboutToUseChoice;

        dialogBox.UpdateChoiceBox(aboutToUseChoice);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableChoiceBox(false);
            if (aboutToUseChoice == true)
            {
                // Yes Option
                OpenPartyScreen();
            }
            else
            {
                // No Option
                StartCoroutine(SendNextTrainerPokemon());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableChoiceBox(false);
            StartCoroutine(SendNextTrainerPokemon());
        }
    }

    /// <summary>
    /// Switches the current Pokemon to a new one.
    /// </summary>
    /// <param name="newPokemon">The new Pokemon to switch to.</param>
    /// <param name="isTrainerAboutToUse">Whether the trainer is about to use the new Pokemon.</param>
    /// <returns>An IEnumerator for the switching process.</returns>
    IEnumerator SwitchPokemon(Pokemon newPokemon, bool isTrainerAboutToUse = false)
    {
        if (playerUnit.Pokemon.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}!");
            playerUnit.PlayReturnAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);
        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");

        if (isTrainerAboutToUse)
            StartCoroutine(SendNextTrainerPokemon());
        else
            state = BattleState.RunningTurn;
    }

    /// <summary>
    /// Sends the next Pokemon from the trainer's party to battle the enemy unit. 
    /// </summary>
    /// <returns>
    /// An IEnumerator that types out the dialog for the trainer sending out the next Pokemon. 
    /// </returns>
    IEnumerator SendNextTrainerPokemon()
    {
        state = BattleState.Busy;

        var nextPokemon = trainerParty.GetHealthyPokemon();
        enemyUnit.Setup(nextPokemon);
        yield return dialogBox.TypeDialog($"{trainer.Name} sent out {nextPokemon.Base.Name}!");

        state = BattleState.RunningTurn;
    }

    /// <summary>
    /// Coroutine for using an item in battle.
    /// </summary>
    /// <param name="usedItem">The item being used.</param>
    /// <returns>
    /// An IEnumerator for the coroutine.
    /// </returns>
    IEnumerator OnItemUsed(ItemBase usedItem)
    {
        state = BattleState.Busy;
        inventoryUI.gameObject.SetActive(false);

        if (usedItem is PokeballItem)
        {
            yield return ThrowPokeball((PokeballItem)usedItem);
        }

        StartCoroutine(RunTurns(BattleAction.UseItem));
    }

    /// <summary>
    /// Throws a Pokeball at the enemy Pokemon in an attempt to catch it.
    /// </summary>
    /// <param name="pokeballItem">The Pokeball item to use.</param>
    /// <returns>An IEnumerator that runs the Pokeball throwing animation.</returns>
    IEnumerator ThrowPokeball(PokeballItem pokeballItem)
    {
        state = BattleState.Busy;

        if (isTrainerBattle)
        {
            // YOU BROKE THE RULES YOU THIEF
            yield return dialogBox.TypeDialog($"Don't be a thief!");
            state = BattleState.RunningTurn;
            yield break;
        }

        yield return dialogBox.TypeDialog($"{player.Name} used {pokeballItem.Name.ToUpper()}!");

        var pokeballObj = Instantiate(pokeballSprite, playerUnit.transform.position - new Vector3(5, 0), Quaternion.identity);
        var pokeball = pokeballObj.GetComponent<SpriteRenderer>();
        pokeball.sprite = pokeballItem.Icon;

        // Animations
        yield return pokeball.transform.DOJump(enemyUnit.transform.position + new Vector3(0, 2), 2f, 1, 1f).WaitForCompletion();
        yield return enemyUnit.PlayCaptureAnimation();
        yield return pokeball.transform.DOMoveY(enemyUnit.transform.position.y - 1.3f, 0.5f).WaitForCompletion();

        int shakeCount = TryToCatchPokemon(enemyUnit.Pokemon, pokeballItem);

        for (int i = 0; i < Mathf.Min(shakeCount, 3); ++i)
        {
            yield return new WaitForSeconds(0.5f);
            yield return pokeball.transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
        }

        if (shakeCount == 4)
        {
            // Pokemon is caught
            yield return dialogBox.TypeDialog($"Gotcha! {enemyUnit.Pokemon.Base.Name} was caught!");
            yield return pokeball.DOFade(0, 1.5f).WaitForCompletion();

            playerParty.AddPokemon(enemyUnit.Pokemon);
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} has been added to the party!");

            Destroy(pokeball);
            BattleOver(true);
        }
        else
        {
            // Pokemon broke out
            yield return new WaitForSeconds(0.5f);
            pokeball.DOFade(0, 0.2f);
            yield return enemyUnit.PlayBreakOutAnimation();

            if (shakeCount == 0)
                yield return dialogBox.TypeDialog($"Oh no! The wild {enemyUnit.Pokemon.Base.Name} broke free!");
            else if (shakeCount == 1)
                yield return dialogBox.TypeDialog($"Aww! It appeared to be caught!");
            else if (shakeCount == 2)
                yield return dialogBox.TypeDialog($"Aargh! Almost had it!");
            else
                yield return dialogBox.TypeDialog($"Gah! It was so close, too!");

            Destroy(pokeball);
            state = BattleState.RunningTurn;
        }
    }

    /// <summary>
    /// Attempts to catch a Pokemon using a Pokeball item.
    /// </summary>
    /// <param name="pokemon">The Pokemon to catch.</param>
    /// <param name="pokeballItem">The Pokeball item to use.</param>
    /// <returns>The number of shakes before the Pokemon is caught.</returns>
    int TryToCatchPokemon(Pokemon pokemon, PokeballItem pokeballItem)
    {
        float a = (3 * pokemon.MaxHP - 2 * pokemon.HP) * pokemon.Base.CatchRate * pokeballItem.CatchRateModifier * ConditionsDB.GetStatusBonus(pokemon.Status) / (3 * pokemon.MaxHP);

        if (a >= 255)
            return 4;

        float b = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / a));

        int shakeCount = 0;
        while (shakeCount < 4)
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
                break;

            ++shakeCount;
        }

        return shakeCount;
    }

    /// <summary>
    /// Tries to escape from the battle.
    /// </summary>
    /// <returns>
    /// An IEnumerator that will attempt to escape from the battle.
    /// </returns>
    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog($"You can't run from trainer battles!");
            state = BattleState.RunningTurn;
            yield break;
        }

        ++escapeAttempts;

        int playerSpeed = playerUnit.Pokemon.Speed;
        int enemySpeed = enemyUnit.Pokemon.Speed;

        if (enemySpeed < playerSpeed)
        {
            yield return dialogBox.TypeDialog($"Ran away safely!");
            BattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialogBox.TypeDialog($"Ran away safely!");
                BattleOver(true);
            }
            else
            {
                yield return dialogBox.TypeDialog($"Can't escape!");
                state = BattleState.RunningTurn;
            }
        }
    }
}