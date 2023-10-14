using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;

    private void Start()
    {
        Wallet.i.OnMoneyChanged += SetMoneyText;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetMoneyText();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void SetMoneyText()
    {
        moneyText.text = "$ " + Wallet.i.Money;
    }
}
