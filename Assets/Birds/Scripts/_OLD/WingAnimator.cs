using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingAnimator : MonoBehaviour
{
    // Alpha (-90 to 90)
    float currentAlpha;
    float targetAlpha;

    // Dihedral (-90 to 90)
    float currentDihedral;
    float targetDihedral;

    // Extension (0 to 1)
    float currentExtension;
    float targetExtension;


    void Start(){
        
    }

    void Update(){
        AnimateAlpha();
    }



    public void SetTargetAlpha(float alpha){
        targetAlpha = alpha;
    }

    public void AnimateAlpha(){
        // Animates the wings by lerping them towards the given alpha

        transform.localRotation = Quaternion.Euler(-targetAlpha, 0, 0);
    }
}
