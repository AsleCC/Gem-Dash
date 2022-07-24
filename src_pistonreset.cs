using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_pistonreset : MonoBehaviour {
    public bool isTouched;
    bool isReseted;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Borders" && !isReseted) {
            Debug.Log("Touched!");
            isReseted = true;
        }
        
    }
    void Update() {
        if (isReseted) {
            isTouched = true;
        }
        if(!isReseted) {
            isTouched = false;
        }
        isReseted = false;
    }
    private void Start() {
        isReseted = false;
    }

}

