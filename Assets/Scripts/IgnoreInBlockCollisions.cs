using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreInBlockCollisions : MonoBehaviour
{
    Collider2D[] m_subColliders;

    void Awake()
    {
        m_subColliders = GetComponentsInChildren<Collider2D>();
        for (int i=0; i < m_subColliders.Length; ++i)
        {
            for (int j = 0; j < m_subColliders.Length; ++j)
            {
                if (i == j) continue;
                Physics2D.IgnoreCollision(m_subColliders[i], m_subColliders[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
