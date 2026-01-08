using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PauseCurrentCommand", story: "Pauses current command", category: "Action", id: "08ef8f472c9bf856d39cd0616172d2ad")]
public partial class PauseCurrentCommandAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<AllyCommand> newCommand;
    protected override Status OnStart()
    {
        if(commandData.Value.currentCommand != AllyCommand.None) //Perhaps add the priority to commands
        {
            commandData.Value.PauseCurrentCommand(newCommand);
        }
        else
        {
            commandData.Value.currentCommand = newCommand;
        }

        return Status.Success;
    }

}

