using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara : MonoBehaviour
{
    [SerializeField] Transform playerCamera= null;
    [SerializeField] float sens= 3.5f;
    float cameraPitch = 0.0f;//Para que la cámara mire directamente hacia el frente
    [SerializeField] bool lockCursor = true;
    void Start(){
        if(lockCursor) {
            Cursor.lockState =CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update(){
        UpdateMouseLook();
    }

    void UpdateMouseLook() {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")); //Mousedelta contiene la x e y (Mousedelta[x,y])

        cameraPitch -= mouseDelta.y * sens; //Para corregir el error de la y invertida
        cameraPitch =Mathf.Clamp(cameraPitch, -90.0f, 90.0f); //Capamos la rotacion  y para que no haga un giro de 360º la cámara
        //Rotación Y (Cámara)
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        //Rotación X
        transform.Rotate(Vector3.up * mouseDelta.x * sens); //Dirección, información del cursor (x), sensibilidad
    }
}
