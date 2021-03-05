using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;

public class Floor : MonoBehaviour
{
    public int squareCount = 1;
    private GameObject myGrid;

    // Start is called before the first frame update
    void OnEnable()
    {
        myGrid=Instantiate(Resources.Load("Grid")) as GameObject;
    }

    public void ResetSquare()
    {
        Destroy(myGrid);
        myGrid = Instantiate(Resources.Load("Grid"), new Vector3(0,0,0), Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
