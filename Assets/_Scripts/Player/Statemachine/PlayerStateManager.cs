using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    BaseState currentState;

    public PlayerGameOverState GameOverState = new PlayerGameOverState();
    public PlayerDamagedState DamagedState = new PlayerDamagedState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerIdleState IdleState = new PlayerIdleState();
    

    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
