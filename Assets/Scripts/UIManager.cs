using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text currentBalance;
    public int balance;

    public GameObject machinePanel;
    public GameObject candyPanel;

    // Use this for initialization
    void Start()
    {
        currentBalance.text = "$" + balance.ToString();
    }

    public void AddToBalance(int amount)
    {
        balance += amount;
        currentBalance.text = "$" + balance.ToString();
    }

    public bool CanBuy(int amount)
    {
        if (amount <= balance)
            return true;
        return false;
    }

    public void ShowHideMachines()
    {
        if (machinePanel.activeSelf == false)
        {
            if (candyPanel.activeSelf == true)
                candyPanel.SetActive(false);
            machinePanel.SetActive(true);
        }
        else
            machinePanel.SetActive(false);
    }

    public void ShowHideCandy()
    {
        if (candyPanel.activeSelf == false)
        {
            if (machinePanel.activeSelf == true)
                machinePanel.SetActive(false);
            candyPanel.SetActive(true);
        }
        else
            candyPanel.SetActive(false);
    }
}
