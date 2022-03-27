using System;
using UnityEngine;

#nullable enable
public class NotifyOnDeath : MonoBehaviour
{
    public event EventHandler? OnDeath;

    private void OnDestroy()
    {
        OnDeath?.Invoke(this, null);
    }
}
