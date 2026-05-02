using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class make_coffee : MonoBehaviour
{
    public GameObject coffee;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coffee.SetActive(false);
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        coffee.SetActive(true);
    }
}
