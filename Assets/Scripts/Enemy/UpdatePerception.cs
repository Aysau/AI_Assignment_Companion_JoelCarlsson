using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace GameAI.Lab4
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
    name: "Update Perception",
    description: "Updates Target/LOS/LastKnownPosition from GuardSensors.",
    story: "Update perception and write to the blackboard.",
    category: "Action/Sensing",
    id: "b0c8b9b7-8f64-4c0c-a9a0-0d9e7c2e2fb8"
)]
    public class UpdatePerception : Unity.Behavior.Action
    {
        [SerializeReference]
        public BlackboardVariable<GameObject>
    Target;
        [SerializeReference]
        public BlackboardVariable<bool>
    HasLineOfSight;
        [SerializeReference]
        public BlackboardVariable<Transform>
    selfTransform;
        [SerializeReference]
        public BlackboardVariable<Vector3>
    LastKnownPosition;
        [SerializeReference]
        public BlackboardVariable<float>
    TimeSinceLastSeen;

        protected override Node.Status OnStart()
        {
            // Ensure we have sane defaults. 
            if (TimeSinceLastSeen != null && TimeSinceLastSeen.Value
    < 0f)
                TimeSinceLastSeen.Value = 9999f;
            LastKnownPosition.Value = selfTransform.Value.position;
            return Node.Status.Running;
        }

        protected override Node.Status OnUpdate()
        {
            var sensors = GameObject != null ?
    GameObject.GetComponent<GuardSensors>() : null;
            if (sensors == null)
            {
                // No sensors attached -> treat as "can't see 
                //anything" 
                if (HasLineOfSight != null) HasLineOfSight.Value =
    false;
                if (TimeSinceLastSeen != null)
                    TimeSinceLastSeen.Value += Time.deltaTime;
                return Node.Status.Success;
            }

            bool sensed = sensors.TrySenseTarget(
                out GameObject sensedTarget,
                out Vector3 sensedPos,
                out bool hasLOS
            );

            if (sensed && hasLOS)
            {
                if (Target != null) Target.Value = sensedTarget;
                if (HasLineOfSight != null) HasLineOfSight.Value =
    true;
                if (LastKnownPosition != null)
                    LastKnownPosition.Value = sensedPos;
                if (TimeSinceLastSeen != null)
                    TimeSinceLastSeen.Value = 0f;
            }
            else
            {
                // Keep Target as-is (we "remember" who we were 
                //chasing), 
                // but mark that we don't currently have LOS. 
                if (HasLineOfSight != null) HasLineOfSight.Value =
    false;
                if (TimeSinceLastSeen != null)
                    TimeSinceLastSeen.Value += Time.deltaTime;
            }

            // This node is a fast "service-like" update; it 
            //finishes immediately each tick. 
            return Node.Status.Running;
        }
    }

}
