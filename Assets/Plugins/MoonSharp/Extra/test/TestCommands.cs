using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityLua;

public class TestCommands : MonoBehaviour {
    InputField inputField;
    // Start is called before the first frame update
    void Start() {
        inputField = gameObject.GetComponent<InputField>();
    }

    public void OnGetCommand() {
        string[] commands = inputField.text.Split(' ');
        
        if (commands.Length <= 1) return;
        
        string eventName = commands[0];
        string[] args = new string[commands.Length - 1];
        for (int i = 0; i < args.Length; i++) {
            args[i] = commands[i + 1];
        }
        TaskManager.self.TriggerEvent(eventName,gameObject,args);
    }
}