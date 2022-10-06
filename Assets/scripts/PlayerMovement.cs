using System;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] [Range(0, 1f)] private float animationSmoothness = 0.3f;

    private Animator _anim;
    private CharacterController _controller;
    private static readonly int Blend = Animator.StringToHash("Blend");
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        RaycastHit hitInfo;
        // todo filter rotation raycast by ground plane
        // I should implement an invisible plane above the map, below the camera for the raycast to strike so it
        // can strike off the map. Or just compute the ideal target point with some math by tilting
        // the plane that the camera is probably using for ScreenToWorldPoint.
        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hitInfo)) return;

        var mousePos = hitInfo.point;
        var lookDir = mousePos - transform.position;
        var angle = Math.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, (float) angle, 0);
    }

    private void MovePlayer()
    {
        // Calculate Input Vectors
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var motion = new Vector3(horizontal, 0, vertical).normalized;
        motion.Normalize();

        if (motion.magnitude >= .1f)
        {
            _controller.Move(motion * (Time.deltaTime * moveSpeed));
        }

        // animate the player based on movement
        _anim.SetFloat(Blend, motion.magnitude, animationSmoothness, Time.deltaTime);
    }
}