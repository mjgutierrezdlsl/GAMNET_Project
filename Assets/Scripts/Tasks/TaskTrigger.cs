using UnityEngine;
using UnityEngine.Events;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private TaskPanel _taskPanel;
    [SerializeField] private UnityEvent _onTaskActivate;

    private Transform _panelParent;
    private bool _isInitialized;

    public bool IsCompleted { get; private set; }

    public void Initialize(Transform panelParent)
    {
        _panelParent = panelParent;
        _isInitialized = true;
    }

    public void ActivateTask()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"{name} is not initialized.");
            return;
        }
        var panel = Instantiate(_taskPanel, _panelParent);
    }
}