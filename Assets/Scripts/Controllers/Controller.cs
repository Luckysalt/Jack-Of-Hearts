using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected ActorSO m_actor; // toDO:maybe use Assetbundle to load ActorSO
    protected State m_currentState;
    protected Rigidbody m_rigidBody;

    protected Vector3 m_aimDirection;
    protected Vector3 m_moveDirection;
    protected Vector3 m_lookDirection;

    protected Vector3 m_goalVel = Vector3.zero;
    protected bool m_wantsToMove;

    protected float m_currentHealth;

    [HideInInspector] public UnityEvent<float,float> onHealthUIChange= new UnityEvent<float,float>();
    [HideInInspector] public UnityEvent OnHurt = new UnityEvent();
    //Player
    protected int m_currentAttackID = 0;// Todo: move to Weapon class

    protected virtual void Awake()
    {
        currentState = GetComponent<Idle>();
        wantsToMove = false;
        m_rigidBody = GetComponent<Rigidbody>();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        onHealthUIChange.Invoke(currentHealth, actor.maxHealth);
    }
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }
    public ActorSO actor { get { return m_actor; } set { m_actor = value; } }
    public Rigidbody rigidBody { get { return m_rigidBody; } set { m_rigidBody = value; } }
    public Vector3 aimDirection { get { return m_aimDirection; } set { m_aimDirection = value; } }
    public Vector3 moveDirection { get { return m_moveDirection; } set { m_moveDirection = value; } }
    public Vector3 lookDirection { get { return m_lookDirection; } set { m_lookDirection = value; } }
    public int currentAttackID { get { return m_currentAttackID; } set { m_currentAttackID = value; } }
    public Vector3 goalVel { get { return m_goalVel; } set { m_goalVel = value; } }
    public bool wantsToMove { get { return m_wantsToMove; } set { m_wantsToMove = value; } }
    public float currentHealth { get { return m_currentHealth; } set { m_currentHealth = value; } }
}
