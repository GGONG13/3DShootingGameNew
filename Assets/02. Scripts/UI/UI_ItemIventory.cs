using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class UI_ItemIventory : MonoBehaviour
{
    public TextMeshProUGUI HealthItemCountTextUI;
    public TextMeshProUGUI StaminaItemCountTextUI;
    public TextMeshProUGUI BulletItemCountTextUI;

    private void Start()
    {
        Refresh();

        ItemManager.Instance.OnDataChanged = Refresh;
      //  ItemManager.Instance.Mulity(Refresh);
    }
    public void Refresh()
    {
        HealthItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{ItemManager.Instance.GetItemCount(ItemType.Bullet)}";
    }

  
}
