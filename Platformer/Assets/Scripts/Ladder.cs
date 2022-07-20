using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum ladderPart { complete, bottom, top};
    [SerializeField] ladderPart part = ladderPart.complete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case ladderPart.complete:
                    player.canClimb = true;
                    break;
                    
                case ladderPart.bottom:
                    player.bottomLadder = true;
                    break;

                case ladderPart.top:
                    player.topLadder = true;
                    break;

                default:
                    break;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case ladderPart.complete:
                    player.canClimb = false;
                    player.ladder = this;
                    break;

                case ladderPart.bottom:
                    player.bottomLadder = false;
                    break;

                case ladderPart.top:
                    player.topLadder = false;
                    break;

                default:
                    break;
            }
        }
    }
}
