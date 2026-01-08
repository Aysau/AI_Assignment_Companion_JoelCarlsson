using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HasCommand", story: "Check if ally has specific command", category: "Action", id: "ffedc7e810ff0aa5bb22404f45205989")]
public partial class HasCommandAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<AllyCommand> requiedCommand;

    protected override Status OnStart()
    {
        bool hasCommand = (commandData.Value.currentCommand == requiedCommand);
        if (hasCommand) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

