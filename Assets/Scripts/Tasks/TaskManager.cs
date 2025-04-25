using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class TaskManager : NetworkSingleton<TaskManager>
{
    [SerializeField] private Transform _taskPanelParent;
    [SerializeField] private Transform _taskTriggerParent;
    [SerializeField] private TaskTrigger[] _taskTriggerPrefabs;
    [SerializeField] private int _taskCount = 1;
    [SerializeField] private float _spawnRadius = 3f;
    private List<TaskTrigger> _taskTriggers = new();
    private bool _areTasksComplete;

    private void Start()
    {
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong id)
    {
        print($"Client {id} connected");
        if (IsServer)
        {
            SpawnTasks();
            // Unsubscribe from event to prevent spawning multiple times
            NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        }

    }

    public void SpawnTasks()
    {
        for (int i = 0; i < _taskCount; i++)
        {
            var taskTrigger = Instantiate(_taskTriggerPrefabs[Random.Range(0, _taskTriggerPrefabs.Length)], _taskTriggerParent);
            taskTrigger.transform.position = transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius;
            taskTrigger.Initialize(_taskPanelParent);
            // TaskDisplay.Instance.CreateEntry(taskTrigger);
            _taskTriggers.Add(taskTrigger);
            taskTrigger.GetComponent<NetworkObject>().Spawn();
        }
    }

    private void Update()
    {
        // TODO: MOVE CHECKS OUT OF UPDATE
        if (_areTasksComplete) { return; }
        var completeTasks = _taskTriggers.Count(trigger => trigger.IsTaskComplete);
        if (completeTasks < _taskCount) { return; }
        else
        {
            TaskDisplay.Instance.CreateEntry("All Tasks Complete", Color.green);
            _areTasksComplete = true;
        }
    }

}
