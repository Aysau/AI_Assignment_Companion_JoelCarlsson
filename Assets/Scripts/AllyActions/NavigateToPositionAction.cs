using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;


public enum PositionType
{
    WaitPosition,
    PlayerPosition,
    FollowPosition
}


[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavigateToPosition", story: "Navigate to a specific postion using Navmeshagent", category: "Action", id: "9078fa30e9f6dbbc4eea6a85bfaaa13a")]
public partial class NavigateToPositionAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<PositionType> positionType;
    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<NavMeshAgent> navAgent;
    [SerializeReference]
    public BlackboardVariable<float> arriveDistance;
    [SerializeReference]
    public BlackboardVariable<bool> commandChanged;

    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        Vector3 targetPos = GetTargetPosition();
        float distance = Vector3.Distance(allyTransform.Value.position, targetPos);

        //if (distance <= arriveDistance)
        //{
        //    navAgent.Value.isStopped = true;
        //    return Status.Success;
        //}



        navAgent.Value.isStopped = false;
        navAgent.Value.SetDestination(targetPos);
        return Status.Running;
    }

    private Vector3 GetTargetPosition()
    {
        switch (positionType.Value)
        {
            case PositionType.WaitPosition: return commandData.Value.waitPosition;
            case PositionType.PlayerPosition: return commandData.Value.playerTransform.position;
            case PositionType.FollowPosition: return commandData.Value.GetFollowPosition();
            default: return Vector3.zero;
        }
    }
}

