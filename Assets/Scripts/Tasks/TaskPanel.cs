using System;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private UnityEvent OnTaskComplete;
    public virtual void CompleteTask() => IsComplete = true;
    public virtual void ResetTask() => IsComplete = false;
}