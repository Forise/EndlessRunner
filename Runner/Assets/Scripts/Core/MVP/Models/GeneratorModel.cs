using System.Collections.Generic;

namespace Core
{
    [System.Serializable]
    public class GeneratorModel<T> : Model
    {
        protected List<T> objects = new List<T>();

        public event ModelChangedDelegate OnObjectsChanged;

        public List<T> GeneratedObjects
        {
            get => objects;
            set
            {
                objects = value;
                OnObjectsChanged();
            }
        }
    }
}