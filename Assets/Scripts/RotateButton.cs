using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateButton : MonoBehaviour
{
    public Rubiks.RubiksColors color;
    public bool clockwise;
    public Rubiks cube;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => cube.Rotate(color, clockwise));
    }
}
