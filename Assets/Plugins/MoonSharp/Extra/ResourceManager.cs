using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using PimDeWitte.UnityMainThreadDispatcher;
using System.Threading.Tasks;

namespace UnityLua {
    public class ResourceManager : MonoBehaviour {
        public static ResourceManager self;
        public Dictionary<string, GameObject> GUIDToGameObjectPair = new Dictionary<string, GameObject>();
        public ResourceTable table;
        // Start is called before the first frame update
        void Awake() {
            if (self == null) {
                self = this;
                DontDestroyOnLoad(this);
            } else {
                Destroy(this);
            }
            table.Convert();
        }
        public bool ObjectExists(string guid) {
            return GUIDToGameObjectPair.ContainsKey(guid);
        }
        public void CreateObject(string guid){
            GUIDToGameObjectPair.TryGetValue(guid, out GameObject obj);
            if (obj) Instantiate(obj);
            else Debug.LogError("object didn't found");
        }
        public string LoadObject(string name){
            table.nameToGuidPair.TryGetValue(name, out string guid);
            if (guid == null) return null;
            Addressables.LoadAssetAsync<GameObject>(guid).Completed += AfterLoaded;
            return guid;
        }

        private void AfterLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded){
                GameObject obj = handle.Result;
                string guid = obj.GetComponent<GUID>().guid;
                Debug.LogError(obj.name);
                Debug.LogError(guid);
                GUIDToGameObjectPair.Add(guid, obj);
            } else {
                Debug.LogError("Asset didn't load");
            }
        }
    }
}