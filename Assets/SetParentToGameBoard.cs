using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentToGameBoard : MonoBehaviour
{
    void Start()
    {
        ParentToGameBoard();
        Invoke("ParentToGameBoard", 1f);
    }

    void ParentToGameBoard()
    {
        GameObject gameBoard = GameObject.FindWithTag("GameBoard");
        if (gameBoard != null)
        {
            transform.parent = gameBoard.transform;
        }
    }
}
