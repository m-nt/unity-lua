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
		void Awake() {
			if (self == null) {
				self = this;
				DontDestroyOnLoad(gameObject);
			} else {
				Destroy(this);
			}
		}

        // Update is called once per frame
        void Update()
        {

        }
    }
}