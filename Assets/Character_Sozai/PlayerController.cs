using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //LoadSceneを使うために必要

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRigid;    //PlayerのRigidbody2D
    Animator animator;  //歩行アニメーションの再生速度追加プログラム（アニメーションクリップを作成した時にコメントアウトする）

    public float walkSpeed = 6.0f;  //プレイヤーの移動速度
    public float maxJumpForce = 20f;    //プレイヤーの最大ジャンプ力
    private int direction = 1;  //方向判定用変数
    public PhysicsMaterial2D bounce, normal;    //ジャンプ中のPhysicsMaterial

    private float moveInput;    //方向キー入力受付用
    private float jumpForce = 0.0f; //現在のジェンプ力用変数

    private bool isGrounded;    //接地判定用変数
    [SerializeField] private ContactFilter2D filter2d = default;    //接地判定用の接触条件

    void Start()
    {
        this.playerRigid = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();  //歩行アニメーションの再生速度追加プログラム（アニメーションクリップを作成した時にコメントアウトする）※AnimatorのComponentを取得    
    }

    void Update()
    {
        //接地判定を行う
        isGrounded = playerRigid.IsTouching(filter2d);

        //左右に移動する
        moveInput = Input.GetAxisRaw("Horizontal");

        if (jumpForce == 0.0f && isGrounded)
        {
            playerRigid.velocity = new Vector2(moveInput * walkSpeed, playerRigid.velocity.y);

            //アニメーション処理
            if (moveInput == 0.0f)
            {
                animator.SetTrigger("Idle");
            }
            else
            {
                animator.SetTrigger("Run");
            }
        }

        //左を向いているとき反転させる
        if (moveInput * direction < 0 && isGrounded)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            direction *= -1;
        }

        //キーが押されたらジャンプ開始判定を行う
        if (Input.GetKey("space") && isGrounded)
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce += 0.4f;  //1フレームごとに加えるジャンプ力
                animator.SetTrigger("Squat");   //アニメーション処理
            }
            else
            {
                jumpForce = maxJumpForce;   //最大ジャンプ力に達したら固定
                animator.SetTrigger("SquatMax");    //アニメーション処理
            }

            //左右移動を止める
            playerRigid.velocity = new Vector2(0.0f, playerRigid.velocity.y);

            //PhysicsMaterialをジャンプ中用のものに変える
            playerRigid.sharedMaterial = bounce;
        }

        //キーが離されたらジャンプを実行
        if (Input.GetKeyUp("space") && jumpForce > 0.0f)
        {
            //ジャンプ力が一定値に達していない場合はジャンプさせない
            if(jumpForce < 2.4f)
            {
                ResetJumpForce();

                animator.Play("Idle");  //アニメーション処理

                playerRigid.sharedMaterial = normal;    //PhysicsMaterialを通常時用に戻す
            }
            else
            {
                float tmpX = moveInput * walkSpeed * 2;
                float tmpY = jumpForce;

                playerRigid.velocity = new Vector2(tmpX, tmpY);

                animator.SetTrigger("Jump");    //アニメーション処理

                Invoke("ResetJumpForce", 0.02f);    //ジャンプ後にジャンプ力をリセット
            }
        }

        //落下処理
        if (playerRigid.velocity.y <= -1)
        {
            playerRigid.sharedMaterial = normal;    //PhysicsMaterialを通常時用に戻す

            animator.SetTrigger("Fall");    //アニメーション処理
        }

        //着地後のアニメーション処理
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall") && isGrounded)
        {
            animator.Play("Idle");
        }
    }

    //ゴールに到着
    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("ClearScene"); // ClearSceneに移動する。
    }

    //ジャンプ力をリセットするメソッド
    void ResetJumpForce()
    {
        jumpForce = 0.0f;
    }
}
