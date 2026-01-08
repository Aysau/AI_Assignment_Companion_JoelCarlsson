using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ClearCommand", story: "Claer current command", category: "Action", id: "795dcafae8c1e6172ff998915645a02b")]
public partial class ClearCommandAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    protected override Status OnStart()
    {
        commandData.Value.ClearCommand();
        return Status.Success;
    }
}

