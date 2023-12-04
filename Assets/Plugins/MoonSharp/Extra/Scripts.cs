using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace UnityLua {
    public class Scripts : MonoBehaviour
    {
        public static Scripts self;
        public Script baseScripts;
        Dictionary<string, Script> Dict = new Dictionary<string, Script>{};
        void DebugError(object value){
            Debug.LogError($"[Lua] - {value}");
        }
        public void Wait(int ms) {
            Task.Delay(ms).Wait();
        }
        public void AddScript(string path, string script)
        {
            Script s = baseScripts;
            Task.Run(() =>
            {
                s.DoString(script);
            });
            Dict.Add(path, s);
        }

        public void RemoveScript(string path)
        {
            Dict.Remove(path);
        }
        private void Awake()
        {
            if (self == null)
            {
                self = this;
                DontDestroyOnLoad(this);
                Script.WarmUp();
            }
            else
            {
                DestroyImmediate(this);
            }
            baseScripts = new Script(CoreModules.Preset_Complete);
            // shared types
            UserData.RegisterType<EventArgs>();
            UserData.RegisterType<GameObject>();
            // UserData.RegisterType<object>();
            // load all natives for the lua file
            baseScripts.Globals["Print"] = (Action<object>)DebugError;
            baseScripts.Globals["Wait"] = (Action<int>)Wait;
            baseScripts.Globals["LoadObject"] = (Func<string, string>)ResourceManager.self.LoadObject;
            baseScripts.Globals["CreateObject"] = (Func<string,float,float,float,string>)ResourceManager.self.CreateObject;
            baseScripts.Globals["ObjectExists"] = (Func<string, bool>)ResourceManager.self.ObjectExists;
            baseScripts.Globals["RegisterCommand"] = (Action<string, DynValue>)ResourceManager.self.RegisterCommand;
            baseScripts.Globals["TriggerCommand"] = (Action<string, GameObject, string[]>)ResourceManager.self.TriggerCommand;
            baseScripts.Globals["IsValidObject"] = (Func<string, bool>)ResourceManager.self.IsValidObject;
            baseScripts.Globals["MoveObject"] = (Action<string, float, float, float>)ResourceManager.self.MoveObject;
        }
    }
}