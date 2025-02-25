using UnityEngine;
using UnityEngine.Events;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private TaskPanel _taskPanel;
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [SerializeField] private UnityEvent _onTaskComplete;

    private Transform _panelParent;
    private TaskPanel _panelInstance;
    private bool _isInitialized;

    public bool IsTaskComplete { get; private set; }

    public void Initialize(Transform panelParent)
    {
        _panelParent = panelParent;
        _isInitialized = true;
        _panelInstance = Instantiate(_taskPanel, _panelParent);
        _panelInstance.gameObject.SetActive(false);

        _panelInstance.OnTaskComplete += () =>
        {
            IsTaskComplete = true;
            _onTaskComplete?.Invoke();
        };
    }

    public void ActivateTask()
    {
        if (!_isInitialized)
        {
            Debug.LogWarning($"{name} is not initialized.");
            return;
        }
        _panelInstance.gameObject.SetActive(true);
    }
}