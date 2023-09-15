using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public void PlayTetrominoLanding(GameObject tetromino)
    {
        Animator animator = tetromino.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Land");
    }
}
