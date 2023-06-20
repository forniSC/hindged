using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalQueue : MonoBehaviour
{
    public List<GameObject> m_list;
    List<Sprite> m_sprites = new List<Sprite>();

    static AnimalQueue m_instance;

    private void Awake()
    {
        m_instance = this;
    }

    public static AnimalQueue GetInstance()
    {
        return m_instance;
    }

    public List<GameObject> GetList()
    {
        return m_list;
    }

    public List<Sprite> GetSprites()
    {
        return m_sprites;
    }

    public void RefreshSprites()
    {
        m_sprites.Clear();
        foreach (GameObject g in m_list)
        {
            m_sprites.Add(Instantiate(g.GetComponent<QueuePreview>().Sprite));
        }
    }

    public GameObject Pop()
    {
        if (m_list.Count == 0)
            return null;

        GameObject result = m_list[0];
        m_list.RemoveAt(0);
        RefreshSprites();
        return result;
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
