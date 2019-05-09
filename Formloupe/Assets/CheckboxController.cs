using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckboxController : MonoBehaviour
{

    public GameObject ParentTracker;
    private Material Material;
    private string MaterialTypeComponents;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        //this.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        //StartCoroutine(Hide(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCheckboxForMaterial(Material QueuedMaterial, string QueuedMaterialTypeComponents)
    {
        Material = QueuedMaterial;
        MaterialTypeComponents = QueuedMaterialTypeComponents;
        this.gameObject.GetComponent<Renderer>().material = Material;
        this.gameObject.SetActive(true);
        StartCoroutine(Hide(10));
    }

    IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        ParentTracker.GetComponent<FurnitureController>().SetMaterialTypeFlag(MaterialTypeComponents, true);
        ParentTracker.GetComponent<FurnitureController>().SetMaterial(Material, MaterialTypeComponents);
        this.gameObject.SetActive(false);
    }
}
