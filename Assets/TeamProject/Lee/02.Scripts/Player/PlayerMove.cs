using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController Player_Controller;
    private Transform Camera_Pivot; //ī�޶� x�� ȸ��
    private Transform Player_Transform;
    public AudioSource PlayerSource;
    private AudioClip Walk;
    private AudioClip Run;
    private AudioClip Jump;

    private Vector3 Velocity_y = Vector3.zero; //�߷�

    private float moveSpeed;
    private float currentXRot = 0f;

    private readonly float rotSpeed = 5.0f;
    private readonly float Gravity = -25f;
    private readonly float JumpHeight = 0.4f;

    [SerializeField]private Vector3 playerDir = Vector3.zero; //�̵� ����
    public Vector3 PlayerDir
    { 
        get { return playerDir; } 
        set { playerDir = value; } 
    }
    private Vector2 playerRot = Vector2.zero; //ȸ�� ����
    public Vector2 PlayerRot
    {
        get { return playerRot; }
        set { playerRot = value; }
    }
    private bool player_isRun; //�޸��� ���� üũ
    public bool Player_isRun
    {
        get { return player_isRun; }
        set { player_isRun = value; }
    }
    private bool Player_isJump;
    public bool Player_IsJump
    {
        get { return Player_isJump; }
        set
        {
            Player_isJump = value;
        }
    }
    private bool isGround;
    public bool IsGround
    {
        get { return isGround; }
        set { isGround = value; }
    }

    void Awake()
    {
        Player_Transform = transform;
        Player_Controller = GetComponent<CharacterController>();
        Camera_Pivot = transform.GetChild(0).GetComponent<Transform>();

        Walk = Resources.Load<AudioClip>("Sound/Player/Walk");
        Run = Resources.Load<AudioClip>("Sound/Player/Run");
        Jump = Resources.Load<AudioClip>("Sound/Player/Jump");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        InGameSoundManager.instance.ActiveSound(gameObject, Walk, 2.0f, false, true, true, 1);
        PlayerSource = InGameSoundManager.instance.Data["Walk"].SoundBox_AudioSource;
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover)
        {
            PlayerSource.Stop();
            return;
        }
        
        CheckJumpState();
        Player_Moving();
        Camera_Moving();
    }

    private void Player_Moving()
    {
        if(PlayerDir == Vector3.zero || !IsGround) //�������� ���� ������ �Ҹ� ����
        {
            PlayerSource.Stop();
        }
        else if (!Player_isRun) //�ȱ�
        {
            moveSpeed = 3.0f;
            if (PlayerSource.clip != Walk || !PlayerSource.isPlaying)
            {
                PlayerSource.clip = Walk;
                PlayerSource.Play();
            }
        }
        else if (Player_isRun && PlayerDir.z >= 1.0f) //wŰ�� ������ �� �޸���
        {
            moveSpeed = 7.0f;
            if (PlayerSource.clip != Run || !PlayerSource.isPlaying)
            {
                PlayerSource.clip = Run;
                PlayerSource.Play();
            }
        }
        else if (Player_isRun && PlayerDir.z >= 0.7f) //w�� ad Ű�� ������ �� �޸���
        {
            moveSpeed = 5.0f;
            if (PlayerSource.clip != Run || !PlayerSource.isPlaying)
            {
                PlayerSource.clip = Run;
                PlayerSource.Play();
            }
        }

        Vector3 move = transform.TransformDirection(PlayerDir);
        Player_Controller.Move(move * moveSpeed * Time.deltaTime);
    }
    private void CheckJumpState()
    {
        if (IsGround)
        {
            Velocity_y.y = 0;
            if (Player_IsJump)
            {
                Velocity_y.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }
        }
        else
        {
            Velocity_y.y += Gravity * Time.deltaTime;
            Player_IsJump = false;
        }
        Player_Controller.Move(Velocity_y * Time.deltaTime);
    }
    private void Camera_Moving()
    {
        Player_Transform.Rotate(Vector3.up * PlayerRot.x * rotSpeed * Time.deltaTime);

        currentXRot -= playerRot.y * rotSpeed * Time.deltaTime;
        currentXRot = Mathf.Clamp(currentXRot, -45f, 45f);
        Camera_Pivot.localRotation = Quaternion.Euler(currentXRot, 0f, 0f);
    }
}
