using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : BaseState
{
    private PlayerController playerController;
    public override void EnterState(PlayerStateManager player)
    {
        playerController = player.GetComponent<PlayerController>();
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {

    }

    public override void OnTriggerEnter(PlayerStateManager player)
    {
        Debug.Log("Ouch");
    }
}
