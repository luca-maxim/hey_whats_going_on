using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Rigidbody car;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int kmh = (int) (car.velocity.magnitude * 3.61);
        text.SetText(kmh + " km/h");
    }
}
