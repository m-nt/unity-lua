using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PimDeWitte.UnityMainThreadDispatcher;
using System.Threading.Tasks;
using System;
using System.Threading;

public class TestDispatcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Thread.Sleep(1000);
        Task.Run(() =>
        {
            Debug.LogError("Start the Task");
            UnityMainThreadDispatcher.self.Enqueue(UnityObject("hello"));
        });
    }
    public IEnumerator UnityObject(string message) {
        yield return new WaitForEndOfFrame();
        Debug.LogError(message);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
