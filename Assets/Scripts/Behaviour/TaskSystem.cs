using System.Collections.Generic;
using UnityEngine;

public class TaskSystem
{
    public List<Task> TaskQueue = new List<Task>();
    private Task _currentTask = null;

    private int _taskCount;


    public void Run()
    {
        if (TaskQueue.Count == 0)
        {
            return;
        }
        if (_currentTask == null)
        {
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

    public enum Priority
    {
        Low, Medium, High
    }

    private Status _taskStatus = Status.Pending;
    private Priority _taskPriority = Priority.Medium; 

    public Task(string taskName)
    {
        TaskName = taskName;
    }

    public virtual void Execute()
    {
        Debug.Log("Task \"" + TaskName + "\" not implemented!");
    }

    public Status TaskStatus 
    { 
        get { return _taskStatus; } 
        set { _taskStatus = value; }
    }
    public Priority TaskPriority 
    { 
        get { return _taskPriority; }
        set { _taskPriority = value; }
    }
}