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
public class ItemObjectTypeFactory
{
    public ItemType ItemType;
    public int Count;


    

    public ItemObjectTypeFactory(ItemType itemType, int count)
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
                // Todo : 플레이어 체력 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                break;
            }
            case ItemType.Stamina:
            {
                // Todo : 플레이어 스태미너 꽉차기
                PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
                playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;
                break;
            }
            case ItemType.Bullet:
            {
                // Todo : 플레이어가 현재 들고 있는 총의 총알이 꽉찬다.
                PlayerGunFireAbility playerGunFire = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
                playerGunFire.CurrentGun._bulletCount = playerGunFire.CurrentGun._bullet;
                playerGunFire.RefreshGun();
                break;
            }
        }
        return true;
    }


    // UI를 새로고침해주는 함수
  
}
