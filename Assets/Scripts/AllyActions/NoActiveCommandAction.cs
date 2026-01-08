using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NoActiveCommand", story: "Check if ally has no active command", category: "Action", id: "7ee7ef89f3806a6320771e526ec2839c")]
public partial class NoActiveCommandAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;

    protected override Status OnStart()
    {
        bool noActiveCommand = (commandData.Value.currentCommand == AllyCommand.None);
        if (noActiveCommand) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

