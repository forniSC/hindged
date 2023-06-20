using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    static Level m_instance;

    public static Level GetInstance()
    {
        return m_instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetEntryPoint()
    {
        EntryPoint p = GetComponentInChildren<EntryPoint>();
        return p.transform.position;
    }
}
