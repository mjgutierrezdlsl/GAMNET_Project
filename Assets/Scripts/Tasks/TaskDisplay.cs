using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskDisplay : Singleton<TaskDisplay>
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private TextMeshProUGUI _taskDescription;
    Dictionary<TaskTrigger, TextMeshProUGUI> _taskList = new();

    public void CreateEntry(TaskTrigger taskTrigger)
    {
        var taskDescription = Instantiate(_taskDescription, _contentParent);
        taskDescription.text = taskTrigger.Description;
        _taskList.Add(taskTrigger, taskDescription);
    }
    private void Update()
    {
        foreach (var kvp in _taskList)
        {
            kvp.Value.color = kvp.Key.IsTaskComplete ? Color.green : Color.white;
        }
    }
}