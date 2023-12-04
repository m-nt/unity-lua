using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Threading;
using MoonSharp.Interpreter;

namespace UnityLua {
    public class ResourceManager : MonoBehaviour {
        public static ResourceManager self;
        public Dictionary<string, GameObject> GUIDToGameObjectReferencePair = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> GUIDToGameObjectInstancePair = new Dictionary<string, GameObject>();
        public ResourceTable table;
        // Start is called before the first frame update
        void Awake() {
            if (self == null) {
                self = this;
                DontDestroyOnLoad(this);
            } else {
                Destroy(this);
            }
            GUID[] objcts = FindObjectsOfType<GUID>();
            foreach (GUID item in objcts) {
                if (!GUIDToGameObjectReferencePair.ContainsKey(item.guid)) GUIDToGameObjectReferencePair.Add(item.guid, item.gameObject);
                if (!GUIDToGameObjectInstancePair.ContainsKey(item.guid)) GUIDToGameObjectInstancePair.Add(item.guid, item.gameObject);
            }
            table.Convert();
        }
        public void RegisterCommand(string name, DynValue callback) {
            if (callback.Type != DataType.Function) return;
            TaskManager.self.AddListenerFirstStay(name, callback);
        }
        public void TriggerCommand(string name, GameObject sender, string[] args) {
            TaskManager.self.TriggerEvent(name, sender, args);
        }
        public void MoveObject(string guid, float x, float y, float z) {
            TaskManager.self.Enqueue(_moveObject(guid,x,y,z));
        }
        IEnumerator _moveObject(string guid, float x, float y, float z) {
            if (!GUIDToGameObjectInstancePair.ContainsKey(guid)) yield break;
            GUIDToGameObjectInstancePair.TryGetValue(guid, out GameObject obj);
            if (!obj) yield break;
            obj.transform.position = new Vector3(x, y, z);
            yield return new WaitForEndOfFrame();
        }
        public bool ObjectExists(string guid) {
            return GUIDToGameObjectReferencePair.ContainsKey(guid);
        }
        public string CreateObject(string guid,float x,float y, float z){
            string id = Guid.NewGuid().ToString();
            TaskManager.self.Enqueue(_CreateObject(id,guid,x,y,z));
            return id;
        }
        IEnumerator _CreateObject(string id,string guid, float x, float y, float z) {
            yield return new WaitForEndOfFrame();
            GUIDToGameObjectReferencePair.TryGetValue(guid, out GameObject obj);
            if (!obj) Debug.LogError("object didn't found");
            GameObject new_obj = Instantiate(obj,new Vector3(x,y,z),Quaternion.identity);
            GUIDToGameObjectInstancePair.Add(id, new_obj);
        }
        public bool IsValidObject(string name) {
            return table.nameToGuidPair.ContainsKey(name);
        }
        public string LoadObject(string name){
            string guid = null;
            TaskManager.self.Enqueue(_loadObject(name, (_guid) =>{guid = _guid;}));
            while (guid == null) {
                Task.Delay(10);
            }
            return guid;
        }
        delegate void loadCallback(string guid);
        IEnumerator _loadObject(string name, loadCallback callback) {
            yield return new WaitForEndOfFrame();
            table.nameToGuidPair.TryGetValue(name, out string guid);
            if (guid == null) yield break;
            if (GUIDToGameObjectReferencePair.ContainsKey(guid)) {
                callback(guid);
                yield break;
            }
            Addressables.LoadAssetAsync<GameObject>(guid).Completed += AfterLoaded;
            callback(guid);
        }

        private void AfterLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded){
                GameObject obj = handle.Result;
                string guid = obj.GetComponent<GUID>().guid;
                GUIDToGameObjectReferencePair.Add(guid, obj);
            } else {
                Debug.LogError("Asset didn't load");
            }
        }
    }
}