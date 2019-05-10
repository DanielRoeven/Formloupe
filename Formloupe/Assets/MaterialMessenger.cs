using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MaterialMessenger : MonoBehaviour
{
    public Material PinewoodUntreated;
    public Material PinewoodCinnamon;
    public Material PinewoodCoffee;
    public Material AluminiumGrey;
    public Material AluminiumWhite;
    public Material SteelBlack;
    public Material LeatherAsh;
    public Material LeatherCoffee;
    public Material LeatherLatte;
    public Material SuedeLatte;
    public Material CottonBeige;
    public Material CottonBlue;
    public Material AcrylicWhite;
    public Material AcrylicBlack;
    public Material AcrylicTransparent;
    public Material Base;

    public GameObject[] Trackers;

    // Lists of history of each applied material per type
    private List<Material> ActiveHardMaterials = new List<Material>();
    private List<Material> ActiveMediumMaterials = new List<Material>();
    private List<Material> ActiveSoftMaterials = new List<Material>();
    private List<Material> ActiveAccessoryMaterials = new List<Material>();

    // Type of material currently on each reader
    private string Reader0TypeComponents = "";
    private string Reader1TypeComponents = "";
    private string Reader2TypeComponents = "";
    private string Reader3TypeComponents = "";

    // Material currently on each reader
    private Material ActiveReader0Mat;
    private Material ActiveReader1Mat;
    private Material ActiveReader2Mat;
    private Material ActiveReader3Mat;

    public AudioClip metalSound;
    public AudioClip woodSound;
    public AudioClip plasticSound;
    public AudioClip fabricSound;
    public AudioClip leatherSound;

    // Use this for initialization
    void Start()
    {
        ActiveReader0Mat = Base;
        ActiveReader1Mat = Base;
        ActiveReader2Mat = Base;
        ActiveReader3Mat = Base;
    }
    // Update is called once per frame
    void Update()
    {

    }

    // Invoked when a line of data is received from the serial device
    void OnMessageArrived(string msg)
    {
        Debug.Log(msg);
        // Get reader nr. from serial message
        string Reader = msg.Substring(1, 1);
        
        // Get material id, material, and material type from serial message  
        string Id = IDFromMessage(msg);
        Material Material = MaterialFromID(Id);
        string MaterialTypeComponents = MaterialTypeComponentsFromID(Id);

        // If the material id is 0, then swatch was removed
        if (Id == "0")
        {
            // Find the previous material and material type from reader nr.
            Material PreviousMaterialOfReader = GetReaderMaterial(Reader);
            string PreviousMaterialTypeComponents = GetPreviousMaterialTypeComponentsOfReader(Reader);

            // Set the material of reader to base
            SetReaderMaterial(Reader, Base);

            // Remove the previous material from the history of material type
            RemovePreviousMaterialFromActiveMaterials(PreviousMaterialOfReader, PreviousMaterialTypeComponents);

            // Find the most recently added material of that material type
            Material PreviousMaterial = GetPreviousMaterialOfMaterialTypeComponents(PreviousMaterialTypeComponents);
 
            // Set the material type to the most recently added material of that type
            // TODO: make interface in case of clash
            SetMaterial(PreviousMaterial, PreviousMaterialTypeComponents, PreviousMaterialOfReader);
        }
        else
        {
            // Save the active material and material type per reader for use later
            SetReaderMaterial(Reader, Material);
            SetReaderMaterialTypeComponents(Reader, MaterialTypeComponents);

            // Add the new material to the history of the material type
            AddActiveMaterial(Material, MaterialTypeComponents);

            // Set the material type to the new material
            // TODO: make interface in case of clash
            SetMaterial(Material, MaterialTypeComponents);
        }

        PlaySound(Id);
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        //Debug.Log(success ? "Device connected" : "Device disconnected");
    }

    // Sets the current material for a given reader
    void SetReaderMaterial(string Reader, Material Material)
    {
        switch (Reader)
        {
            case "0":
                ActiveReader0Mat = Material;
                return;
            case "1":
                ActiveReader1Mat = Material;
                return;
            case "2":
                ActiveReader2Mat = Material;
                return;
            case "3":
                ActiveReader3Mat = Material;
                return;
            default:
                return;
        }
    }
    
    // Gets the current material for a given reader
    Material GetReaderMaterial(string Reader)
    {
        switch (Reader)
        {
            case "0":
                return ActiveReader0Mat;
            case "1":
                return ActiveReader1Mat;
            case "2":
                return ActiveReader2Mat;
            case "3":
                return ActiveReader3Mat;
            default:
                return Base;
        }
    }

    // Sets the material type for a given reader
    void SetReaderMaterialTypeComponents(string Reader, string MaterialTypeComponents)
    {
        switch (Reader)
        {
            case "0":
                Reader0TypeComponents = MaterialTypeComponents;
                return;
            case "1":
                Reader1TypeComponents = MaterialTypeComponents;
                return;
            case "2":
                Reader2TypeComponents = MaterialTypeComponents;
                return;
            case "3":
                Reader3TypeComponents = MaterialTypeComponents;
                return;
            default:
                return;
        }
    }

    // Gets the material type of a given reader
    string GetPreviousMaterialTypeComponentsOfReader(string Reader)
    {
        Debug.Log("GetPreviousMaterialTypeComponentsOfReader:" + Reader);

        switch (Reader)
        {
            case "0":
                return Reader0TypeComponents;
            case "1":
                return Reader1TypeComponents;
            case "2":
                return Reader2TypeComponents;
            case "3":
                return Reader3TypeComponents;
            default:
                return "";
        }
    }

    // Add material to history of material type
    void AddActiveMaterial(Material Material, string MaterialTypeComponents)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                ActiveHardMaterials.Add(Material);
                return;
            case "MediumComponents":
                ActiveMediumMaterials.Add(Material);
                return;
            case "SoftComponents":
                ActiveSoftMaterials.Add(Material);
                return;
            case "AccessoryComponents":
                ActiveAccessoryMaterials.Add(Material);
                return;
            default:
                return;
        }
    }

    // Remove material from history of material type
    void RemovePreviousMaterialFromActiveMaterials(Material Material, string MaterialTypeComponents)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                ActiveHardMaterials.Remove(Material);
                return;
            case "MediumComponents":
                ActiveMediumMaterials.Remove(Material);
                return;
            case "SoftComponents":
                ActiveSoftMaterials.Remove(Material);
                return;
            case "AccessoryComponents":
                ActiveAccessoryMaterials.Remove(Material);
                return;
            default:
                return;
        }
    }

    // Get most recently added material in history of material type
    Material GetPreviousMaterialOfMaterialTypeComponents(string MaterialTypeComponents)
    {
        switch (MaterialTypeComponents)
        {
            case "HardComponents":
                if (ActiveHardMaterials.Count > 0)
                {
                    return ActiveHardMaterials[ActiveHardMaterials.Count - 1];
                }
                else
                {
                    return Base;
                }
            case "MediumComponents":
                if (ActiveMediumMaterials.Count > 0)
                {
                    return ActiveMediumMaterials[ActiveMediumMaterials.Count - 1];
                }
                else
                {
                    return Base;
                }
            case "SoftComponents":
                if (ActiveSoftMaterials.Count > 0)
                {
                    return ActiveSoftMaterials[ActiveSoftMaterials.Count - 1];
                }
                else
                {
                    return Base;
                }
            case "AccessoryComponents":
                if (ActiveAccessoryMaterials.Count > 0)
                {
                    return ActiveAccessoryMaterials[ActiveAccessoryMaterials.Count - 1];
                }
                else
                {
                    return Base;
                }
            default:
                return Base;
        }
    }

    // Set material type components to a given material
    void SetMaterial(Material Material, string MaterialTypeComponents, Material OverrideMateral = null)
    {
        foreach (GameObject Tracker in Trackers)
        {
            Tracker.GetComponent<FurnitureController>().UpdateMaterial(Material, MaterialTypeComponents, OverrideMateral);
        }
    }

    // Parses ID from serial message
    string IDFromMessage(string msg)
    {
        string id = msg.Substring(3, msg.Length - 3);
        id = id.Substring(0, id.Length - 1);
        return id;
    }

    // Finds material for a given id
    Material MaterialFromID(string id)
    {
        switch (id)
        {
            case "188718099":
                return PinewoodUntreated;
            case "23321231217":
                return PinewoodCinnamon;
            case "10114931217":
                return PinewoodCoffee;
            case "4314431217":
                return AluminiumGrey;
            case "176532217":
                return AluminiumWhite;
            case "498731217":
                return SteelBlack;
            case "1715938131":
                return LeatherAsh;
            case "15413242131":
                return LeatherCoffee;
            case "6510335217":
                return LeatherLatte;
            case "2027235217":
                return SuedeLatte;
            case "8212131217":
                return CottonBeige;
            case "21816530217":
                return CottonBlue;
            case "13711735217":
                return AcrylicWhite;
            case "223031217":
                return AcrylicBlack;
            case "15112031217":
                return AcrylicTransparent;
            case "0":
                return Base;
            default:
                return Base;
        }
    }

    // Finds material type for a given id
    string MaterialTypeComponentsFromID(string id)
    {
        switch (id)
        {
            case "188718099":
                return "HardComponents"; //PinewoodUntreated;
            case "23321231217":
                return "HardComponents"; //PinewoodCinnamon;
            case "10114931217":
                return "HardComponents"; //PinewoodCoffee;
            case "4314431217":
                return "AccessoryComponents"; //AluminiumGrey;
            case "176532217":
                return "AccessoryComponents"; //AluminiumWhite;
            case "498731217":
                return "AccessoryComponents"; //SteelBlack;
            case "1715938131":
                return "MediumComponents"; //LeatherAsh;
            case "15413242131":
                return "MediumComponents"; //LeatherCoffee;
            case "6510335217":
                return "MediumComponents"; //LeatherLatte;
            case "2027235217":
                return "MediumComponents"; //SuedeLatte;
            case "8212131217":
                return "SoftComponents"; //CottonBeige;
            case "21816530217":
                return "SoftComponents"; //CottonBlue;
            case "13711735217":
                return "HardComponents"; //AcrylicWhite;
            case "223031217":
                return "HardComponents"; //AcrylicBlack;
            case "15112031217":
                return "HardComponents"; //AcrylicTransparent;
            case "0":
                return "HardComponents"; //Base;
            default:
                return "HardComponents"; //Base;
        }
    }

    private void PlaySound(string id)
    {
        AudioSource audio = GetComponent<AudioSource>();
        switch (id)
        {
            case "188718099":
                audio.clip = woodSound; //PinewoodUntreated;
                audio.Play();
                break;
            case "23321231217":
                audio.clip = woodSound; //PinewoodCinnamon;
                audio.Play();
                break;
            case "10114931217":
                audio.clip = woodSound; //PinewoodCoffee;
                audio.Play();
                break;
            case "4314431217":
                audio.clip = metalSound; //AluminiumGrey;
                audio.Play();
                break;
            case "176532217":
                audio.clip = metalSound; //AluminiumWhite;
                audio.Play();
                break;
            case "498731217":
                audio.clip = metalSound; //SteelBlack;
                audio.Play();
                break;
            case "1715938131":
                audio.clip = leatherSound; //LeatherAsh;
                audio.Play();
                break;
            case "15413242131":
                audio.clip = leatherSound; //LeatherCoffee;
                audio.Play();
                break;
            case "6510335217":
                audio.clip = leatherSound; //LeatherLatte;
                audio.Play();
                break;
            case "2027235217":
                audio.clip = leatherSound; //SuedeLatte;
                audio.Play();
                break;
            case "8212131217":
                audio.clip = fabricSound; //CottonBeige;
                audio.Play();
                break;
            case "21816530217":
                audio.clip = fabricSound; //CottonBlue;
                audio.Play();
                break;
            case "13711735217":
                audio.clip = plasticSound; //AcrylicWhite;
                audio.Play();
                break;
            case "223031217":
                audio.clip = plasticSound; //AcrylicBlack;
                audio.Play();
                break;
            case "15112031217":
                audio.clip = plasticSound; //AcrylicTransparent;
                audio.Play();
                break;
            case "0":
                break;
            default:
                break;
        }
        }
    }