using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class AISeekSteering : MonoBehaviour
{
    public GameObject player;
    
    public float MaxVelocity;
    public float SeekForce = 0.1f;
    public float slowingRadius = 2.5f;

    [Header("Wandering")] 
    public float wanderCircleRadius;
    public float offset;
    
    private Vector3 velocity;
    private Vector3 target;

    private Vector3 desiredVelocity;
    private Vector3 steering;
    
    //wandering
    private Vector3 displacement;


    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //Vector3 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        target = player.transform.position;
        target.z = transform.position.z;
        
        //Persue
        velocity = Vector3.ClampMagnitude(velocity + Wander(), MaxVelocity);

        transform.position += velocity * Time.deltaTime;
        transform.rotation = RotateTowardsMyVelocity();
    }

    Quaternion RotateTowardsMyVelocity()
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    Vector3 Seek(Vector3 pos)
    {
        desiredVelocity = pos - transform.position;
        float distance = desiredVelocity.magnitude;

        if (distance < slowingRadius)
        {
            desiredVelocity = Vector3.Normalize(desiredVelocity) * (MaxVelocity * (distance / slowingRadius));
        }
        else
        {
            desiredVelocity = Vector3.Normalize(desiredVelocity) * MaxVelocity;
        }
        
        steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, SeekForce);

        return steering;
    }

    Vector3 Persue()
    {
        Vector3 playerVelocity = player.GetComponent<PlayerMovement>().Velocity;
        
        float distance = Vector3.Distance(target, transform.position);
        float ahead = distance / 10;
        Vector3 futurePos = target + playerVelocity;

        return Seek(futurePos);
    }
    
    Vector3 Wander()
    {
        Vector3 position = transform.position;
        Vector3 direction = transform.right;
        
        Vector3 circleCenter = position + direction * offset;
        displacement = transform.up;

        Vector3 v = new Vector3();
        float wanderAngle = Random.Range(0, Mathf.PI * 2);
        v.x = Mathf.Cos(wanderAngle) * wanderCircleRadius;
        v.y = Mathf.Sin(wanderAngle) * wanderCircleRadius;

        displacement = circleCenter + v;

        return Seek(displacement);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        Vector3 direction = transform.right;
        
        //Gizmos.color = Color.green;
        Gizmos.DrawLine(position, position + velocity);
        
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(position, position + desiredVelocity);
        
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(position, slowingRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position + direction * offset, wanderCircleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, displacement);
        
        Debug.Log($"Position: {position}, position + direction: {position + direction * offset}");
    }
}
 