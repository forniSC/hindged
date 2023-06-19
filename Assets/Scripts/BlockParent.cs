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
}
