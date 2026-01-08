using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsHealthCritical", story: "Checks if ally health is critical", category: "Action", id: "f13dda4a9a722ff029ff4075e6d348df")]
public partial class IsHealthCriticalAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> threshold;
    protected override Status OnStart()
    {
        bool criticalState = commandData.Value.IsHealthCritical(threshold);
        if(criticalState) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

