using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMoneyOnDeath : MonoBehaviour
{
    public int amount = 1;
    public int worth = 1;
    public GameObject moneyPrefab;
    
    private void OnDestroy()
    {
        for (var i = 0; i < amount; i++)
            Spawn();
    }

    private void Spawn()
    {
        var instance = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
        var asMoney = instance.GetComponent<MoneyPickup>();
        asMoney.amount = worth;
        var wackyHoming = instance.GetComponent<WackyHoming>();
        wackyHoming.to = Game.Instance.State.Player.gameObject;
    }
}
