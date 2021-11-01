using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpanwer : MonoBehaviour
{
    public PieceType type;
    public Piece currentPiece;
    public void Spawn()
    {
        int amountObj = 0;
        switch (type)
        {
            case PieceType.jump:
                amountObj = LevelManager.instance.jump.Count;
                break;

            case PieceType.ramp:
                amountObj = LevelManager.instance.ramp.Count;
                break;

            case PieceType.longBlock:
                amountObj = LevelManager.instance.longBlocks.Count;
                break;

            case PieceType.slide:
                amountObj = LevelManager.instance.slides.Count;
                break;
        }
        currentPiece = LevelManager.instance.GetPiece(type, Random.Range(0,amountObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}
