using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] List<ItemBase> availableItems;

    public IEnumerator Trade()
    {
        ShopMenuState.i.AvailableItems = availableItems;
        yield return GameController.Instance.StateMachine.PushAndWait(ShopMenuState.i);
    }

    public List<ItemBase> AvailableItems => availableItems;
}
