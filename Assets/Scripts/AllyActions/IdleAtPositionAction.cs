using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IdleAtPosition", story: "Idle at current or specific position", category: "Action", id: "fc4cdcb222f05249ae72a5cb3edc4e12")]
public partial class IdleAtPositionAction : Action
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
    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        Vector3 idlePos = GetTargetPosition();
        float distance = Vector3.Distance(allyTransform.Value.position, idlePos);

        if (distance > arriveDistance)
        {
            navAgent.Value.isStopped = false;
            navAgent.Value.SetDestination(idlePos);
        }
        return Status.Running;
        //else
        //{
        //    navAgent.Value.isStopped = true;
        //    return Status.Success;
        //}
    }
    private Vector3 GetTargetPosition()
    {
        switch (positionType.Value)
        {
            case PositionType.WaitPosition: return commandData.Value.waitPosition;
            case PositionType.PlayerPosition: return commandData.Value.playerTransform.position;
            default: return Vector3.zero;
        }
    }
}

