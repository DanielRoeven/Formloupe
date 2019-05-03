using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : MonoBehaviour
{
    public GameObject Bed1;

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
        GameObject[] Bed1SoftComponents = Bed1.GetComponent<Bed1Controller>().getSoftComponents();
        return Bed1SoftComponents;
    }
}
