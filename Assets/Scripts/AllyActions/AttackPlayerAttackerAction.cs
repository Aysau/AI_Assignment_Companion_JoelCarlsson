using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackPlayerAttacker", story: "Attack the enemy threatening the player", category: "Action", id: "d839583a3ebaf726e1a77a275514b24e")]
public partial class AttackPlayerAttackerAction : Action
{

    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<NavMeshAgent> navAgent;
    [SerializeReference]
    public BlackboardVariable<AllyPerceptionSystem> perception;
    [SerializeReference]
    public BlackboardVariable<float> attackRange;
    [SerializeReference]
    public BlackboardVariable<float> attackCooldown;
    [SerializeReference]
    public BlackboardVariable<float> lastAttackTime;
    [SerializeReference]
    public BlackboardVariable<float> damageAmount;
    protected override Status OnStart()
    {
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        GameObject enemy = perception.Value.GetPlayerAttacker();

        if (enemy == null)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, enemy.transform.position);

        if (distance > attackRange)
        {
            navAgent.Value.isStopped = false;
            navAgent.Value.SetDestination(enemy.transform.position);
            return Status.Running;
        }
        else //In range so can perform attack
        {
            navAgent.Value.isStopped = true;

            Vector3 direction = (enemy.transform.position - allyTransform.Value.position).normalized;
            direction.y = 0; //TODO maybe wrong
            if (direction != Vector3.zero)
            {
                allyTransform.Value.rotation = Quaternion.LookRotation(direction);
            }

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                PerformAttack(enemy);
                lastAttackTime.Value = Time.time;
            }

            return Status.Running;
        }
    }

    void PerformAttack(GameObject enemy)
    {
        Health targetHealth = enemy.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            Debug.Log($"[Ally] Attacking {enemy.name} for {damageAmount} damage");
        }
        else
        {
            Debug.Log($"[Ally] Defending player from: {enemy.name}");
        }
            
    }
}

