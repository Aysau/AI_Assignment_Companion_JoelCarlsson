using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateAllyVariables", story: "Updates the variables for the ally", category: "Action", id: "369911e47bd743d70806eaf9444144cf")]
public partial class UpdateAllyVariablesAction : Action
{
    [SerializeReference]
    public BlackboardVariable<AllyCommandData> commandData;
    [SerializeReference]
    public BlackboardVariable<float> thresholdHealthPlayer;
    [SerializeReference]
    public BlackboardVariable<float> thresholdHealthAlly;


    [SerializeReference]
    public BlackboardVariable<bool> allyHealthCritical;
    [SerializeReference]
    public BlackboardVariable<bool> playerHealthLow;
    [SerializeReference]
    public BlackboardVariable<bool> allyUnderAttack;
    [SerializeReference]
    public BlackboardVariable<bool> playerUnderAttack;
    [SerializeReference]
    public BlackboardVariable<CurrentCommand> currentCommand;
    [SerializeReference]
    public BlackboardVariable<bool> areaSafe;

    protected override Status OnUpdate() //Updates all the ally variables on the blackboard
    {
        allyHealthCritical.Value = commandData.Value.IsHealthCritical(thresholdHealthAlly);
        playerHealthLow.Value = commandData.Value.IsPlayerHealthLow(thresholdHealthPlayer);
        allyUnderAttack.Value = commandData.Value.isSelfUnderAttack;
        playerUnderAttack.Value = commandData.Value.isPlayerUnderAttack;
        areaSafe.Value = commandData.Value.isAreaSafe;

        switch (commandData.Value.currentCommand)
        {
            case AllyCommand.None:
                currentCommand.Value = CurrentCommand.None;
                break;
            case AllyCommand.Wait:
                currentCommand.Value = CurrentCommand.Wait;
                break;
            case AllyCommand.AttackTarget:
                currentCommand.Value = CurrentCommand.AttackTarget;
                break;
            case AllyCommand.Interact:
                currentCommand.Value = CurrentCommand.Interact;
                break;
            case AllyCommand.Flee:
                currentCommand.Value = CurrentCommand.Flee;
                break;
            case AllyCommand.DefendSelf:
                currentCommand.Value = CurrentCommand.DefendSelf;
                break;
            case AllyCommand.DefendPlayer:
                currentCommand.Value = CurrentCommand.DefendPlayer;
                break;
            case AllyCommand.AssistPlayer:
                currentCommand.Value = CurrentCommand.AssistPlayer;
                break;
        }


        return Status.Running;
    }



}

