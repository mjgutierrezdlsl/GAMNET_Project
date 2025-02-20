using System;
using UnityEngine;

public abstract class TaskPanel : MonoBehaviour
{
    private bool _isComplete;
    public bool IsComplete
    {
        get => _isComplete;
        protected set
        {
            _isComplete = value;

            if (!_isComplete) { return; }
            OnTaskComplete?.Invoke();
        }
    }
    public event Action OnTaskComplete;
    public virtual void CompleteTask() => IsComplete = true;
    public virtual void ResetTask() => IsComplete = false;
}