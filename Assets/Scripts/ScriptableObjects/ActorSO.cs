using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using FMODUnity;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
/// <summary>
/// The Actor Scriptable Object (short: ActorSO) resembles a Player,Enemy or NPC 
/// it acts as the middle man for independent modular components like Controllers, States and (Animation/Sound/Health/...)Managers
/// and contains parameters and events they can subscribe to
/// </summary>
public class ActorSO : ScriptableObject
{
    #region Properties
    [Header("Movement")]
    [SerializeField] private float m_rotationSpeed = 500f;
    [SerializeField] private float m_maxSpeed = 8f;
    [SerializeField] private float m_acceleration = 200f;
    [SerializeField] private float m_maxAccelForce = 150f;
    [SerializeField] private AnimationCurve m_accelerationFactorFromDot;
    [SerializeField] private AnimationCurve m_maxAccelerationForceFactorFromDot;
    private Vector3 m_moveForceScale = new Vector3(1f, 0f, 1f);
    private float m_speedFactor = 1f;
    private float m_maxAccelForceFactor = 1f;
    private Vector3 m_goalVel = Vector3.zero;

    [Header("Dash")]
    [SerializeField] private float m_dashTime = 0.5f;
    [SerializeField] private float m_dashCoolDown = 1f;
    [SerializeField] private float m_dashSpeed = 0.7f;
    public float dashDistance = 5f;

    [Header("Attack")]
    public float attackTime = 1f;
    public float attackCoolDown = 1f;
    public float attackDashSpeed = 10f;
    public float attackDashDistance = 3f;
    public int currentAttackID = 0;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask m_groundDetectionAffected;
    [SerializeField] private float m_groundDetectionRayDistance;
    [SerializeField] private float m_heightAboveGround;
    [SerializeField] private float m_springStrength;
    [SerializeField] private float m_springDamper;

    [Header("Anti Clipping")]
    public LayerMask antiClippingDetection;
    #endregion

    #region Getters/Setters
    //Movement
    public float rotationSpeed { get { return m_rotationSpeed; } set { m_rotationSpeed = value; } }
    public float maxSpeed { get { return m_maxSpeed; } set { m_maxSpeed = value; } }
    public float acceleration { get { return m_acceleration; } set { m_acceleration = value; } }
    public float maxAccelForce { get { return m_maxAccelForce; } set { m_maxAccelForce = value; } }
    public AnimationCurve accelerationFactorFromDot { get { return m_accelerationFactorFromDot; } set { m_accelerationFactorFromDot = value; } }
    public AnimationCurve maxAccelerationForceFactorFromDot { get { return m_maxAccelerationForceFactorFromDot; } set { m_maxAccelerationForceFactorFromDot = value; } }
    public Vector3 moveForceScale { get { return m_moveForceScale; } set { m_moveForceScale = value; } }
    public float speedFactor { get { return m_speedFactor; } set { m_speedFactor = value; } }
    public float maxAccelForceFactor { get { return m_maxAccelForceFactor; } set { m_maxAccelForceFactor = value; } }
    public Vector3 goalVel { get { return m_goalVel; } set { m_goalVel = value; } }
    //Dash
    public float dashTime { get { return m_dashTime; } set { m_dashTime = value; } }
    public float dashSpeed { get { return m_dashSpeed; } set { m_dashSpeed = value; } }
    public float dashCoolDown { get { return m_dashCoolDown; } set { m_dashCoolDown = value; } }
    //Ground Detection
    public LayerMask groundDetectionAffected { get { return m_groundDetectionAffected; } set { m_groundDetectionAffected = value; } }
    public float groundDetectionRayDistance { get { return m_groundDetectionRayDistance; } set { m_groundDetectionRayDistance = value; } }
    public float heightAboveGround { get { return m_heightAboveGround; } set { m_heightAboveGround = value; } }
    public float springStrength { get { return m_springStrength; } set { m_springStrength = value; } }
    public float springDamper { get { return m_springDamper; } set { m_springDamper = value; } }
    #endregion 

    #region Events
    //States
    [HideInInspector] public UnityEvent OnIdle = new UnityEvent();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent OnWalk = new UnityEvent();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent OnDash = new UnityEvent();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent OnAttack = new UnityEvent();//<<<<<<-----------CHANGE
    //Animations
    [HideInInspector] public UnityEvent OnIdleAnim = new UnityEvent();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent OnWalkAnim = new UnityEvent();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent<bool> OnDashAnim = new UnityEvent<bool>();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent<bool, int> OnAttackAnim = new UnityEvent<bool, int>();//<<<<<<-----------CHANGE
    [HideInInspector] public UnityEvent<EventReference, Vector3> OnPlayOneShot = new UnityEvent<EventReference, Vector3>();
    #endregion
}