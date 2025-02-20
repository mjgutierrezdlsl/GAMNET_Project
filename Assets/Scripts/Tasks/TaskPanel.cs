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
            _onTaskComplete?.Invoke();
        }
    }

    [SerializeField] private UnityEvent _onTaskComplete;
    public event Action OnTaskComplete;

    public virtual void CompleteTask()
    {
        IsComplete = true;
        Destroy(gameObject);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        ResetTask();
    }

    protected virtual void ResetTask()
    {
        IsComplete = false;
    }
}