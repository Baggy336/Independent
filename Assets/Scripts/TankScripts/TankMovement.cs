using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Netcode;
using Unity.Collections;

// Handle location for host and client tanks
public class TankMovement : NetworkBehaviour
{
    public int playerNumber = 1;
    public float speed = 12f;
    public float turnSpeed = 180f;
    /*
    public AudioSource movementAudio;
    public AudioClip engineIdle;
    public AudioClip engineWorking;
    */
    public float pitchRange = .2f;

    private NetworkVariable<float> forwardPosition = new NetworkVariable<float>();
    private NetworkVariable<float> sideRotation = new NetworkVariable<float>();

    private string moveAxis;
    private string turnAxis;
    private Rigidbody body;
    private float moveInput;
    private float turnInput;
    private float startingPitch;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) Destroy(this);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        body.isKinematic = false;
        moveInput = 0f;
        turnInput = 0f;

    }

    private void OnDisable()
    {
        body.isKinematic = true;
    }

    // Setup the movement axis
    private void Start()
    {
        moveAxis = "Vertical";
        turnAxis = "Horizontal";
        //startingPitch = movementAudio.pitch;
    }

    /*
    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardPosition.Value);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + sideRotation.Value, transform.rotation.z);
    }

    private void UpdateClient()
    {
        float d = Drive();
        float t = Steer();
        UpdateClientPositionServerRpc(d, t);
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forward, float rotation)
    {
        forwardPosition.Value = forward;
        sideRotation.Value = rotation;
    }
    */

    private void Update()
    {
        if (IsOwner)
        {
            moveInput = Input.GetAxis(moveAxis);
            turnInput = Input.GetAxis(turnAxis);
            Drive();
            Steer();
        }

        //EngineAudio();
    }

    // Handles forward and backward movement
    private float Drive()
    {
        Vector3 dir = transform.forward * moveInput * speed * Time.deltaTime;

        body.MovePosition(body.position + dir);
        float dirs = dir.magnitude;
        return dirs;
    }

    // Handle side to side rotation
    private float Steer()
    {
        float turnRadius = turnInput * turnSpeed * Time.deltaTime;

        Quaternion turn = Quaternion.Euler(0f, turnRadius, 0f);

        body.MoveRotation(body.rotation * turn);
        float rots = turn.y;
        return rots;
    }




    /*
private void EngineAudio()
{
    if (Mathf.Abs(moveInput) < 0.1f && Mathf.Abs(turnInput) < 0.1f)
    {
        if (movementAudio.clip == engineWorking)
        {
            movementAudio.clip = engineIdle;
            movementAudio.pitch = Random.Range(startingPitch - pitchRange, startingPitch + pitchRange);
            movementAudio.Play();
        }
    }
    else
    {
        if (movementAudio.clip == engineIdle)
        {
            movementAudio.clip = engineWorking;
            movementAudio.pitch = Random.Range(startingPitch - pitchRange, startingPitch + pitchRange);
            movementAudio.Play();
        }
    }
}
*/
}
