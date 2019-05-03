using System.Collections;
using System.Collections.Generic;
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

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        Debug.Log("Arrived: " + msg);

        string id = IDFromMessage(msg);
        Material newMaterial = MaterialFromID(id);

        foreach (GameObject Tracker in Trackers)
        {
            Tracker.GetComponent<FurnitureController>().UpdateMaterial(newMaterial);
        }
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }

    string IDFromMessage(string msg)
    {
        string id = msg.Substring(3, msg.Length - 3);
        id = id.Substring(0, id.Length - 1);
        return id;
    }

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
}