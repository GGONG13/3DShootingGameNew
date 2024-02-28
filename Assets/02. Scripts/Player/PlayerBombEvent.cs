using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombEvent : MonoBehaviour
{
    private PlayerBombFireAbility _owner;
    void Start()
    {
        _owner = GetComponentInParent<PlayerBombFireAbility>();
    }

    private void BombEvent()
    {
        _owner.BombGo();
    }
}
