using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTest : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Myszka jest nad UI!");
        }
    }
}
