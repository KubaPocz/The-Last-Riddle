using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class EndCodeController : MonoBehaviour
{
    private DoorController doorController;
    private GameManager gameManager;
    private Camera playerCamera;
    public GameObject codeWindow;
    public TextMeshProUGUI codeInput;
    public GameObject end;
    private bool ending = false;
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        doorController = GetComponent<DoorController>();
        playerCamera = Camera.main;
        doorController.canOpen = false;
        codeWindow.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.firstPersonController.enabled && codeWindow != null)
                    {
                        gameManager.ShowEndCodeWindow(codeWindow);
                    }
                    else if (!gameManager.firstPersonController.enabled)
                    {
                        StartCoroutine(gameManager.HideEndCodeWindow(codeWindow));
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled && gameManager.codeWindowOn)
                {
                    StartCoroutine(gameManager.HideEndCodeWindow(codeWindow));
                }
            }
        }
        if (ending)
        {
            gameManager.staminaBar.SetActive(false);
            gameManager.firstPersonController.enabled = false;
            gameManager.firstPersonController.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameManager.firstPersonController.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Vector3 direction = end.transform.position - playerCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRotation, 0.5f* Time.deltaTime);
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, end.transform.position, 0.5f * Time.deltaTime);
            if (Vector3.Distance(playerCamera.transform.position, end.transform.position) < 0.5f)
            {
                playerCamera.transform.position = end.transform.position;
                gameManager.Ending();
                Destroy(gameObject);
            }           
        }
    }
    public void VerifyCode()
    {
        if (CheckCode())
        {
            doorController.canOpen = true;
            doorController.animator.SetTrigger("doorSwitch");
            StartCoroutine(gameManager.HideEndCodeWindow(codeWindow));
            gameObject.tag = "Untagged";
            StartCoroutine(EndingOn());
        }
        else
        {
            gameManager.error.text = LocalizationManager.Instance.GetText("WrongCode");
            StartCoroutine(gameManager.ClearError());
        }
    }
    bool CheckCode()
    {
        bool codeOK = true;
        string[] codeText = codeInput.text.Trim().Split(" ");
        if (codeText.Length == gameManager.code.Length)
        {
            for (int i = 0; i < gameManager.code.Length; i++)
            {
                codeText[i] = codeText[i].Replace("\u200B", "").Trim();
                if (codeText[i].Trim().ToLower() != gameManager.code[i].Trim().ToLower())
                {
                    codeOK = false;
                }
            }
        }
        else
            codeOK = false;
        return codeOK;
    }
    IEnumerator EndingOn()
    {
        yield return new WaitForSeconds(1f);
        ending = true;
    }
}