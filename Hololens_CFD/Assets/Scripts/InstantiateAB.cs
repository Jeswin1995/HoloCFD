using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class InstantiateAB : MonoBehaviour
{
    string assetBundleName = "velocity_reference";
    string assetName = "Shape_IndexedFaceSetreference_velocity.py_timestep1_";
    string defaultLineMaterialName = "Default-Line";
    string saveTo = "";
    AssetBundle Bundle;
    Material[] materials;
    bool assetsLoaded = false;
    int counter = 0;
    int index = 0;
    private void Start()
    {
        StartCoroutine(SaveAndDownload("http://localhost:5000/download", assetBundleName));
    }

    //Method that initiates the call to download AssetBundles
    IEnumerator SaveAndDownload(string url, string asstName)
    {
        saveTo = Application.streamingAssetsPath + '/' + asstName;
        Debug.Log(saveTo);
        transform.gameObject.AddComponent<LoadAssetFromServer>();
        transform.GetComponent<LoadAssetFromServer>().DownloadAssets(url, asstName);
        yield return new WaitUntil(() => File.Exists(saveTo));
        Debug.Log("Download done");
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(saveTo);
        yield return bundleLoadRequest;
        Bundle = bundleLoadRequest.assetBundle;
        var loadAssetRequest = Bundle.LoadAssetAsync<GameObject>("Shape_IndexedFaceSet.001reference_velocity.py_timestep1_time50.0_");
        yield return loadAssetRequest;
        GameObject prefab = loadAssetRequest.asset as GameObject;
        GameObject Instantiatedprefab = Instantiate(prefab);

        //myLoadedAssetBundle.Unload(false);


        SetMaterial(Instantiatedprefab);
        Bundle.Unload(false);
    }

    //Method to load the Materials from AssetBundles to Material array
    void SetMaterial(GameObject prefab)
    {
        //Bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));





        //var loadedAsset = Bundle.LoadAsset<GameObject>(asstName);
        //var loadAssetRequest = assetbundle.LoadAssetAsync<GameObject>(assetName);
        //GameObject loadedAsset = Bundle.LoadAsset<GameObject>(assetName);
        //GameObject Prefab = Instantiate(loadedAsset);
        //if (loadedAsset != null)
        //{
        //    // Instantiate the prefab with the materials set to its original materials
        //    GameObject instantiatedPrefab = Instantiate(loadedAsset);

        //    // Get all MeshRenderers in the instantiated prefab
        MeshRenderer meshRenderer = prefab.GetComponent<MeshRenderer>();
        //    //Debug.Log(meshRenderers[1]);
            
            // Check if the renderer has a material named "Default-Line"
        FindMaterialByName(defaultLineMaterialName,meshRenderer);

           
            

        // You can further manipulate or use the instantiatedPrefab as needed
        //Unload the AssetBundle to free up memory
        //Bundle.Unload(false);
        //materials = velocity_reference.LoadAllAssets<Material>();
        //assetsLoaded = true;
    }
    private void  FindMaterialByName(string materialName, MeshRenderer meshRenderer)
    {
        // Get the shared materials of the MeshRenderer
        Material[] sharedMaterials = meshRenderer.sharedMaterials;
        Debug.Log(sharedMaterials[0]);
        Material newMaterial = Resources.Load<Material>("unit_builtin_extra/Default-Line");

        //// Search for a material with the name "Default-Line" within the shared materials
        //for (int i = 0; i < sharedMaterials.Length; i++)
        //{
        //    if (sharedMaterials[i] != null && sharedMaterials[i].name == "Default-Line")
        //    {
        //        // Create a new material with the properties of the "Default-Line" material
        //        Material newMaterial = new Material(sharedMaterials[i]);

        //        // Assign the new material to the shared material
                
        //        sharedMaterials[i] = newMaterial;
        //        Debug.Log("Materialfound");
        //    }
        //}
        // If a "Default-Line" material is found, set it as the only material for the renderer
        if (newMaterial != null)
        {
            sharedMaterials[0] = newMaterial;
            Debug.Log("defaultLinepresent");
        }
        else
        {
            Debug.Log("Materialnotfound");
        }

    }

}
