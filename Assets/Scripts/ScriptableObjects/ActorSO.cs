using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class ActorSO : ScriptableObject
{
    //Properties
    [Header("State")]
    [SerializeField] private State m_currentState;

    [Header("Input")]
    [SerializeField] private Vector2 m_inputAxis;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask m_groundDetectionAffected;
    [SerializeField] private float m_groundDetectionRayDistance;
    [SerializeField] private float m_heightAboveGround;
    [SerializeField] private float m_springStrength;
    [SerializeField] private float m_springDamper;

    private Rigidbody m_rigidbody;

    //Getters and Setters
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }
    public Vector2 inputAxis { get { return m_inputAxis; } set { m_inputAxis = value; } }
    public LayerMask groundDetectionAffected { get { return m_groundDetectionAffected; } set { m_groundDetectionAffected = value; } }
    public float groundDetectionRayDistance { get { return m_groundDetectionRayDistance; } set { m_groundDetectionRayDistance = value; } }
    public float heightAboveGround { get { return m_heightAboveGround; } set { m_heightAboveGround = value; } }
    public float springStrength { get { return m_springStrength; } set { m_springStrength = value; } }
    public float springDamper { get { return m_springDamper; } set { m_springDamper = value; } }
    public Rigidbody rigidbody { get { return m_rigidbody; } set { m_rigidbody = value; } }

    //Events
    [HideInInspector]public UnityEvent OnIdle = new UnityEvent();
    [HideInInspector]public UnityEvent OnWalk = new UnityEvent();
}