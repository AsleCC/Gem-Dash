using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class src_gemchara : MonoBehaviour
{
    //Gem stuff
    [SerializeField] Vector2 gempos;
    //Core mechanic stuff
    public bool isStarted;
    float timer;

    void Start() {
        gempos = new Vector2(0, 0);
        isStarted = false;
    }

    // Update is called once per frame
    void Update() {
        PlayerMovement();
       
    }
    bool PlayerMovement() {
        if(Input.GetAxisRaw("Horizontal") == 1) {
            gempos = new Vector2(6.75f, 0);
            isStarted = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1) {
            gempos = new Vector2(-6.75f, 0);
            isStarted = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 1) {
            gempos = new Vector2(0, 3.75f);
            isStarted = true;
        }
        else if (Input.GetAxisRaw("Vertical") == -1) {
            gempos = new Vector2(0, -3.75f);
            isStarted = true;
        }
        transform.position = gempos;
        return isStarted;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Me morí");
    }

}
