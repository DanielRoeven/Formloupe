using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    public GameObject FurnitureTracker;
    public GameObject[] Furnitures;
    private GameObject Checkbox;
    private int VisibleFurniture = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Furniture in Furnitures)
        {
            Furniture.SetActive(false);
        }

        Furnitures[VisibleFurniture].SetActive(true);

        Checkbox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Checkbox.transform.parent = FurnitureTracker.transform;
        Checkbox.transform.position = new Vector3(0.05f, 0.2f, 0.05f);
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

    public void UpdateMaterial(Material Material, string MaterialTypeComponents)
    {
        if (MaterialTypeComponents != "")
        {
            foreach (GameObject Furniture in Furnitures)
            {
                Checkbox.SetActive(false);

                foreach (Transform component in Furniture.transform.Find(MaterialTypeComponents))
                {
                    component.gameObject.GetComponent<Renderer>().material = Material;
                }
            }
        }
    }
}
