using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_pistonreset : MonoBehaviour {
    public bool isTouched;
    bool isReseted;
    public int bonks;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Borders" && !isReseted) {
            bonks++;
            isReseted = true;
        }
        
    }
    void Update() {
        if (isReseted) {
            isTouched = true;
            bonks = 0;
        }
        if(!isReseted) {
            isTouched = false;
        }
        isReseted = false;
    }
    private void Start() {
        isReseted = false;
        bonks = 0;
    }

}

