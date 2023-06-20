using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    static Stage m_instance;

    Converger[] m_waitingConvergers = new Converger[0];
    BlockParent m_waitingBlockParent = null;

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_waitingConvergers.Length > 0)
        {
            bool isWaiting = false;
            foreach (Converger c in m_waitingConvergers)
            {
                isWaiting |= c.IsConverging;
            }
            if (!isWaiting)
            {
                // Fix the intepolation
                foreach (Converger c in m_waitingConvergers)
                {
                    c.gameObject.transform.SetPositionAndRotation(c.TargetPos, Quaternion.Euler(0, 0, c.TargetAngle));
                }
                m_waitingBlockParent.GetComponent<ChangeStatic>().setStatic(true);

                m_waitingConvergers = new Converger[0];
                m_waitingBlockParent = null;
            }
        }
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
        BlockParent blockParent = blockRoot.GetComponent<BlockParent>();
        m_waitingConvergers = blockParent.ConvergeToGrid();
        m_waitingBlockParent = blockParent;
    }
}
