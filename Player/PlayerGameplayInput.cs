using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplayInput : MonoBehaviour
{
    public Vector3 MousePosition { get; private set; }
    private PlayerStateController _playerStateController;

    void Start()
    {
        _playerStateController = GetComponent<PlayerStateController>();
    }
    
    void Update()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.F))
        {
            _playerStateController.InformInput(KeyCode.F);
        }
    }
    
    public Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
