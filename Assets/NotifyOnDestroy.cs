using System;
using UnityEngine;

#nullable enable
public class NotifyOnDestroy : MonoBehaviour
{
    public event EventHandler? OnOnDestroy;

    private void OnDestroy()
    {
        OnOnDestroy?.Invoke(this, null);
    }
}
