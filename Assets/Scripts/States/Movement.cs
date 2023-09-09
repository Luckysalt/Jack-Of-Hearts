using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : State
{
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        PlayerMovement();
        FaceMovementDirection();
    }
    private void PlayerMovement()
    {
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
        if (!actor.moveDirection.Equals(Vector3.zero))
        {
            Quaternion rotation = Quaternion.LookRotation(actor.moveDirection);
            rotation.x = 0f;
            rotation.z = 0f;
            actor.rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, actor.rotationSpeed * Time.fixedDeltaTime));
        }
    }
}