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
    [Header("Health")]
    [SerializeField] private float m_maxHealth = 100f;
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

    [Header("Dash")]
    [SerializeField] private float m_dashTime = 0.5f;
    [SerializeField] private float m_dashCoolDown = 0f;
    [SerializeField] private float m_dashSpeed = 0.7f;
    [SerializeField] private float m_dashDistance = 5f;

    [Header("Attack")]
    [SerializeField] private float m_attackTime = 0.6f;
    [SerializeField] private float m_attackCoolDown = 0f;
    [SerializeField] private float m_attackDashSpeed = 10f;
    [SerializeField] private float m_attackDashDistance = 3f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask m_groundDetectionAffected;
    [SerializeField] private float m_groundDetectionRayDistance;
    [SerializeField] private float m_heightAboveGround;
    [SerializeField] private float m_springStrength;
    [SerializeField] private float m_springDamper;

    [Header("Anti Clipping")]
    [SerializeField] private LayerMask m_antiClippingDetection;
    #endregion

    #region Getters
    //Health
    public float maxHealth => m_maxHealth;
    //Movement
    public float rotationSpeed => m_rotationSpeed;
    public float maxSpeed => m_maxSpeed;
    public float acceleration => m_acceleration;
    public float maxAccelForce => m_maxAccelForce;
    public AnimationCurve accelerationFactorFromDot => m_accelerationFactorFromDot;
    public AnimationCurve maxAccelerationForceFactorFromDot => m_maxAccelerationForceFactorFromDot;
    public Vector3 moveForceScale => m_moveForceScale;
    public float speedFactor => m_speedFactor;
    public float maxAccelForceFactor => m_maxAccelForceFactor;
    //Dash
    public float dashTime => m_dashTime;
    public float dashSpeed => m_dashSpeed;
    public float dashCoolDown => m_dashCoolDown;
    public float dashDistance => m_dashDistance;
    //Attack
    public float attackTime => m_attackTime;
    public float attackCoolDown => m_attackCoolDown;
    public float attackDashSpeed => m_attackDashSpeed;
    public float attackDashDistance => m_attackDashDistance;
    //Ground Detection
    public LayerMask groundDetectionAffected => m_groundDetectionAffected;
    public float groundDetectionRayDistance => m_groundDetectionRayDistance;
    public float heightAboveGround => m_heightAboveGround;
    public float springStrength => m_springStrength;
    public float springDamper => m_springDamper;
    public LayerMask antiClippingDetection => m_antiClippingDetection;
    #endregion 

    #region Events
    //States
    [HideInInspector] public UnityEvent OnIdle = new UnityEvent();
    [HideInInspector] public UnityEvent OnWalk = new UnityEvent();
    [HideInInspector] public UnityEvent OnDash = new UnityEvent();
    [HideInInspector] public UnityEvent OnAttack = new UnityEvent();
    //Animations
    [HideInInspector] public UnityEvent OnIdleAnimation = new UnityEvent();
    [HideInInspector] public UnityEvent OnWalkAnimation = new UnityEvent();
    [HideInInspector] public UnityEvent<bool> OnDashAnimation = new UnityEvent<bool>();
    [HideInInspector] public UnityEvent<bool, int> OnAttackAnimation = new UnityEvent<bool, int>();
    //SFX
    [HideInInspector] public UnityEvent<EventReference, Vector3> OnPlayOneShot = new UnityEvent<EventReference, Vector3>();

    #endregion
}