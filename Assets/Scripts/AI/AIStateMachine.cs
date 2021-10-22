using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private List<IAITask> tasks;

    public IAITask GetTask(IAITask searchedTask)
    {
        return Array.Find(tasks.ToArray(), task => task.taskID == searchedTask.taskID);
    }

    public IAITask GetTask(string searchedTask)
    {
        return Array.Find(tasks.ToArray(), task => task.taskID == searchedTask);
    }

    private int SearchForTask(IAITask searchedTask)
    {
        int taskIndex = -1;
        taskIndex = Array.FindIndex(tasks.ToArray(), task => task.taskID == searchedTask.taskID);

        return taskIndex;
    }

    public void ChangeUpdateTime(IAITask changingTask, float newSpeed)
    {
        int taskIndex = SearchForTask(changingTask);
        if(taskIndex == -1)
        {
            Debug.LogError("Couldn't find " + changingTask);
            return;
        }

        tasks[taskIndex].updateState = IAITask.UpdateState.custom;
        tasks[taskIndex].updateTime = newSpeed;

    }
    public void ChangeToDelta(IAITask changingTask)
    {
        int taskIndex = SearchForTask(changingTask);
        if (taskIndex == -1)
        {
            Debug.LogError("Couldn't find " + changingTask);
            return;
        }
        tasks[taskIndex].updateState = IAITask.UpdateState.delta;
    }
    public void ChangeToFixed(IAITask changingTask)
    {
        int taskIndex = SearchForTask(changingTask);
        if (taskIndex == -1)
        {
            Debug.LogError("Couldn't find " + changingTask);
            return;
        }

        tasks[taskIndex].updateState = IAITask.UpdateState.fixedDelta;
    }
    public void AddTask(IAITask newState)
    {
        tasks.Add(newState);
    }

    public void RemoveState(IAITask task)
    {
        int taskIndex = SearchForTask(task);
        if (taskIndex == -1)
        {
            Debug.LogError("Couldn't find " + task);
            return;
        }

        tasks.Remove(tasks[taskIndex]);
    }
}
