using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(PlayerStateManager player);

    public abstract void UpdateState(PlayerStateManager player);

    public abstract void OnCollisionEnter(PlayerStateManager player);

    public abstract void OnTriggerEnter(PlayerStateManager player);
}
