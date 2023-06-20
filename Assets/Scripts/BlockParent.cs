using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParent : MonoBehaviour
{
    void Awake()
    {
        DragWithTouchOrClick[] dragHandlers = GetComponentsInChildren<DragWithTouchOrClick>();
        foreach (DragWithTouchOrClick p in dragHandlers)
        {
            p.m_parent = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Converger[] ConvergeToGrid()
    {
        Converger[] subConvergers = GetComponentsInChildren<Converger>();
        foreach (Converger c in subConvergers)
        {
            c.Converge();
        }
        return subConvergers;
    }
}
