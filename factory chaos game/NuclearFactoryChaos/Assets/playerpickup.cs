using UnityEngine;

public class PlayerPickupSimple : MonoBehaviour
{
    public Camera playerCamera; // Podes arrastar a câmera aqui diretamente (alternativa ao Camera.main)
    public Transform holdPoint; 
    public float throwForce = 10f;
    public float pickupDistance = 10f;

    private Rigidbody heldObject;
    private bool wasKinematic;

    void Start()
    {
        // Se não atribuíste a câmera manualmente, tenta encontrar
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        // Debug: lista todas as câmeras na cena
        Camera[] allCameras = FindObjectsOfType<Camera>();
        Debug.Log($"[PlayerPickup] Câmeras encontradas na cena: {allCameras.Length}");
        foreach (Camera cam in allCameras)
        {
            Debug.Log($"[PlayerPickup] - Câmera: '{cam.name}', Tag: '{cam.tag}', Ativa: {cam.gameObject.activeInHierarchy}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[PlayerPickup] Tecla E pressionada");
            if (heldObject == null)
            {
                Debug.Log("[PlayerPickup] Tentando pegar objeto...");
                TryPickup();
            }
            else
            {
                Debug.Log("[PlayerPickup] Soltando objeto...");
                DropObject();
            }
        }

        // Atirar
        if (heldObject != null && Input.GetMouseButtonDown(0))
        {
            Debug.Log("[PlayerPickup] Atirando objeto...");
            ThrowObject();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null && holdPoint != null)
        {
            // Segura o objeto à frente da câmera (usar FixedUpdate para física)
            heldObject.MovePosition(holdPoint.position);
            heldObject.velocity = Vector3.zero;
            heldObject.angularVelocity = Vector3.zero;
        }
    }

    void TryPickup()
    {
        Debug.Log("[PlayerPickup] TryPickup() chamado - INÍCIO");
        
        // Usa a câmera atribuída ou tenta Camera.main
        Camera cam = playerCamera != null ? playerCamera : Camera.main;
        
        if (cam == null)
        {
            Debug.LogError("[PlayerPickup] ❌ Nenhuma câmera encontrada!");
            Debug.LogError("[PlayerPickup] SOLUÇÃO: Arrasta a câmera para o campo 'Player Camera' no Inspector, OU");
            Debug.LogError("[PlayerPickup] Certifica-te de que a câmera tem a tag 'MainCamera' (sem espaço!)");
            return;
        }
        Debug.Log($"[PlayerPickup] ✓ Câmera encontrada: {cam.name}, Tag: '{cam.tag}'");

        if (holdPoint == null)
        {
            Debug.LogError("[PlayerPickup] ❌ holdPoint não está atribuído! Atribua um Transform no Inspector.");
            return;
        }
        Debug.Log($"[PlayerPickup] ✓ holdPoint encontrado: {holdPoint.name}");

        Debug.Log($"[PlayerPickup] Fazendo raycast com distância: {pickupDistance}");
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            Debug.Log($"[PlayerPickup] Raycast acertou: {hit.collider.name}, Tag: {hit.collider.tag}");
            
            if (hit.collider.CompareTag("Pickup"))
            {
                Debug.Log("[PlayerPickup] Objeto tem tag 'Pickup'");
                
                if (hit.collider.attachedRigidbody != null)
                {
                    Debug.Log($"[PlayerPickup] Objeto pego com sucesso: {hit.collider.name}");
                    heldObject = hit.collider.attachedRigidbody;
                    wasKinematic = heldObject.isKinematic;
                    heldObject.isKinematic = false;
                    heldObject.useGravity = false;
                    heldObject.velocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;
                }
                else
                {
                    Debug.LogWarning($"[PlayerPickup] Objeto '{hit.collider.name}' não tem Rigidbody!");
                }
            }
            else
            {
                Debug.Log($"[PlayerPickup] Objeto '{hit.collider.name}' não tem a tag 'Pickup' (tag atual: '{hit.collider.tag}')");
            }
        }
        else
        {
            Debug.Log("[PlayerPickup] Raycast não acertou nada");
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Debug.Log($"[PlayerPickup] Objeto solto: {heldObject.name}");
            heldObject.useGravity = true;
            heldObject.isKinematic = wasKinematic;
            heldObject = null;
        }
        else
        {
            Debug.LogWarning("[PlayerPickup] Tentou soltar mas não há objeto segurado!");
        }
    }

    void ThrowObject()
    {
        Camera cam = playerCamera != null ? playerCamera : Camera.main;
        
        if (heldObject != null && cam != null)
        {
            Debug.Log($"[PlayerPickup] Objeto atirado: {heldObject.name} com força: {throwForce}");
            heldObject.useGravity = true;
            heldObject.isKinematic = wasKinematic;
            heldObject.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
            heldObject = null;
        }
        else
        {
            Debug.LogWarning("[PlayerPickup] Tentou atirar mas não há objeto segurado ou câmera não encontrada!");
        }
    }
}

