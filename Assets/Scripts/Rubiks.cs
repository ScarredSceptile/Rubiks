using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Rubiks : MonoBehaviour
{
    public enum RubiksColors
    {
        Red,
        Blue,
        Yellow,
        Green,
        White,
        Orange
    }

    private GameObject[,,] cube = new GameObject[3,3,3];
    [SerializeField]
    private GameObject[] cubeList = new GameObject[27];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    cube[i, j, k] = cubeList[i * 9 + j * 3 + k];
                }
            }
        }
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < 25; i++)
        {
            Rotate((RubiksColors)Random.Range(0, 6), Random.Range(0,2) == 1);
        }
    }

    private GameObject[,] RotateArray(GameObject[,] arr, bool clockwise)
    {
        if (clockwise)
        {
            for (int i = 0; i < 1.5f; i++)
            {
                for (int j = i; j < 3 - i - 1; j++)
                {
                    GameObject temp = arr[i, j];

                    arr[i, j] = arr[2 - j, i];
                    arr[2 - j, i] = arr[2 - i, 2 - j];
                    arr[2 - i, 2 - j] = arr[j, 2 - i];
                    arr[j, 2 - i] = temp;
                }
            }
        }

        else
        {
            for (int i = 0; i < 1.5f; i++)
            {
                for (int j = i; j < 3 - i - 1; j++)
                {
                    GameObject temp = arr[i, j];

                    arr[i, j] = arr[j, 2 - i];
                    arr[j, 2 - i] = arr[2 - i, 2 - j];
                    arr[2 - i, 2 - j] = arr[2 - j, i];
                    arr[2 - j, i] = temp;
                }
            }
        }

        return arr;
    }

    private void RotateTop(int arrayLocation, int zMult, int xMult, int rotation, int yMult, bool clockwise)
    {
        GameObject[,] temp = new GameObject[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                temp[i, j] = cube[arrayLocation, i, j];

                float x = cube[arrayLocation, i, j].transform.localPosition.x;
                float z = cube[arrayLocation, i, j].transform.localPosition.z;

                cube[arrayLocation, i, j].transform.localPosition = new Vector3(z * zMult, 1.1f * yMult, x * xMult);

                cube[arrayLocation, i, j].transform.Rotate(new Vector3(0, rotation, 0), Space.World);
            }
        }

        RotateArray(temp, clockwise);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cube[arrayLocation, i, j] = temp[i, j];
            }
        }
    }

    private void RotateSides(int arrayLocation, int yMult, int xMult, int rotation, int zMult, bool clockwise)
    {
        GameObject[,] temp = new GameObject[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                temp[i, j] = cube[i, arrayLocation, j];

                float x = cube[i, arrayLocation, j].transform.localPosition.x;
                float y = cube[i, arrayLocation, j].transform.localPosition.y;

                cube[i, arrayLocation, j].transform.localPosition = new Vector3(y * yMult, x * xMult, 1.1f * zMult);

                cube[i, arrayLocation, j].transform.Rotate(new Vector3(0, 0, rotation), Space.World);
            }
        }

        RotateArray(temp, clockwise);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cube[i, arrayLocation, j] = temp[i, j];
            }
        }
    }

    private void RotateFrontBack(int arrayLocation, int zMult, int yMult, int rotation, int xMult, bool clockwise)
    {
        GameObject[,] temp = new GameObject[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                temp[i, j] = cube[i, j, arrayLocation];

                float y = cube[i, j, arrayLocation].transform.localPosition.y;
                float z = cube[i, j, arrayLocation].transform.localPosition.z;

                cube[i, j, arrayLocation].transform.localPosition = new Vector3(1.1f * xMult, z * zMult, y * yMult);

                cube[i, j, arrayLocation].transform.Rotate(new Vector3(rotation, 0, 0), Space.World);
            }
        }

        RotateArray(temp, clockwise);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cube[i, j, arrayLocation] = temp[i, j];
            }
        }
    }

    public void Rotate(RubiksColors side, bool clockwise)
    {
        switch (side)
        {
            case RubiksColors.Red:
                //Around 0, 1, 1
                if (clockwise)
                {
                    RotateTop(0, 1, -1, 90, 1, clockwise);
                }

                else
                {
                    RotateTop(0, -1, 1, -90, 1, clockwise);
                }

                break;
            case RubiksColors.Blue:
                //Around 1, 0, 1
                if (clockwise)
                {
                    RotateSides(0, 1, -1, -90, -1, !clockwise);
                }

                else
                {
                    RotateSides(0, -1, 1, 90, -1, !clockwise);
                }
                break;
            case RubiksColors.Yellow:
                //Around 1, 1, 2
                if (clockwise)
                {
                    RotateFrontBack(2, 1, -1, -90, -1, !clockwise);
                }

                else
                {
                    RotateFrontBack(2, -1, 1, 90, -1, !clockwise);
                }
                break;
            case RubiksColors.Green:
                //Around 1, 2, 1
                if (clockwise)
                {
                    RotateSides(2, -1, 1, 90, 1, clockwise);
                }

                else
                {
                    RotateSides(2, 1, -1, -90, 1, clockwise);
                }
                break;
            case RubiksColors.White:
                //Around 1, 1, 0
                if (clockwise)
                {
                    RotateFrontBack(0, -1, 1, 90, 1, clockwise);
                }

                else
                {
                    RotateFrontBack(0, 1, -1, -90, 1, clockwise);
                }
                break;
            case RubiksColors.Orange:
                //Around 2, 1, 1
                if (clockwise)
                {
                    RotateTop(2, -1, 1, -90, -1, !clockwise);
                }

                else
                {
                    RotateTop(2, 1, -1, 90, -1, !clockwise);
                }
                break;
        }
    }

    [UnityEditor.CustomEditor(typeof(Rubiks))]
    public class UIElementEditor : UnityEditor.Editor
    {
        //-------------------------------------------------
        // Custom Inspector GUI allows us to click from within the UI
        //-------------------------------------------------
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Rubiks uiElement = (Rubiks)target;
            if (GUILayout.Button("Shuffle"))
            {
                uiElement.Shuffle();
            }
        }
    }

}
