using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour, IPlayerState
{
    public bool Active { get; set; }
    public PlayerStateController.PlayerState MyState { get; set; }

    [SerializeField, Range(0,100)] private float magnitude;
    [SerializeField, Range(0,100)] private float maxAcceleration;
    
    public Vector3 Velocity;

    //[SerializeField] Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);
    
    private Vector2 playerInput;

    private PlayerGameplayInput playerGameplayInput;
    private PlayerStateController playerStateController;

    void Awake()
    {
        MyState = PlayerStateController.PlayerState.FreeToMove;
        
        //temp
        Active = true;
    }

    void Start()
    {
        playerGameplayInput = GetComponent<PlayerGameplayInput>();
        playerStateController = GetComponent<PlayerStateController>();
    }

    void Update()
    {
        if (Active)
        {
            playerInput = playerGameplayInput.GetMovementInput();
        }
        
        Rotation(playerGameplayInput.MousePosition);

        Vector3 desiredVelocity = playerInput * magnitude;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        Velocity.x = 
            Mathf.MoveTowards(Velocity.x, desiredVelocity.x, maxSpeedChange);
        Velocity.y = 
            Mathf.MoveTowards(Velocity.y, desiredVelocity.y, maxSpeedChange);

         Vector3 displacement = Velocity * Time.deltaTime;
         Vector3 newPosition = transform.localPosition + displacement;
         
         transform.localPosition = newPosition;
    }

    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void Rotation(Vector3 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 moveDirection = mousePosition - transform.position;
        
        float _angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }
    
    public void SubscribeToPlayerStateController(IPlayerState iPlayerState)
    {
        playerStateController.SubscribePlayerState(iPlayerState);
    }
}
