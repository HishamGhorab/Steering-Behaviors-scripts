using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public enum PlayerState {FreeToMove, Attacking, MoveAttacking}

    private List<IPlayerState> _subscribedStates;
    
    private PlayerState _playerState;

    void Start()
    {
        _playerState = PlayerState.FreeToMove;
    }
    
    void Update()
    {
        Debug.Log(_playerState);
    }
    public void InformInput(KeyCode keyInput)
    {
        switch (keyInput)
        {
            case KeyCode.F: 
                ChangeState(PlayerState.Attacking);
                break;
        }
    }

    public void SubscribePlayerState(IPlayerState iPlayerState)
    {
        _subscribedStates.Add(iPlayerState);
    }

    void ChangeState(PlayerState playerState)
    {
        _playerState = PlayerState.Attacking;
        foreach (IPlayerState _state in _subscribedStates)
        {
            if (_state.MyState != _playerState)
            {
                _state.Active = false;
            }
        }
    }
    

}
