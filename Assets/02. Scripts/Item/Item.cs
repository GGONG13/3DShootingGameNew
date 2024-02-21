using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Health,
    Stamina,
    Bullet
}
public class Item
{
    public ItemType ItemType;
    public int Count;


    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }
  

    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }
        Count--;
        switch (ItemType)
        {
            case ItemType.Health:
            {
                // Todo : �÷��̾� ü�� ������
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                break;
            }
            case ItemType.Stamina:
            {
                // Todo : �÷��̾� ���¹̳� ������
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;
                break;
            }
            case ItemType.Bullet:
            {
                // Todo : �÷��̾ ���� ��� �ִ� ���� �Ѿ��� ������.
                PlayerGunFireAbility playerGunFire = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                playerGunFire.CurrentGun._bulletCount = playerGunFire.CurrentGun._bullet;
                playerGunFire.RefreshGun();
                break;
            }
        }
        return true;
    }


    // UI�� ���ΰ�ħ���ִ� �Լ�
  
}
