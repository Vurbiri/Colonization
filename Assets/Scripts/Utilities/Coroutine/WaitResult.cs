using System;
using UnityEngine;

public class WaitResult<T> : CustomYieldInstruction
{
    public T Result { get; private set; }

    public override bool keepWaiting => _keepWaiting;
    private bool _keepWaiting = true;

    public static WaitResult<T> Empty { get; } = new(default);

    public event Action<T> EventCompleted;

    public WaitResult()
    {
        _keepWaiting = true;
    }
    public WaitResult(T result) => SetResult(result);

    public void SetResult(T result)
    {
        Result = result;
        _keepWaiting = false;

        EventCompleted?.Invoke(result);
    }

    public WaitResult<T> Delete()
    {
        Result = default;
        _keepWaiting = false;

        return new();
    }

}
