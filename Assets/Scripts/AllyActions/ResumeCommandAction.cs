using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResumeCommand", story: "Resume paused command", category: "Action", id: "3159c4936bdf14760fd5ef0844fb52f2")]
public partial class ResumeCommandAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    protected override Status OnStart()
    {
        commandData.Value.ResumePausedCommand();
        return Status.Success;
    }

}

