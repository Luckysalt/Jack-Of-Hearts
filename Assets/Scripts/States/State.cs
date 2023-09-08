using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected ActorSO actor;
    protected virtual void SetToCurrentState() 
    {
        actor.currentState = this;
        StartState();
    }
    protected virtual void StartState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() 
    {
        GroundDetection();
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

            //Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.red);
        }
        else
        {
            if (!actor.rigidbody.useGravity) actor.rigidbody.useGravity = true;
        }
    }
}