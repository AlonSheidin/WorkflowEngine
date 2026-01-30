
using System.Text.Json.Serialization;

namespace WorkflowEngine.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class JsonWorkflowStore : IWorkflowStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions =
        new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

    public JsonWorkflowStore(string filePath = "workflowInstances.json")
    {
        _filePath = filePath;

        if (!File.Exists(_filePath))
        {
            SaveAll(new Dictionary<int, WorkflowInstance>());
        }
    }


    public void SaveInstance(WorkflowInstance instance)
    {
        var data = LoadAll();

        if (data.ContainsKey(instance.InstanceId))
            throw new InvalidOperationException("Instance already exists.");

        data[instance.InstanceId] = instance;
        SaveAll(data);
    }

    public WorkflowInstance LoadInstance(int id)
    {
        var data = LoadAll();

        if (!data.TryGetValue(id, out var instance))
            throw new KeyNotFoundException($"Instance {id} not found.");

        return instance;
    }

    public void UpdateInstance(WorkflowInstance instance)
    {
        var data = LoadAll();

        if (!data.ContainsKey(instance.InstanceId))
            throw new KeyNotFoundException("Instance does not exist.");

        data[instance.InstanceId] = instance;
        SaveAll(data);
    }


    private Dictionary<int, WorkflowInstance> LoadAll()
    {
        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new Dictionary<int, WorkflowInstance>();

        return JsonSerializer.Deserialize<Dictionary<int, WorkflowInstance>>(json,  _jsonOptions)
               ?? new Dictionary<int, WorkflowInstance>();
    }

    private void SaveAll(Dictionary<int, WorkflowInstance> data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    
}
