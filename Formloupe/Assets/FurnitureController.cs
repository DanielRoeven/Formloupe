using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    public GameObject[] Furnitures;
    public GameObject Checkbox;
    private int VisibleFurniture = 0;

    private bool HardMaterialSet = false;
    private bool MediumMateralSet = false;
    private bool SoftMaterialSet = false;
    private bool AccessoryMaterialSet = false;

    public bool HasHardMaterial;
    public bool HasMediumMaterial;
    public bool HasSoftMaterial;
    public bool HasAccessoryMaterial;

    private Material HardMaterial;
    private Material MediumMaterial;
    private Material SoftMaterial;
    private Material AccessoryMaterial;

    // Start is called before the first frame update
    void Start()
    {
        HardMaterial = new Material(Shader.Find("Standard"));
        MediumMaterial = new Material(Shader.Find("Standard"));
        SoftMaterial = new Material(Shader.Find("Standard"));
        AccessoryMaterial = new Material(Shader.Find("Standard"));

        foreach (GameObject Furniture in Furnitures)
        {
            Furniture.SetActive(false);
        }

        Furnitures[VisibleFurniture].SetActive(true);
        //Checkbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("TAP FURNITURE");

        VisibleFurniture = (VisibleFurniture + 1) % Furnitures.Length;
        foreach (GameObject Furniture in Furnitures)
        {
            Furniture.SetActive(false);
        }

        Furnitures[VisibleFurniture].SetActive(true);
    }

    // Function called by material messenger, when new swatch read
    public void UpdateMaterial(Material Material, string MaterialTypeComponents, Material OverrideMaterial = null)
    {
        if (MaterialTypeComponents == "")
        {
            throw new System.ArgumentException("Invalid Material Type (Remove Swatches First)");
        }

        Debug.Log(Material.name);
        if (Material.name == "base_material" && IsApplicableMaterial(Material, MaterialTypeComponents))
        {
            SetMaterialTypeFlag(MaterialTypeComponents, false);
            SetMaterial(Material, MaterialTypeComponents);
        }
        else if(OverrideMaterial != null && MaterialTypeIsSetToMaterial(MaterialTypeComponents, OverrideMaterial))
        {
            SetMaterialTypeFlag(MaterialTypeComponents, true);
            SetMaterial(Material, MaterialTypeComponents);
        }
        else if (IsApplicableMaterial(Material, MaterialTypeComponents) && GetMaterialTypeFlag(MaterialTypeComponents))
        {
            // Material type has already been applied, ask to override with checkbox
            Checkbox.GetComponent<CheckboxController>().ShowCheckboxForMaterial(Material, MaterialTypeComponents);
        }
        else if (IsApplicableMaterial(Material, MaterialTypeComponents))
        {
            SetMaterialTypeFlag(MaterialTypeComponents, true);
            SetMaterial(Material, MaterialTypeComponents);
        }
    }

    private bool MaterialTypeIsSetToMaterial(string MaterialTypeComponents, Material Material)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                return (HasHardMaterial && HardMaterial.name == Material.name);
            case "MediumComponents":
                return (HasMediumMaterial && MediumMaterial.name == Material.name);
            case "SoftComponents":
                return (HasSoftMaterial && SoftMaterial.name == Material.name);
            case "AccessoryComponents":
                return (HasAccessoryMaterial && AccessoryMaterial.name == Material.name);
            default:
                throw new System.ArgumentException("Invalid Material Type");
        }
    }

    private bool IsApplicableMaterial(Material Material, string MaterialTypeComponents)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                return (HasHardMaterial && HardMaterial.name != Material.name);
            case "MediumComponents":
                return (HasMediumMaterial && MediumMaterial.name != Material.name);
            case "SoftComponents":
                return (HasSoftMaterial && SoftMaterial.name != Material.name);
            case "AccessoryComponents":
                return (HasAccessoryMaterial && AccessoryMaterial.name != Material.name);
            default:
                throw new System.ArgumentException("Invalid Material Type");
        }
    }

    // Called if no material set, or if checkbox tapped
    public void SetMaterial(Material Material, string MaterialTypeComponents)
    {
        if (MaterialTypeComponents != "")
        {
            Debug.Log(Material.name);
            if (Material.name == "base_material")
            {
                // If material is base, no material is chosen
                SetMaterialTypeFlag(MaterialTypeComponents, false);
            }
            else
            {
                SetMaterialTypeFlag(MaterialTypeComponents, true);
            }

            // Apply material to every furniture within the tracker
            foreach (GameObject Furniture in Furnitures)
            {
                // Apply the furniture to every component of type within the furniture
                foreach (Transform component in Furniture.transform.Find(MaterialTypeComponents))
                {
                    Debug.Log(component.gameObject);
                    component.gameObject.GetComponent<Renderer>().material = Material;
                }
            }

            switch (MaterialTypeComponents)
            {
                case "HardComponents":
                    HardMaterial = Material;
                    break;
                case "MediumComponents":
                    MediumMaterial = Material;
                    break;
                case "SoftComponents":
                    SoftMaterial = Material;
                    break;
                case "AccessoryComponents":
                    AccessoryMaterial = Material;
                    break;
                default:
                    throw new System.ArgumentException("Invalid Material Type");
            }
        }
    }

    private bool GetMaterialTypeFlag(string MaterialTypeComponents)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                return HardMaterialSet;
            case "MediumComponents":
                return MediumMateralSet;
            case "SoftComponents":
                return SoftMaterialSet;
            case "AccessoryComponents":
                return AccessoryMaterialSet;
            default:
                throw new System.ArgumentException("Invalid Material Type");
        }
    }

    public void SetMaterialTypeFlag(string MaterialTypeComponents, bool Set)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                HardMaterialSet = Set;
                return;
            case "MediumComponents":
                MediumMateralSet = Set;
                return;
            case "SoftComponents":
                SoftMaterialSet = Set;
                return;
            case "AccessoryComponents":
                AccessoryMaterialSet = Set;
                return;
            default:
                throw new System.ArgumentException("Invalid Material Type");
        }
    }
}
