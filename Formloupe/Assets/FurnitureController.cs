using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{

    public GameObject[] Furnitures;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMaterial(Material material)
    {
        foreach (GameObject Furniture in Furnitures)
        {
            foreach (Transform SoftComponent in Furniture.transform.Find("SoftComponents"))
            {
                SoftComponent.gameObject.GetComponent<Renderer>().material = material;
            }
        }
    }
}
