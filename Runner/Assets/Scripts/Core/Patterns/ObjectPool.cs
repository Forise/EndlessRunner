using System.Collections.Generic;
using UnityEngine;

namespace Core.Patterns
{
    [System.Serializable]
    public class ObjectPool<T> : MonoBehaviour where T : Component
    {
        #region Fields
        [SerializeField]
        private T prefab;
        [SerializeField]
        private int count = 10;

        private List<T> pool = new List<T>();
        private List<T> holdedObjects = new List<T>();
        private bool isInited = false;
        #endregion Fields

        #region Properties
        public IReadOnlyList<T> HoldedObjects { get => holdedObjects.AsReadOnly(); }
        public IReadOnlyList<T> Pool { get => pool.AsReadOnly(); }

        public T Prefab { get => prefab; }
        public int Count { get => count; }
        #endregion Properties

        public ObjectPool() { pool = new List<T>(); }

        public ObjectPool(List<T> list)
        {
            pool = new List<T>();
            pool = list;
            foreach (var obj in pool)
            {
                (obj as GameObject).gameObject.SetActive(false);
            }
            isInited = true;
        }

        #region Unity Methods
        private void Awake()
        {
            Init();
        }
        #endregion Unity Mthods

        #region Methods
        private void Init()
        {
            if (!isInited)
            {
                if (!prefab)
                {
                    Debug.LogError("PREFAB IS NULL! Pease make sure you attached prefab to Object Pool.", this);
                    return;
                }
                pool = new List<T>();
                for (int i = 0; i < count; i++)
                {
                    T obj = Instantiate(prefab, transform);
                    if (obj == null)
                        Debug.LogError("Instantiation of objoct from pool is failed! Object is NULL!", this);
                    obj.name += i;
                    obj.gameObject.SetActive(false);
                    pool.Add(obj);
                }
                isInited = true;
            }
        }

        public T Pull(bool hold = false)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    if (hold)
                        holdedObjects.Add(pool[i]);
                    return pool[i];
                }
            }
            Debug.LogWarning("Object is null! Check count of objects in pull!");
            return null;
        }

        public T PullRandom(bool hold = false)
        {
            var rnd = Random.Range(0, pool.Count);
            if (!pool[rnd].gameObject.activeInHierarchy)
            {
                if (hold)
                    holdedObjects.Add(pool[rnd]);
                return pool[rnd];
            }
            return null;
        }

        public void Push(T obj, bool forcePush = false)
        {
            if (!obj)
            {
                Debug.LogError("Pushing object to pool is NULL!", this);
                return;
            }
            else if (obj.gameObject.activeInHierarchy)
            {
                try
                {
                    var founded = pool.Find(x => x == obj);
                    if (founded && (!holdedObjects.Contains(founded) || forcePush))
                    {
                        founded.gameObject.SetActive(false);
                        holdedObjects.Remove(founded);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex, this);
                }
            }
        }
        #endregion Methods
    }
}
