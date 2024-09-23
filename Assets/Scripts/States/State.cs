using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected ActorSO actor;
    protected float stateLifeTime;
    protected virtual void SetToCurrentState() 
    {
        actor.currentState = this;
        StartState();
    }
    protected virtual void StartState()
    {
        if (!actor.moveDirection.Equals(Vector3.zero)) actor.lookDirection = actor.moveDirection;
    }
    protected virtual void EndState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() 
    {
        GroundDetection();
        Movement();
        FacingDirection();
    }
    ///<summary>
    /// Sets the height of the Player above ground with a spring, to avoid friction and handle slopes/steps/imperfections better
    /// Sets the gravity true if the player is airborne and false if grounded 
    /// </summary>
    private void GroundDetection()
    {
        Vector3 rayDir = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDir, out hit, actor.groundDetectionRayDistance, actor.groundDetectionAffected))
        {
            if (actor.rigidbody.useGravity) actor.rigidbody.useGravity = false;

            Vector3 vel = actor.rigidbody.velocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;

            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relativeVel = rayDirVel - otherDirVel;
            float currentHeight = hit.distance - actor.heightAboveGround;

            float springForce = (currentHeight * actor.springStrength) - (relativeVel * actor.springDamper);

            actor.rigidbody.AddForce(rayDir * springForce);

            if (hitBody != null) // applies force to rigidbodies beneath 
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }

            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.red);
        }
        else
        {
            if (!actor.rigidbody.useGravity) actor.rigidbody.useGravity = true;
        }
    }
    private void Movement()
    {
        if (actor.currentState.GetType() == typeof(Dash)) return; //if actor is dashing, skip movement
        if (actor.currentState.GetType() == typeof(Attack)) return;

        Vector3 unitVel = actor.goalVel.normalized;

        float velDot = Vector3.Dot(actor.moveDirection, unitVel);

        float accel = actor.acceleration * actor.accelerationFactorFromDot.Evaluate(velDot);

        Vector3 goalVel = actor.moveDirection * actor.maxSpeed * actor.speedFactor;

        actor.goalVel = Vector3.MoveTowards(actor.goalVel, goalVel, accel * Time.fixedDeltaTime); // ToDo: goalVel + groundVel

        Vector3 neededAccel = (actor.goalVel - actor.rigidbody.velocity) / Time.fixedDeltaTime;
        float maxAccel = actor.maxAccelForce * actor.maxAccelerationForceFactorFromDot.Evaluate(velDot) * actor.maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        actor.rigidbody.AddForceAtPosition(Vector3.Scale(neededAccel * actor.rigidbody.mass, actor.moveForceScale), transform.position);
    }
    private void FacingDirection()
    {
        float rotationSpeed;
        if (actor.currentState.GetType() == typeof(Dash)) rotationSpeed = 1000f; //if actor is dashing, instant rotation towards dash direction
        else if (actor.currentState.GetType() == typeof(Attack)) rotationSpeed = 1000f; //same for attack
        else rotationSpeed = actor.rotationSpeed;

        if (!actor.lookDirection.Equals(Vector3.zero))
        {
            Quaternion rotation = Quaternion.LookRotation(actor.lookDirection);
            rotation.x = 0f;
            rotation.z = 0f;
            actor.rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}