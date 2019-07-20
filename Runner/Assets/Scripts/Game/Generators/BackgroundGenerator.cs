using UnityEngine;
using System.Collections;
using Core;
using Core.Parallax;

[RequireComponent(typeof(ParallaxBackground))]
public class BackgroundGenerator : AGeneratorPresenter<SpriteRenderer>
{
    #region Fields
    public Camera cam;
    public ParallaxLayerPool back_Layer;
    public float camXOffset = 0.01f;

    private ParallaxBackground parallax;
    private FollowToObject follower;
    #endregion Fields

    #region Unity Methods
    private void Start()
    {
        parallax = parallax ?? GetComponent<ParallaxBackground>();
        cam = FindObjectOfType<Camera>();

        InitParallax();

        follower = follower ?? GetComponent<FollowToObject>();
        follower.SetupObject(cam.gameObject);
    }

    private void Update()
    {
        DisableObject();
    }
    #endregion Unity Methods

    private void InitParallax()
    {
        #region back layer
        for (int i = 0; i < 2; i++)
        {
            var obj = back_Layer.Pull();
            generatorModel.GeneratedObjects.Add(obj);
            obj.gameObject.SetActive(true);
            if(i > 0)
                obj.transform.position = new Vector3(obj.transform.position.x - camXOffset + obj.gameObject.GetComponent<SpriteRenderer>().bounds.size.x, obj.transform.position.y, obj.transform.position.z);
        }
        #endregion backlayer
        parallax.SetLayers();
    }

    protected override void CreateObject()
    {
        throw new System.NotImplementedException();
    }

    protected override void DisableObject()
    {
        float dist;

        foreach (var b in back_Layer.Pool)
        {
            dist = (b.transform.position - Camera.main.transform.position).z;
            if (b.gameObject.transform.position.x + b.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2 < cam.ViewportToWorldPoint(new Vector3(0, 0, dist)).x)
            {
                b.transform.position = new Vector3(cam.ViewportToWorldPoint(new Vector3(0, 0, dist)).x - camXOffset + b.gameObject.GetComponent<SpriteRenderer>().bounds.size.x * 1.5f, b.transform.position.y, b.transform.position.z);
            }
        }
    }

    protected override IEnumerator ForceDisableObject()
    {
        throw new System.NotImplementedException();
    }
}
