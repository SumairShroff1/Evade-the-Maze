using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;       // Швидкість руху гравця
    public float jumpForce = 4.5f;   // Сила стрибка
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;
    public GameObject feet;
    public Rigidbody rb;
    [SerializeField] private Animator animator;

    private bool cooldoun = false;

    void Update()
    {
        if(GamePause.isPaused){
            return;
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Jump();
        // }
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized * speed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (rb.velocity.x != 0 || rb.velocity.z != 0){
            Vector3 direction = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            transform.rotation = Quaternion.LookRotation(direction);
            animator.SetBool("isRunning", true);
        }else{
            animator.SetBool("isRunning", false);
        }

    }
    void Move()
    {
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }

    bool CheckGround()
    {
        return Physics.Raycast(feet.transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
    // void Jump()
    // {
    //     if(!CheckGround() || cooldoun){
    //         return;
    //     }
        
    //     rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    //     StartCoroutine(JumpCooldown());
    // }
    IEnumerator JumpCooldown()
    {
        cooldoun = true;
        yield return new WaitForSeconds(0.3f);
        cooldoun = false;
    }
    public void ActiveSpeedPlayer(float _Speed){
        StartCoroutine(ActiveSpeedPlayerIE(_Speed));
    }
    IEnumerator ActiveSpeedPlayerIE(float _Speed){
        yield return new WaitForSeconds(3f);

        speed = _Speed;
    }
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = true;
    //     }
    // }
    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground"))
    //     {
    //         isGrounded = false;
    //     }
    // }
}
