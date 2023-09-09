using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : State
{
    protected override void StartState()
    {
        if(!actor.moveDirection.Equals(Vector3.zero))actor.lookDirection = actor.moveDirection;
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        ActorMovement();
        FaceMovementDirection();
    }
    private void ActorMovement()
    {
        if (actor.currentState.GetType() == typeof(Dash)) return; //if actor is dashing, skip movement

        Vector3 unitVel = actor.goalVel.normalized;
        float velDot = Vector3.Dot(actor.moveDirection, unitVel);
        float accel = actor.acceleration * actor.accelerationFactorFromDot.Evaluate(velDot);
        Vector3 goalVel = actor.moveDirection * actor.maxSpeed * actor.speedFactor;
        actor.goalVel = Vector3.MoveTowards(actor.goalVel, goalVel, accel * Time.fixedDeltaTime);
        Vector3 neededAccel = (actor.goalVel - actor.rigidbody.velocity) / Time.fixedDeltaTime;
        float maxAccel = actor.maxAccelForce * actor.maxAccelerationForceFactorFromDot.Evaluate(velDot) * actor.maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        actor.rigidbody.AddForceAtPosition(Vector3.Scale(neededAccel * actor.rigidbody.mass, actor.moveForceScale), transform.position);
    }
    private void FaceMovementDirection()
    {
        float rotationSpeed;
        if (actor.currentState.GetType() == typeof(Dash)) rotationSpeed = 1000f; //if actor is dashing, instant rotation towards dash direction
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