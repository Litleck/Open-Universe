using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
  public GameObject spawnPrefab;
  public TMP_Text movementSpeedText;

  public float movementSpeed = 10f;
  public float mouseSensitivity = 1000f;

  float xRotation = 0f;
  float desiredX;

  new Camera camera;

  void Start() {
    camera = this.GetComponent<Camera>();

    Cursor.lockState = CursorLockMode.Locked;

    desiredX = transform.rotation.x;
  }

  void Update() {
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    Vector3 rotation = transform.localRotation.eulerAngles;
    desiredX = rotation.y + mouseX;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0f);

    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    movementSpeed = Mathf.Clamp(movementSpeed + Input.mouseScrollDelta.y * Time.deltaTime * 500, 0, Mathf.Infinity);
    movementSpeedText.text = "Movement Speed: " + movementSpeed.ToString("n2") + "m/s";

    Vector3 newPosition = transform.right * horizontal + transform.forward * vertical * movementSpeed;

    if (Input.GetKey(KeyCode.LeftShift)) {
        newPosition *= 5;
    }

    if (Input.GetKeyDown(KeyCode.R)) {
        ResetCamera();
    }

    if (Input.GetMouseButtonDown(0)) {
        StartCoroutine(SpawnObject());
    }
    
    this.transform.position += newPosition;
  }

  public void ResetCamera() {
    this.transform.position = new Vector3 (0, 0, -10);
    camera.orthographicSize = 5f;
  }

  IEnumerator SpawnObject() {
    Vector3 oldPos = camera.ScreenToWorldPoint(Input.mousePosition);
    
    yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

    Vector3 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
    Vector3 velocity = (newPos - oldPos).normalized * (newPos - oldPos).magnitude;
    
    Debug.DrawLine(oldPos, newPos, Color.white);

    GameObject body = Instantiate(spawnPrefab, newPos, new Quaternion(0, 0, 0, 0));
    body.GetComponent<Body>().initialVelocity = velocity;

    yield return null;
  }
}