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

    public void UpdateMaterial(Material material, string materialTypeComponents)
    {
        foreach (GameObject Furniture in Furnitures)
        {
            foreach (Transform component in Furniture.transform.Find(materialTypeComponents))
            {
                component.gameObject.GetComponent<Renderer>().material = material;
            }
        }
    }
}
