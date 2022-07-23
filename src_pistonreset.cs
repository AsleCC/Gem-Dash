using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_pistonreset : MonoBehaviour {
    public bool isTouched;
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Collided");
        isTouched = true;
    }
    void Update() {
        isTouched = false;
    }
}

