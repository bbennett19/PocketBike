using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour 
{
    public int itemID;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Buy);
    }

    private void OnEnable()
    {
        _button.interactable = !PlayerPointsAndItems.Instance.playerData.itemsPurchased[itemID];
    }

    public void Buy()
    {
        PlayerPointsAndItems.Instance.playerData.itemsPurchased[itemID] = true;
        _button.interactable = false;
    }
}
