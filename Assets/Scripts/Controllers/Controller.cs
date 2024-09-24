using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Idle))] //needs atleast 1 state, idle is default
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    //Base
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
    [SerializeField] protected ActorSO m_actor; // toDO:maybe use Assetbundle to load ActorSO
    protected Rigidbody m_rigidBody;

    //Player
    protected Keybinds m_inputActions;

    protected virtual void Awake()
    {
        currentState = GetComponent<Idle>(); //sets idle state to default
        rigidBody = GetComponent<Rigidbody>();
    }

    public ControllerType controllerType { get { return m_controllerType; } set { m_controllerType = value; } }
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }
    public ActorSO actor { get { return m_actor; } set { m_actor = value; } }
    public Rigidbody rigidBody { get { return m_rigidBody; } set { m_rigidBody = value; } }
    public Keybinds inputActions { get { return m_inputActions; } set { m_inputActions = value; } }
}
