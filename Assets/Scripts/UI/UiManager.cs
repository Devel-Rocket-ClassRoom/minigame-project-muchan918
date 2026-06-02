using System;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    private readonly List<Action> closeActions = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(Action closeAction)
    {
        closeActions.Add(closeAction);
    }

    public void Unregister(Action closeAction)
    {
        closeActions.Remove(closeAction);
    }

    public void CloseAll()
    {
        var copy = new List<Action>(closeActions);
        closeActions.Clear();
        foreach (var action in copy)
            action?.Invoke();
    }
}
