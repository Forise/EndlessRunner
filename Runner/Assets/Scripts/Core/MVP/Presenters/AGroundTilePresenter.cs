using UnityEngine;
using Core;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class AGroundTilePresenter : MonoBehaviour
{
    #region Model
    [SerializeField]
    protected GameObjectModel groundModel = new GameObjectModel();
    #endregion


    protected virtual void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        groundModel.PositionChanged -= UpdatePosition;
        groundModel.LocalPositionChanged -= UpdateLocalPosition;
        groundModel.RotationChanged -= UpdateRotation;
    }
    #region Methods
    protected virtual void Init()
    {
        groundModel = groundModel ?? new GameObjectModel();
        groundModel.PositionChanged += UpdatePosition;
        groundModel.LocalPositionChanged += UpdateLocalPosition;
        groundModel.RotationChanged += UpdateRotation;

        groundModel.Position = transform.position;
        groundModel.LocalPosition = transform.localPosition;
        groundModel.Rotation = transform.rotation;
    }

    public void SetPosition(Vector3 pos)
    {
        groundModel.Position = pos;
    }
    public void SetLocalPosition(Vector3 pos)
    {
        groundModel.LocalPosition = pos;
    }

    public void SetRotation(Quaternion rot)
    {
        groundModel.Rotation = rot;
    }
    #endregion
    public virtual void UpdatePosition()
    {
        transform.position = groundModel.Position;
    }
    public virtual void UpdateLocalPosition()
    {
        transform.localPosition = groundModel.LocalPosition;
    }
    public virtual void UpdateRotation()
    {
        transform.rotation = groundModel.Rotation;
    }
}
