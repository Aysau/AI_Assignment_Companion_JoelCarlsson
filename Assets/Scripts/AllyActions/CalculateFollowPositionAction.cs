using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CalculateFollowPosition", story: "Calculate follow position behind player", category: "Action", id: "2513b0ded26196e3d172cdf07241a438")]
public partial class CalculateFollowPositionAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<Transform> playerTransform;
    [SerializeReference]
    public BlackboardVariable<float> followDistance;
    protected override Status OnStart()
    {
        if(playerTransform.Value == null)
        {
            return Status.Failure;
        }

        Vector3 followPos = playerTransform.Value.position - playerTransform.Value.forward * commandData.Value.followDistance;

        commandData.Value.followPosition = followPos;

        return Status.Success;
    }
}

