using UnityEngine;

namespace Core
{
    public abstract class AUserDataPresenter : MonoBehaviour
    {
        public event Model.ModelChangedDelegate DataChanged
        {
            add { Model.UserDataChanged += value; }
            remove { Model.UserDataChanged -= value; }
        }
        protected abstract AUserDataModel Model { get; set; }

        public string StringDateTime { get => Model.StringDateTime; set => Model.StringDateTime = value; }

        public string JsonUD { get => JsonUtility.ToJson(Model); }

        public abstract void SetNewModelData(AUserDataModel model);
    }
}