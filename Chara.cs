using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara : MonoBehaviour
{
    //Clases
    float cameraPitch = 0.0f;//Para que la cámara mire directamente hacia el frente
    float velocityY = 0.0f;
    //Cosas de la cámara
    [SerializeField] Transform playerCamera= null;
    [SerializeField] float sens= 3.5f;
    

    [SerializeField] bool lockCursor = true;
    //Cosas del movimiento
    [SerializeField] float walkSpeed = 8f;
    [SerializeField] float gravity = -13f;
     [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; 
    CharacterController controller = null;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    void Start(){
        controller = GetComponent<CharacterController>(); //es como en gm2 para referenciar cosos de otros objetos (ej: pene.x)
        if(lockCursor) {
            Cursor.lockState =CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update(){
        UpdateMouseLook();
        WASD();
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
    void WASD() {
        Vector2 targetDir  = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")); //Asignamos un vector 2 los valor horizontal (1 o 0 o -1) e igual que el vertical.
        targetDir.Normalize(); //Correción para que cuando vayas diagonalmente no vayas ligeramente más rápido

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); //Para suavizar el movimiento

        if(controller.isGrounded) { //Si el jugador esta en el suelo, se resetea la velocidad en Y
            velocityY= 0.0f;
        }else {
            velocityY+= gravity *Time.deltaTime; //si no, es aplicamos la gravedad por cada frame para ir acelerando.
        }
        

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x)* walkSpeed + Vector3.up * velocityY; // foward = (0,0,1) * 1 o 0 + right = (0,0,1) * 1 o 0) y todo esto, multiplicado por la velidad de andar

        controller.Move(velocity * Time.deltaTime); //Para aplicar el movimiento al personaje
    }
}
