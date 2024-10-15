using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //LoadScene���g�����߂ɕK�v

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigid;    //Player��Rigidbody2D
    Animator animator;  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O�����i�A�j���[�V�����N���b�v���쐬�������ɃR�����g�A�E�g����j

    public float walkSpeed = 6.0f;  //�v���C���[�̈ړ����x
    public float maxJumpForce = 20f;    //�v���C���[�̍ő�W�����v��
    private int direction = 1;  //��������p�ϐ�
    public PhysicsMaterial2D bounce, normal;    //�W�����v����PhysicsMaterial

    private float moveInput;    //�����L�[���͎�t�p
    private float jumpForce = 0.0f; //���݂̃W�F���v�͗p�ϐ�

    private bool isGrounded;    //�ڒn����p�ϐ�
    [SerializeField] private ContactFilter2D filter2d = default;    //�ڒn����p�̐ڐG����

    void Start()
    {
        this.playerRigid = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //���s�A�j���[�V�����̍Đ����x�ǉ��v���O�����i�A�j���[�V�����N���b�v���쐬�������ɃR�����g�A�E�g����j��Animator��Component���擾    
    }

    void Update()
    {
        //�ڒn������s��
        isGrounded = playerRigid.IsTouching(filter2d);

        //���E�Ɉړ�����
        moveInput = Input.GetAxisRaw("Horizontal");

        if (jumpForce == 0.0f && isGrounded)
        {
            playerRigid.velocity = new Vector2(moveInput * walkSpeed, playerRigid.velocity.y);

            //�A�j���[�V��������
            if (moveInput == 0.0f)
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                animator.SetTrigger("Run");
            }
        }

        //���������Ă���Ƃ����]������
        if (moveInput * direction < 0 && isGrounded)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            direction *= -1;
        }

        //�L�[�������ꂽ��W�����v�J�n������s��
        if (Input.GetKey("space") && isGrounded)
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce += 0.4f;  //1�t���[�����Ƃɉ�����W�����v��
                animator.SetTrigger("Squat");   //�A�j���[�V��������
            }
            else
            {
                jumpForce = maxJumpForce;   //�ő�W�����v�͂ɒB������Œ�
                animator.SetTrigger("SquatMax");    //�A�j���[�V��������
            }

            //���E�ړ����~�߂�
            playerRigid.velocity = new Vector2(0.0f, playerRigid.velocity.y);

            //PhysicsMaterial���W�����v���p�̂��̂ɕς���
            playerRigid.sharedMaterial = bounce;
        }

        //�L�[�������ꂽ��W�����v�����s
        if (Input.GetKeyUp("space") && jumpForce > 0.0f)
        {
            //�W�����v�͂����l�ɒB���Ă��Ȃ��ꍇ�̓W�����v�����Ȃ�
            if(jumpForce < 2.4f)
            {
                ResetJumpForce();

                animator.Play("Idle");  //�A�j���[�V��������

                playerRigid.sharedMaterial = normal;    //PhysicsMaterial��ʏ펞�p�ɖ߂�
            }
            else
            {
                float tmpX = moveInput * walkSpeed * 2;
                float tmpY = jumpForce;

                playerRigid.velocity = new Vector2(tmpX, tmpY);

                animator.SetTrigger("Jump");    //�A�j���[�V��������

                Invoke("ResetJumpForce", 0.02f);    //�W�����v��ɃW�����v�͂����Z�b�g
            }
        }

        //��������
        if (playerRigid.velocity.y <= -1)
        {
            playerRigid.sharedMaterial = normal;    //PhysicsMaterial��ʏ펞�p�ɖ߂�

            animator.SetTrigger("Fall");    //�A�j���[�V��������
        }

        //���n��̃A�j���[�V��������
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isGrounded)
        {
            animator.Play("Idle");
        }
    }

    //�S�[���ɓ���
    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("ClearScene"); // ClearScene�Ɉړ�����B
    }

    //�W�����v�͂����Z�b�g���郁�\�b�h
    void ResetJumpForce()
    {
        jumpForce = 0.0f;
    }
}
