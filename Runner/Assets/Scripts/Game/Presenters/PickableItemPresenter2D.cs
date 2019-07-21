using UnityEngine;

namespace Core
{
    public class PickableItemPresenter2D : APickableItemPresenter
    {
        [SerializeField]
        protected GameObject obj;
        protected Camera cam;

        private void OnEnable()
        {
            gameObject.SetActive(true);
        }

        protected override void Init()
        {
            cam = Camera.main;
            base.Init();
        }

        public override GameObject PickUp()
        {
            gameObject.SetActive(false);
            return obj;
        }

        public void DisableCollider()
        {
            if (coll)
                coll.enabled = false;
        }

        public void EnableCollider()
        {
            if (coll)
                coll.enabled = true;
        }
    }
}