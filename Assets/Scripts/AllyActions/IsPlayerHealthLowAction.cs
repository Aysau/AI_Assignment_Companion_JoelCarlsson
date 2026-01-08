using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsPlayerHealthLow", story: "Check if player health is low", category: "Action", id: "3812d80d4226ba00f9a3053180029b89")]
public partial class IsPlayerHealthLowAction : Action
{

    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> threshold;
    protected override Status OnStart()
    {
        bool criticalStatePlayer = commandData.Value.IsPlayerHealthLow(threshold);
        if (criticalStatePlayer) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

