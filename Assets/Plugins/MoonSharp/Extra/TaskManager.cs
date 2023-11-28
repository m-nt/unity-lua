using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;
namespace UnityLua
{
    public class TaskManager : MonoBehaviour
    {
        
        private static readonly Queue<Action> queue = new Queue<Action>();
        public static TaskManager self = null;
        public void Enqueue(IEnumerator action) {
            lock (queue) {
                queue.Enqueue (() => {
                    StartCoroutine (action);
                });
            }
        }
        public void Enqueue(Action action) {
            lock (queue) {
                queue.Enqueue (() => {
                    ActionWrapper(action);
                });
            }
        }
        public Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction() {
                try 
                {
                    action();
                    tcs.TrySetResult(true);
                } catch (Exception ex) 
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }
        IEnumerator ActionWrapper(Action action)
        {
            action();
            yield return null;
        }
        void Awake() {
            if (self == null) {
                self = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(this);
            }
        }

        // Update is called once per frame
        void Update() {
            lock (queue) {
                while (queue.Count > 0)
                {
                    queue.Dequeue().Invoke();
                }
            }
        }
    }
}