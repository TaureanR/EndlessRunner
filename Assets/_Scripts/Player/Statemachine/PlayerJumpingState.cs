using UnityEngine;

public class PlayerJumpingState : BaseState
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

    }
}
