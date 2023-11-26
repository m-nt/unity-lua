using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using System.Collections;
namespace UnityLua {
    public class Scripts : MonoBehaviour
    {
        public static Scripts self;
        Dictionary<string, Script> Dict = new Dictionary<string, Script>{};
        void DebugError(object value){
            Debug.LogError($"[Lua] - {value}");
        }
        public void AddScript(string path, string script)
        {
            Script s = new Script(CoreModules.Preset_Complete);
            // load all natives for the lua file
            s.Globals["DebugError"] = (Action<object>)DebugError;
            s.Globals["LoadObject"] = (Func<string, string>)ResourceManager.self.LoadObject;
            s.Globals["CreateObject"] = (Action<string>)ResourceManager.self.CreateObject;
            s.Globals["ObjectExists"] = (Func<string, bool>)ResourceManager.self.ObjectExists;
            StartCoroutine(ExecuteLua(s, script));
            Dict.Add(path, s);
        }
        IEnumerator ExecuteLua(Script s, string script){
            yield return new WaitForEndOfFrame();
            s.DoString(script);
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
        }
    }
}