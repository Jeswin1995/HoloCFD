using Microsoft.MixedReality.OpenXR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class AddressableBundler : MonoBehaviour
{
    /// <summary>
    /// The prefab to spawn.
    /// </summary>
    public AssetReferenceGameObject SpawnablePrefab;
    public AssetReference materialAssetRef;

    /// <summary>
    /// The time, in seconds, to delay before spawning prefabs.
    /// </summary>
    public float DelayBetweenSpawns = 2.0f;

    /// <summary>
    /// The time, in seconds, to delay before destroying the spawned prefabs.
    /// </summary>
    public float DealyBeforeDestroying = 1.0f;

    /// <summary>
    /// The number of prefabs to spawn.
    /// </summary>
    public int NumberOfPrefabsToSpawn = 1;

    private Material material;
    [SerializeField] private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTemporaryCube());
        
        // load using string address
        material = Addressables.LoadAssetAsync<Material>(materialAssetRef).WaitForCompletion();
        // load using AssetReference (assigned in Editor)
        //material = materialAssetRef.LoadAssetAsync().WaitForCompletion();
    }

    //IEnumerator StartSpawner()
    //{
    //    //while (true)
    //    //{
    //    //    yield return new WaitForSeconds(DelayBetweenSpawns);
    //      StartCoroutine(SpawnTemporaryCube());
    //    //}
    //}

    IEnumerator SpawnTemporaryCube()
    {
        List<AsyncOperationHandle<GameObject>> handles = new List<AsyncOperationHandle<GameObject>>();

        for (int i = 0; i < NumberOfPrefabsToSpawn; i++)
        {
            //Instantiates a prefab with the address "Cube".  If this isn't working make sure you have your Addressable Groups
            //window setup and a prefab with the address "Cube" exists.
            AsyncOperationHandle<GameObject> handle = SpawnablePrefab.InstantiateAsync();
            handles.Add(handle);
            //GameObject prefab = Instantiate(handle.Result, spawnPoint.position, Quaternion.identity);
            Debug.Log("Gameobject instantiated");
            GameObject prefab = handle.Result.gameObject;
            MeshRenderer renderer = prefab.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            Debug.Log("material changed");



        }

        yield return new WaitForSeconds(DealyBeforeDestroying);

        //Release the AsyncOperationHandles which destroys the GameObject
        //foreach (var handle in handles)
        //    Addressables.Release(handle);
    }
}
