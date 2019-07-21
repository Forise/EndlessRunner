using UnityEngine;
namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class APickableItemPresenter : MonoBehaviour
    {
        [SerializeField]
        protected PickableItemModel model = new PickableItemModel();
        [SerializeField]
        protected Collider2D coll;

        public PickableItemType ItemType
        {
            get => model.Type;
        }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            if (coll)
                coll.enabled = true;
            if (model == null)
                model = new PickableItemModel();
            model.TypeChanged += OnTypeChanged_Handler;
        }

        protected virtual void OnTypeChanged_Handler() { }
        public abstract GameObject PickUp();
    }
}