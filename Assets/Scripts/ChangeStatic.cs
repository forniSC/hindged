using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatic : MonoBehaviour
{
    Rigidbody2D[] m_rigidBodies;

    void Awake()
    {
        m_rigidBodies = GetComponentsInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setStatic(bool isStatic)
    {
        foreach (Rigidbody2D b in m_rigidBodies)
        {
            b.bodyType = isStatic ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        }
    }
}
