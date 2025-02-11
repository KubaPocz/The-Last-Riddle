using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using TMPro;

public class OutlineControler : MonoBehaviour
{
    private Camera playerCamera;
    private float maxDistance = 3f;
    private Outline outline;
    public enum interaction
    {
        wez,
        daj,
        porozmawiaj,
        otworz,
        czytaj,
        uzyj,
        gotuj
    }
    public interaction Interakcja;
    void Start()
    {
        playerCamera = Camera.main;
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
        }

    }
}
