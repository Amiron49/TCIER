using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnMoneyOnDeath : MonoBehaviour
{
    [FormerlySerializedAs("amount")] public int TotalMoney = 10;
    [FormerlySerializedAs("worth")] public int ChunkThreshold = 10;
    private GameObject _moneyPrefab;

    private void Start()
    {
        _moneyPrefab = Game.Instance.Prefabs.Enemy.MoneyPrefab;
    }

    private void OnDestroy()    
    {
        var chunks = TotalMoney / ChunkThreshold;
        var worthPerChunk = TotalMoney / chunks;
        
        for (var i = 0; i < chunks; i++)
            Spawn(worthPerChunk);
    }

    private void Spawn(int worthPerChunk)
    {
        var instance = Instantiate(_moneyPrefab, transform.position, Quaternion.identity);
        var asMoney = instance.GetComponent<MoneyPickup>();
        asMoney.amount = worthPerChunk;
        var wackyHoming = instance.GetComponent<WackyHoming>();
        wackyHoming.to = Game.Instance.State.Player.gameObject;
    }
}
