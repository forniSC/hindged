using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int score = Stage.GetInstance().Coziness;
        GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }
}
