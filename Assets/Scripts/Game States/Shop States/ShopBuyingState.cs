using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyingState : State<GameController>
{
    [SerializeField] Vector2 shopCameraOffset;
    [SerializeField] ShopUI shopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    public static ShopBuyingState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    // Input
    public List<ItemBase> AvailableItems { get; set; }

    bool browseItems = false;

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        browseItems = false;
        StartCoroutine(StartBuyingState());
    }

    public override void Execute()
    {
        if (browseItems)
            shopUI.HandleUpdate();
    }

    IEnumerator StartBuyingState()
    {
        yield return GameController.Instance.MoveCamera(shopCameraOffset);
        walletUI.Show();
        shopUI.Show(AvailableItems, (item) => StartCoroutine(BuyItem(item)),
            () => StartCoroutine(OnBackFromBuying()));

        browseItems = true;
    }

    IEnumerator BuyItem(ItemBase item)
    {
        browseItems = false;

        yield return DialogManager.Instance.ShowDialogText($"How many {item.Name} would you like to purchase?",
            waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(Mathf.Min(9999, Mathf.FloorToInt(Wallet.i.Money / item.Price)),
            item.Price, (selectedCount) => countToBuy = selectedCount);

        DialogManager.Instance.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText($"You are going to be buying {countToBuy} {item.Name} for {totalPrice}$. Are you sure about this purchase?",
                waitForInput: false,
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Yes
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogManager.Instance.ShowDialogText($"Thank you for shopping with us!");
            }
        }
        else
        {
            if (countToBuy > 1)
                yield return DialogManager.Instance.ShowDialogText($"You don't have enough money to buy {countToBuy} {item.Name}'s!");
            else
                yield return DialogManager.Instance.ShowDialogText($"You don't have enough money to buy a {item.Name}!");
        }

        browseItems = true;
    }

    IEnumerator OnBackFromBuying()
    {
        yield return GameController.Instance.MoveCamera(-shopCameraOffset);
        shopUI.Close();
        walletUI.Close();
        gc.StateMachine.Pop();
    }
}
