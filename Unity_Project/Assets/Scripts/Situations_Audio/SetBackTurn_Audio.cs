using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackTurn_Audio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {   
            Debug.Log("set back turn");
        }
    }
}
