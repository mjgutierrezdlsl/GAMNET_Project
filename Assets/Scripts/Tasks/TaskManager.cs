using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField] private Transform _taskPanelParent;
    [SerializeField] private Transform _taskTriggerParent;
    [SerializeField] private TaskTrigger[] _taskTriggerPrefabs;
    [SerializeField] private int _taskCount = 1;
    [SerializeField] private float _spawnRadius = 3f;

    private void Start()
    {
        SpawnTasks();
    }

    public void SpawnTasks()
    {
        for (int i = 0; i < _taskCount; i++)
        {
            var taskTrigger = Instantiate(_taskTriggerPrefabs[Random.Range(0, _taskTriggerPrefabs.Length)], _taskTriggerParent);
            taskTrigger.transform.position = transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius;
            taskTrigger.Initialize(_taskPanelParent);
            TaskDisplay.Instance.CreateEntry(taskTrigger);
        }
    }

}
