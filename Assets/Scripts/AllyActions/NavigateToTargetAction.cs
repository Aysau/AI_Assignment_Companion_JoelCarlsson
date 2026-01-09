using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

public enum TargetType
{
    CommandTargetEnemy,
    CommandTargetObject,
    PlayerTransform,
    NearestEnemy,
    NearestHealthPickup,
}


[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavigateToTarget", story: "Navigate to a GameObject using Navmeshagent", category: "Action", id: "efd456adcdb5ea8065e1362552bd3061")]
public partial class NavigateToTargetAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;


    [SerializeReference]
    public BlackboardVariable<TargetType> targetType;
    [SerializeReference]
    public BlackboardVariable<bool> successOnDestination;
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
        GameObject target = GetTarget();

        if (target == null || !target.activeInHierarchy)
        {
            navAgent.Value.isStopped = true;
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, target.transform.position);

        if (distance <= arriveDistance && successOnDestination)
        {
            //navAgent.Value.isStopped = true;
            return Status.Success;

        }

        navAgent.Value.isStopped = false;
        navAgent.Value.SetDestination(target.transform.position);
        return Status.Running;
    }


    private GameObject GetTarget() //Depending on what the target type is (can be set in tree) return different values
    {
        switch (targetType.Value)
        {
            case TargetType.CommandTargetEnemy: return commandData.Value.targetEnemy;
            case TargetType.CommandTargetObject: return commandData.Value.targetObject;
            case TargetType.PlayerTransform: return commandData.Value.playerTransform.gameObject;
            case TargetType.NearestEnemy: return perception.Value.GetNearestEnemy();
            case TargetType.NearestHealthPickup: return perception.Value.GetNearestHealthPickup();
            default: return null;
        }
    }
}

