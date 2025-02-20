using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElderController : MonoBehaviour
{
    public TextMeshProUGUI scrollElderTextTMP;
    public Image scrollElderImage;
    public Sprite scrollNumber;
    public GameObject elderScroll;
    public int codeID;

    private Camera playerCamera;
    private GameManager gameManager;
    private void Awake()
    {
        elderScroll.SetActive(false);
    }
    void Start()
    {
        playerCamera = Camera.main;
        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (gameManager.firstPersonController.enabled)
                    {
                        scrollElderImage.sprite = scrollNumber;
                        gameManager.UpdateCodeText(codeID - 1);
                        gameManager.ShowScrollElder(elderScroll, scrollElderTextTMP, gameManager.code[codeID - 1]);
                    }
                    else if (!gameManager.firstPersonController.enabled)
                    {
                        StartCoroutine(gameManager.HideScrollElder(elderScroll));
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.firstPersonController.enabled && gameManager.scrollOn)
                {
                    StartCoroutine(gameManager.HideScrollElder(elderScroll));
                }
            }
        }
    }
}
