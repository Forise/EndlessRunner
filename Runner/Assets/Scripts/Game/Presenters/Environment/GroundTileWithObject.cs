using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileWithObject : AGroundTilePresenter
{
    #region Fields
    [SerializeField]
    protected SpawnType spawnType;
    [SerializeField]
    protected string tagToDetectForCollide;
    [SerializeField]
    protected GameObject obj;
    [SerializeField, Range(0f, 1f)]
    protected float objectChance;
    [SerializeField]
    protected int safeSteps;
    [SerializeField]
    protected bool generateObjInRandomPos;
    [SerializeField]
    protected List<Transform> positionsToSpawn = new List<Transform>();

    private Vector3 defaultLocalPosition;
    #endregion Fields

    #region Unity Methods
    private void OnEnable()
    {
        Init();
        if (spawnType == SpawnType.Default)
            SpawnObject();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (spawnType == SpawnType.ByCollide && collision.gameObject.tag.Contains(tagToDetectForCollide))
        {
            SpawnObject();
        }
    }
    #endregion Unity Methods

    #region Methods
    protected override void Init()
    {
        base.Init();
        defaultLocalPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, obj.transform.localPosition.z);
    }

    protected void SpawnObject()
    {
        if (obj)
        {
            if (groundModel.Position.x >= safeSteps)
            {
                var rnd = Random.Range(0f, 1f);
                if (rnd <= objectChance)
                {
                    if (generateObjInRandomPos && positionsToSpawn != null && positionsToSpawn.Count > 0)
                        obj.transform.localPosition = positionsToSpawn[Random.Range(0, positionsToSpawn.Count)].localPosition;
                    obj.SetActive(true);
                }
                else
                    obj.SetActive(false);
            }
            else
                obj.SetActive(false);
        }
    }
    #endregion Methods

    protected enum SpawnType : byte
    {
        Default = 0x00,
        ByCollide = 0x01
    }
}