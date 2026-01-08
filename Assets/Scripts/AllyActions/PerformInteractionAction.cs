using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.AppUI.UI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PerformInteraction", story: "Perform interaction with an object", category: "Action", id: "44626ee06ca4b269c9d3bd6f32a52d48")]
public partial class PerformInteractionAction : Action
{
    [SerializeReference]
    public BlackboardVariable<Transform> allyTransform;
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> interactRange;
    protected override Status OnStart()
    {
        GameObject target = commandData.Value.targetObject;

        if(target == null)
        {
            return Status.Failure;
        }

        float distance = Vector3.Distance(allyTransform.Value.position, target.transform.position);

        if(distance <= interactRange)
        {
            InteractableButton button = commandData.Value.targetObject.GetComponent<InteractableButton>();
            if(button != null)
            {
                button.Press();
                Debug.Log($"[Ally] Interacted with {target.name}");
            }
            else
            {
                Debug.LogError("[Ally] No InteractableButton script found on targetObject");
                return Status.Failure;
            }
            

            

            return Status.Success;
        }

        return Status.Failure;
    }


}

