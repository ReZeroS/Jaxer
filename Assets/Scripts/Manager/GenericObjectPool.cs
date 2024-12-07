using UnityEngine;
using UnityEngine.Pool;

namespace ReZeros.Jaxer.Manager
{
    public class GenericObjectPool<T> where T : MonoBehaviour
    {
        private IObjectPool<T> objectPool;
        private readonly T prefab;
        private readonly Transform parent;

        public GenericObjectPool(T prefab, Transform parent = null, int initialSize = 10, int maxSize = 50)
        {
            this.prefab = prefab;
            this.parent = parent;

            objectPool = new ObjectPool<T>(
                CreateObject,
                OnGetObject,
                OnReleaseObject,
                OnDestroyObject,
                false,
                initialSize,
                maxSize
            );

            // 预生成对象
            PrepopulatePool(initialSize);
        }

        private T CreateObject()
        {
            return Object.Instantiate(prefab, parent);
        }

        private void OnGetObject(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseObject(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.position = new Vector3(10000, 10000, 0); // 隐藏到场景外
        }

        private void OnDestroyObject(T obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void PrepopulatePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Release(Get());
            }
        }

        public T Get()
        {
            return objectPool.Get();
        }

        public void Release(T obj)
        {
            objectPool.Release(obj);
        }
    }
}