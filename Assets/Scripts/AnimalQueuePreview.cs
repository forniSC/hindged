using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalQueuePreview : MonoBehaviour
{
    public Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Sprite> queue = AnimalQueue.GetInstance().GetSprites();
        for (int i=0; i<images.Length; ++i)
        {
            if (queue.Count > i)
            {
                images[i].sprite = queue[i];
                images[i].SetNativeSize();
                images[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                images[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
