using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickControler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image circle;
    public Image outerCircle;

    public Transform floor;

    public float speed = 6f;
    float camRayLength = 100;

    Vector3 movement;
    Vector3 inputVector;

    Animator anim;

    Rigidbody playerRigidbody;

    public Transform player;

    int floorMask;

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerCircle.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / outerCircle.rectTransform.sizeDelta.x);
            pos.y = (pos.y / outerCircle.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            // Move Joystick
            circle.rectTransform.anchoredPosition = new Vector3(inputVector.x * (outerCircle.rectTransform.sizeDelta.x / 3),
                                                                inputVector.z * (outerCircle.rectTransform.sizeDelta.y / 3));
            
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        circle.rectTransform.anchoredPosition = Vector3.zero;
    }

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = player.GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        floor.transform.position = player.position;
        float h = inputVector.x;
        float v = inputVector.z;

        Move(h, v);
        Turning();
        //Animating(h, v);
        
    }

    public void Move(float h, float v)
    {
        player.transform.Translate(h*speed*Time.deltaTime, 0, v*speed*Time.deltaTime);
    }

    public void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - player.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    /*void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }*/

}
