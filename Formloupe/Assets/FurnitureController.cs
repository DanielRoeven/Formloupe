using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{

    public GameObject[] Furnitures;
    private int VisibleFurniture = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Furniture in Furnitures)
        {
            Furniture.SetActive(false);
        }

        Furnitures[VisibleFurniture].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        VisibleFurniture = (VisibleFurniture + 1) % Furnitures.Length;
        foreach (GameObject Furniture in Furnitures)
        {
            Furniture.SetActive(false);
        }

        Furnitures[VisibleFurniture].SetActive(true);
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
