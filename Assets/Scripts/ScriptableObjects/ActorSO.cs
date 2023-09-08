using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class ActorSO : ScriptableObject
{
    /// <summary>
    /// The Actor Scriptable Object (short: ActorSO) resembles a Player,Enemy or NPC 
    /// it acts as the middle man for independent modular components like Controllers, States and (Animation/Sound/Health/...)Managers
    /// and contains parameters and events they can subscribe to
    /// </summary>
    #region Properties
    [Header("State")]
    [SerializeField] private State m_currentState;

    [Header("Movement")]
    [SerializeField] private Vector3 m_moveDirection;
    [SerializeField] private float m_maxSpeed = 8f;
    [SerializeField] private float m_acceleration = 200f;
    [SerializeField] private float m_maxAccelForce = 150f;
    [SerializeField] private AnimationCurve m_accelerationFactorFromDot;
    [SerializeField] private AnimationCurve m_maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 m_moveForceScale = new Vector3(1f, 0f, 1f);
    private float m_speedFactor = 1f;
    private float m_maxAccelForceFactor = 1f;
    private Vector3 m_goalVel = Vector3.zero;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask m_groundDetectionAffected;
    [SerializeField] private float m_groundDetectionRayDistance;
    [SerializeField] private float m_heightAboveGround;
    [SerializeField] private float m_springStrength;
    [SerializeField] private float m_springDamper;

    //Rigidbody
    private Rigidbody m_rigidbody;
    #endregion

    #region Getters/Setters
    //State
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }
    //Movement
    public Vector3 moveDirection { get { return m_moveDirection;  } set { m_moveDirection = value; } }
    public float maxSpeed { get { return m_maxSpeed; } set { m_maxSpeed = value; } }
    public float acceleration { get { return m_acceleration; } set { m_acceleration = value; } }
    public float maxAccelForce { get { return m_maxAccelForce; } set { m_maxAccelForce = value; } }
    public AnimationCurve accelerationFactorFromDot { get { return m_accelerationFactorFromDot; } set { m_accelerationFactorFromDot = value; } }
    public AnimationCurve maxAccelerationForceFactorFromDot { get { return m_maxAccelerationForceFactorFromDot; } set { m_maxAccelerationForceFactorFromDot = value; } }
    public Vector3 moveForceScale { get { return m_moveForceScale; } set { m_moveForceScale = value; } }
    public float speedFactor { get { return m_speedFactor; } set { m_speedFactor = value; } }
    public float maxAccelForceFactor { get { return m_maxAccelForceFactor; } set { m_maxAccelForceFactor = value; } }
    public Vector3 goalVel { get { return m_goalVel; } set { m_goalVel = value; } }

    //Ground Detection
    public LayerMask groundDetectionAffected { get { return m_groundDetectionAffected; } set { m_groundDetectionAffected = value; } }
    public float groundDetectionRayDistance { get { return m_groundDetectionRayDistance; } set { m_groundDetectionRayDistance = value; } }
    public float heightAboveGround { get { return m_heightAboveGround; } set { m_heightAboveGround = value; } }
    public float springStrength { get { return m_springStrength; } set { m_springStrength = value; } }
    public float springDamper { get { return m_springDamper; } set { m_springDamper = value; } }
    //Rigidbody
    public Rigidbody rigidbody { get { return m_rigidbody; } set { m_rigidbody = value; } }
    #endregion

    #region Events
    //States
    [HideInInspector]public UnityEvent OnIdle = new UnityEvent();
    [HideInInspector]public UnityEvent OnWalk = new UnityEvent();
    //Animations
    [HideInInspector] public UnityEvent OnIdleAnim = new UnityEvent();
    [HideInInspector] public UnityEvent OnWalkAnim = new UnityEvent();
    #endregion
}