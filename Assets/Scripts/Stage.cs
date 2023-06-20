using System;
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

    public void RecalculateInteractions()
    {
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;
        BlockParent[] parents = GetComponentsInChildren<BlockParent>();
        foreach (BlockParent parent in parents)
        {
            foreach (Transform container in parent.transform)
            {
                foreach (Transform t in container)
                {
                    Vector3 pos = t.position;
                    minX = Math.Min(minX, Mathf.RoundToInt(pos.x));
                    maxX = Math.Max(maxX, Mathf.RoundToInt(pos.x + 1));
                    minY = Math.Min(minY, Mathf.RoundToInt(pos.y));
                    maxY = Math.Max(maxY, Mathf.RoundToInt(pos.y + 1));
                }
            }
        }
        if (minX < int.MaxValue)
        {
            //Debug.Log("Bounding box: " + minX + "," + minY + " --- " + maxX + "," + maxY);
            BlockParent[,] grid = new BlockParent[maxX - minX, maxY - minY];
            foreach (BlockParent parent in parents)
            {
                foreach (Transform container in parent.transform)
                {
                    foreach (Transform t in container)
                    {
                        Vector3 pos = t.position;
                        int x = Mathf.RoundToInt(pos.x) - minX;
                        int y = Mathf.RoundToInt(pos.y) - minY;
                        grid[x, y] = parent;
                    }
                }
            }
            /*
            for (int j = maxY - minY - 1; j >= 0; --j)
            {
                String s = "";
                {
                    for (int i = 0; i < maxX - minX; ++i)
                    if (grid[i, j] != null)
                    {
                        s += "#";
                    }
                    else
                    {
                        s += "o";
                    }
                }
                Debug.Log(s);
            }*/
        }
    }
}
