using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    static Stage m_instance;

    Converger[] m_waitingConvergers = new Converger[0];
    BlockParent m_waitingBlockParent = null;
    int m_coziness = 0;

    public int Coziness { get { return m_coziness; } }

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

    public bool RecalculateInteractions()
    {
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;
        Dictionary<BlockParent, Tuple<int, int>> animalPositions = new Dictionary<BlockParent, Tuple<int, int>>();
        Dictionary<BlockParent, int> animalSizes = new Dictionary<BlockParent, int>();
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
                        if (grid[x, y] == null)
                        {
                            grid[x, y] = parent;

                            int size = 1;
                            if (animalSizes.ContainsKey(parent))
                            {
                                size = animalSizes[parent] + 1;
                            }
                            animalSizes[parent] = size;
                            animalPositions[parent] = new Tuple<int, int>(x, y);
                        }
                        else if (grid[x, y] != parent)
                        {
                            Debug.Log("Overlapping animals!");
                            return false;
                        }
                    }
                }
            }

            foreach (BlockParent parent in parents)
            {
                if (!animalPositions.ContainsKey(parent))
                {
                    Debug.Log("Animal not found!");
                    return false;
                }
                else
                {
                    Tuple<int, int> pos = animalPositions[parent];
                    bool[,] visited = new bool[maxX - minX, maxY - minY];
                    int count = Visit(pos.Item1, pos.Item2, visited, grid, parent);
                    if (count != animalSizes[parent])
                    {
                        Debug.Log("Animal is split to pieces!");
                        return false;
                    }
                }
            }
            /*for (int j = maxY - minY - 1; j >= 0; --j)
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
        return true;
    }

    private int Visit(int x, int y, bool[,] visited, BlockParent[,] grid, BlockParent animal)
    {
        if (visited[x, y]) return 0;

        int size = 1;
        visited[x, y] = true;
        if (x > 0 && grid[x - 1, y] == animal) size += Visit(x - 1, y, visited, grid, animal);
        if (y > 0 && grid[x, y - 1] == animal) size += Visit(x, y - 1, visited, grid, animal);
        if (x < grid.GetLength(0) - 1 && grid[x + 1, y] == animal) size += Visit(x + 1, y, visited, grid, animal);
        if (y < grid.GetLength(1) - 1 && grid[x, y + 1] == animal) size += Visit(x, y + 1, visited, grid, animal);
        return size;
    }
}
