using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreInBlockCollisions : MonoBehaviour
{
  public Collider2D[] m_subColliders;

    // Start is called before the first frame update
    void Start()
    {
        int limit = m_subColliders.Length - 1;
        for (int i=0; i<limit; ++i)
        {
            Physics2D.IgnoreCollision(m_subColliders[i], m_subColliders[i+1]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
