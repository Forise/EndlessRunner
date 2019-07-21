using System.Collections;
using UnityEngine;
using Core;
using Core.Patterns;

public class GroundGeneratorPresenter : AGeneratorPresenter<AGroundTilePresenter>
{
    #region Fields
    [SerializeField]
    public Camera cam;
    [SerializeField]
    private float camStep;

    [SerializeField]
    protected float nextPosStep = 1;
    [SerializeField]
    protected float nextPos = 0;
    [SerializeField]
    protected Dictionary_GroundTilePool_Chance groundTilePools;
    protected WeightedRandomCollection<GroundTilePool> randomCollection = new WeightedRandomCollection<GroundTilePool>();
    #endregion Fields

    protected virtual void Start()
    {
        StartForceDisable();
        StartCoroutine(GenerationCoroutine());
    }

    #region Methods
    protected override void Init()
    {
        //Init random collection
        foreach (var p in groundTilePools)
        {
            randomCollection.AddEntry(p.Key, p.Value);
        }

        generatorModel = generatorModel ?? new GeneratorModel<AGroundTilePresenter>();
        cam = FindObjectOfType<Camera>();
        base.Init();
    }

    protected override void CreateObject()
    {
        var rnd = Random.Range(0f, 100f);
        GroundTilePool pool;
        pool = randomCollection.GetRandom();

        var newPlatform = pool.Pull();



        if (generatorModel.GeneratedObjects.Count > 0)
        {
            if (generatorModel.GeneratedObjects[generatorModel.GeneratedObjects.Count - 1].transform.position.x - cam.transform.position.x - 1 <= camStep)
            {
                InstantiateTile(newPlatform);
            }
        }
        else
        {
            InstantiateTile(newPlatform);
        }
    }

    private void InstantiateTile(AGroundTilePresenter newPlatform)
    {
        if (newPlatform != null)
        {
            generatorModel.GeneratedObjects.Add(newPlatform.GetComponent<AGroundTilePresenter>());
            SetNewGroundTilePos(newPlatform);
            newPlatform.SetRotation(Quaternion.identity);
            newPlatform.transform.SetAsLastSibling();
            newPlatform.gameObject.SetActive(true);
        }
    }

    protected override void DisableObject()
    {
        foreach (var tile in generatorModel.GeneratedObjects)
        {
            if (tile.gameObject.transform.position.x + camStep < cam.transform.position.x)
                tile.transform.parent.GetComponent<ObjectPool<AGroundTilePresenter>>().Push(tile);
        }
    }

    protected virtual void SetNewGroundTilePos(AGroundTilePresenter obj)
    {
        obj.SetPosition(new Vector3(nextPos, obj.transform.position.y, obj.transform.position.z));

        nextPos += nextPosStep;
    }

    protected override IEnumerator ForceDisableObject()
    {
        yield return new WaitForSeconds(5f);
        var tiles = FindObjectsOfType<AGroundTilePresenter>();
        foreach (var tile in tiles)
        {
            if (tile && tile.gameObject.transform.position.x + camStep < cam.transform.position.x)
            {
                var pool = tile.transform.parent.GetComponent<GroundTilePool>();
                pool?.Push(tile);
            }
        }
        yield return StartCoroutine(ForceDisableObject());
    }

    protected void UpdateView()
    {
        DisableObject();
        CreateObject();

        int objectsCountInPools = 0;
        foreach (var p in groundTilePools)
        {
            objectsCountInPools += p.Key.Count;
        }

        if (generatorModel.GeneratedObjects.Count > objectsCountInPools)
            generatorModel.GeneratedObjects.RemoveAt(0);
    }
    #endregion

    #region Coroutines
    private IEnumerator GenerationCoroutine()
    {
        DisableObject();
        CreateObject();

        int objectsCountInPools = 0;
        foreach (var p in groundTilePools)
        {
            objectsCountInPools += p.Key.Count;
        }

        if (generatorModel.GeneratedObjects.Count > objectsCountInPools)
            generatorModel.GeneratedObjects.RemoveAt(0);
        yield return null;
        yield return StartCoroutine(GenerationCoroutine());
    }
    #endregion
}

[System.Serializable]
public class Dictionary_GroundTilePool_Chance : SerializableDictionary<GroundTilePool, float> { }