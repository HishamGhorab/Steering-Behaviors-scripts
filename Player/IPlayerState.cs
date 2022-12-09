using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public bool Active { get; set; }
    public PlayerStateController.PlayerState MyState { get; set;}

    public void SubscribeToPlayerStateController(IPlayerState iPlayerState);
}
