using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private AssetReference HXprefab;

    [SerializeField]
    private AssetReference HXMaterial;
    void Start()
    {
        AsyncOperationHandle<Material> materialHandle = Addressables.LoadAssetAsync<Material>(HXMaterial);
        Material material1 = materialHandle.Result;
        HXprefab.InstantiateAsync().Completed += (go) =>
        {
            HXMaterial.InstantiateAsync().Completed += (go1) =>
            {
                Renderer HXmeshRender = go.Result.GetComponent<MeshRenderer>();
                Material[] sharedMaterials = HXmeshRender.sharedMaterials;
                sharedMaterials[0] = material1;
            };

        };
        //Addressables.InitializeAsync().Completed += AddressablesManager_Completed(material1);
    }
    
    //private void AddressablesManager_Completed(AsyncOperationHandle<IResourceLocator> fbx)
    //{
    //    HXprefab.InstantiateAsync().Completed += (go) =>
    //    {
    //        HXMaterial.InstantiateAsync().Completed += (go1) =>
    //        {
    //            Renderer HXmeshRender = go.Result.GetComponent<MeshRenderer>();
    //            Material[] sharedMaterials = HXmeshRender.sharedMaterials;
    //            sharedMaterials[0] = material1;
    //        };           
                       
    //    };
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
