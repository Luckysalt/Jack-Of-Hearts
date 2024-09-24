using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    //Base
    [SerializeField] protected ActorSO m_actor; // toDO:maybe use Assetbundle to load ActorSO
    public enum ControllerType
    {
        Player,
        Enemy,
        Boss,
        NPC,
        Companion,
    }
    protected ControllerType m_controllerType;
    protected State m_currentState;

    protected Rigidbody m_rigidBody;

    protected Vector3 m_aimDirection;
    protected Vector3 m_moveDirection;
    protected Vector3 m_lookDirection;

    protected Vector3 m_goalVel = Vector3.zero;

    //Player
    public enum InputBuffer
    {
        Empty,
        Attack,
        Dash,
    }
    protected InputBuffer m_playerInputBuffer = InputBuffer.Empty;
    protected Keybinds m_inputActions;

    protected LayerMask m_aimPlaneLayerMask;

    protected int m_currentAttackID = 0;

    protected virtual void Awake()
    {
        currentState = GetComponent<Idle>();
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public ControllerType controllerType { get { return m_controllerType; } set { m_controllerType = value; } }
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }
    public ActorSO actor { get { return m_actor; } set { m_actor = value; } }
    public Rigidbody rigidBody { get { return m_rigidBody; } set { m_rigidBody = value; } }
    public Keybinds inputActions { get { return m_inputActions; } set { m_inputActions = value; } }
    public InputBuffer playerInputBuffer { get { return m_playerInputBuffer; } set { m_playerInputBuffer = value; } }
    public LayerMask aimPlaneLayerMask { get { return m_aimPlaneLayerMask; } set { m_aimPlaneLayerMask = value; } }
    public Vector3 aimDirection { get { return m_aimDirection; } set { m_aimDirection = value; } }
    public Vector3 moveDirection { get { return m_moveDirection; } set { m_moveDirection = value; } }
    public Vector3 lookDirection { get { return m_lookDirection; } set { m_lookDirection = value; } }
    public int currentAttackID { get { return m_currentAttackID; } set { m_currentAttackID = value; } }
    public Vector3 goalVel { get { return m_goalVel; } set { m_goalVel = value; } }
}
