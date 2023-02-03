using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ControlMode
{
    Normal,
    Root
}

public class Master : MonoBehaviour
{
    PlayerBehavior player;
    Root root;

    public ControlMode target;
}

public abstract class Unit : SerializedMonoBehaviour
{
    public KeyCode Key_Down;
    public KeyCode Key_Up;
    public KeyCode Key_Right;
    public KeyCode Key_Left;

    public KeyCode Key_Interact;    

    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}