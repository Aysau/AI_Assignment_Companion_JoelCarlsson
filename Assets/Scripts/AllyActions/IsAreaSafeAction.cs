using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsAreaSafe", story: "Check if area is safe (no enemies)", category: "Action", id: "13073f14886bdba79b87fddfff910a00")]
public partial class IsAreaSafeAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;

    protected override Status OnStart()
    {
        bool AreaSafe = commandData.Value.isAreaSafe;
        if (AreaSafe) return Status.Success;
        else
        {
            return Status.Failure;
        }
    }
}

