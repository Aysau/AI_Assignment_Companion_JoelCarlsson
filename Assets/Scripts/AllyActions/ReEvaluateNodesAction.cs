using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ReEvaluateNodes", story: "Restarts the tree to make sure changes are adapted to", category: "Action", id: "f2317b5d4a1c2640cc5d6e300ed8f4ea")]
public partial class ReEvaluateNodesAction : Action
{

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

