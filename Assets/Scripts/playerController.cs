using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour {
    //Public Vars
    public PlayerInput player_input;
    public InputAction left_stick;
    public InputAction right_stick;
    public bool is_2d;
    public bool is_fps;
    public float speed;
    public Camera cam;
    public float mouseSensitivity;
    
    //Private Vars
    private Rigidbody rb;
    private Rigidbody2D rb2d;
    
    //FPS camera movement mins and maxs
    private float minY;
    private float maxY;
    private float minX;
    private float maxX;


    // Start is called before the first frame update
    void Start() {
        player_input = this.gameObject.GetComponent<PlayerInput>();
        left_stick = player_input.actions["Move"];
        right_stick = player_input.actions["Right_Stick"];
        speed = 2f;

        if (is_2d) {
            this.gameObject.AddComponent<Rigidbody2D>();
            rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        } else {
            this.gameObject.AddComponent<Rigidbody>();
            rb = this.gameObject.GetComponent<Rigidbody>();
        }

        if (is_fps) {
            cam = player_input.camera;
            cam.transform.parent = this.gameObject.transform;
            cam.transform.position = new Vector3(0, 1, 0);

            mouseSensitivity = 3f;
            minY = -60f;
            maxY = 60f;
        }
    }

    //Allows for 2d player movement
    public void movement2D() { 
        
    }

    //Allows for 3d movement
    public void movement3D() {
        Vector2 movement = left_stick.ReadValue<Vector2>();
        
        Vector3 move3d = new Vector3();
        move3d.x = movement.x;
        move3d.z = movement.y;
        
        Vector3 tempVect = move3d * speed * Time.fixedDeltaTime;
        rb.MovePosition(this.transform.position + tempVect);
    }

    public void FPS_camera()
    {
        Vector3 camMovement = right_stick.ReadValue<Vector2>();

        float lookX = camMovement.x * mouseSensitivity * Time.deltaTime;
        float lookY = camMovement.y * mouseSensitivity * Time.deltaTime;

        float tempCamMove = -lookY;
        tempCamMove = Mathf.Clamp(tempCamMove, minY, maxY);

        cam.transform.localRotation = Quaternion.Euler(tempCamMove, 0f, 0f);
        cam.transform.Rotate(Vector3.up * lookX);
        
        Debug.Log(tempCamMove);
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        movement3D();
        FPS_camera();
    }
}
