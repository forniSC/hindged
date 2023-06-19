using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    static Stage m_instance;

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Stage GetInstance()
    {
        return m_instance;
    }

    public void OnBlockStartMove(GameObject blockRoot)
    {
        ChangeStatic staticController = blockRoot.GetComponent<ChangeStatic>();
        staticController.setStatic(false);
    }

    public void OnBlockEndMove(GameObject blockRoot)
    {
        ChangeStatic staticController = blockRoot.GetComponent<ChangeStatic>();
        staticController.setStatic(true);
    }
}
