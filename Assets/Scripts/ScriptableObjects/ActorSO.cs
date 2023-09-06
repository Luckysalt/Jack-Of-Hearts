using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class ActorSO : ScriptableObject
{
    //Properties
    [Header("General")]
    [SerializeField] private Vector2 m_lookDirection;
    [SerializeField] private Vector3 m_Position;
    [SerializeField] private Vector3 m_Movement;

    [Header("Input")]
    [SerializeField]private Vector2 m_inputAxis;
    private InputAction.CallbackContext m_callbackContext;

    [Header("State")]
    [SerializeField] private State m_currentState;

    //Getters and Setters
    public Vector2 lookDirection { get { return m_lookDirection; } set { m_lookDirection = value; } }
    public Vector3 position { get { return m_Position; } set { m_Position = value; } }
    public Vector3 movement { get { return m_Movement; } set { m_Movement = value; } }
    public Vector2 inputAxis { get { return m_inputAxis; } set { m_inputAxis = value; } }
    public InputAction.CallbackContext callbackContext { get { return m_callbackContext; } set { m_callbackContext = value; } }
    public State currentState { get { return m_currentState; } set { m_currentState = value; } }

    //Events
    [HideInInspector]public UnityEvent OnIdle = new UnityEvent();
    [HideInInspector]public UnityEvent OnWalk = new UnityEvent();
}