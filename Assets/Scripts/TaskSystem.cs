using System.Collections.Generic;
using UnityEngine;

public class TaskSystem
{
    public List<Task> TaskQueue = new List<Task>();
    private Task _currentTask = null;

    private int _taskCount;


    public void Run()
    {
        if (_currentTask == null)
        {
            if (TaskQueue.Count == 0)
            {
                return;
            }
            _currentTask = TaskQueue[0];
        }

        if (_currentTask.TaskStatus == Task.Status.Ongoing || _currentTask.TaskStatus == Task.Status.Pending)
        {
            _currentTask.Execute();
        }
        else if (_currentTask.TaskStatus == Task.Status.Finished)
        {
            TaskQueue.RemoveAt(0);
            if (TaskQueue.Count == 0)
            {
                _currentTask = null;
                return;
            } 
            _currentTask = TaskQueue[0];
        }
        Debug.Log(_currentTask.TaskName);
    }

    public void AddTask(Task task)
    {
        TaskQueue.Add(task);
    }

    public int GetTaskQueueLength()
    {
        return TaskQueue.Count;
    }
}

public class Task
{
    public string TaskName;

    public enum Status
    {
        Pending, Ongoing, Finished
    };

    public Status TaskStatus = Status.Pending;


    public Task(string taskName)
    {
        TaskName = taskName;
    }

    public virtual void Execute()
    {
        Debug.Log("Task \"" + TaskName + "\" not implemented!");
    }
}