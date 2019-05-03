using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed1Controller : MonoBehaviour
{
    public GameObject Bed1Sheets1;
    public GameObject Bed1Sheets2;
    public GameObject Bed1Pillow1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject[] getSoftComponents()
    {
        GameObject[] softComponents = { Bed1Sheets1, Bed1Sheets2, Bed1Pillow1 };
        return softComponents;
    }
}
