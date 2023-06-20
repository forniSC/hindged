using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubblockSortLayer : MonoBehaviour
{
    public int SortOrder = 0;

    void Awake()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer p in sprites)
        {
            p.sortingOrder = SortOrder;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
