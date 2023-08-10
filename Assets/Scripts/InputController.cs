using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private GameObject buttons;


    void Start()
    {
        InitInput();
    }
    private void InitInput()
    {
        if (IsMobile())
        {
            buttons.SetActive(true);
        }
    }
    private bool IsMobile()
    {
        if (Application.isMobilePlatform)
        {
            return true;
        }
        return false;

    }
}
