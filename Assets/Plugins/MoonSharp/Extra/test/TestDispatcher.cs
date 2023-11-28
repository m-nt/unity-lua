using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityLua;

public class TestDispatcher : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void Callback(object value);
    void Start()
    {
        Thread.Sleep(1000);
        Task.Run(() =>
        {
            Debug.LogError("Start the Task");
            TaskManager.self.Enqueue(UnityObject((object value)=>{
                Debug.LogError((string)value);
            }));
        });
    }
    public IEnumerator UnityObject(Callback callback) {
        Debug.LogError("Start IEnum");
        yield return new WaitForSeconds(5);
        callback("hello from callback");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
