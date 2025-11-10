using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float jumpForce = 7f;
    public Transform cam;

    private Rigidbody rb;
    private float yaw;
    private float pitch;

    private bool cursorLocked = true;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Verifica se a câmera foi atribuída
        if (cam == null)
        {
            Debug.LogError("PlayerController: A variável 'cam' não foi atribuída no Inspector! O script será desativado.");
            enabled = false;
            return;
        }

        // Inicializa yaw e pitch com base na rotação atual
        yaw = transform.eulerAngles.y;
        pitch = cam.localEulerAngles.x;
        // Corrige pitch para range -180 a 180
        if (pitch > 180) pitch -= 360;

        LockCursor(true);
    }

    void Update()
    {
        // Só processa input se o script estiver ativo e a câmera atribuída
        if (cam != null)
            HandleInput();
    }

    void HandleInput()
    {
        // --- TOGGLE CURSOR ---
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false); // pausa, cursor livre
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            LockCursor(true); // volta ao jogo
        }

        // --- MOVIMENTO ---
        if (cursorLocked)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Garante que cam está atribuída antes de usar
            if (cam != null)
            {
                Vector3 move = (cam.forward * v + cam.right * h).normalized * moveSpeed;
                rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
            }

            // --- JUMP ---
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset Y velocity before jump
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
        else
        {
            // Quando o cursor está livre, para o movimento horizontal/vertical
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // --- CÂMERA / MOUSE LOOK ---
        if (cursorLocked && cam != null)
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            yaw += mouseX * lookSpeed;
            pitch -= mouseY * lookSpeed;
            // Removido o clamp para permitir rotação completa em 3D
            // pitch = Mathf.Clamp(pitch, -85f, 85f);

            // Rotaciona o player em yaw (Y) e a câmera em pitch (X)
            transform.rotation = Quaternion.Euler(0, yaw, 0);
            cam.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }

    void LockCursor(bool locked)
    {
        cursorLocked = locked;
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Detecta se está no chão para permitir pulo
    void OnCollisionStay(Collision collision)
    {
        // Considera grounded se tocar em qualquer coisa que não seja trigger
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
