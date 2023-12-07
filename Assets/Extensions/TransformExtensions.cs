using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    
    public static List<Vector3> GetPositions(this List<Transform> transforms){
        // Gets a list of the positions of all transforms in the given list
        List<Vector3> posList = new List<Vector3>();
        
        foreach (Transform trans in transforms){
            posList.Add(trans.position);
        }

        return posList;
    }

    public static List<Quaternion> GetRotations(this List<Transform> transforms){
        // Gets a list of the positions of all transforms in the given list
        List<Quaternion> rotList = new List<Quaternion>();
        
        foreach (Transform trans in transforms){
            rotList.Add(trans.rotation);
        }
        
        return rotList;
    }



    public static IEnumerator ScaleOverDurationUpdate(Transform scaledTransform, float duration, float initialScale, float finalScale){
        // Scales the given transform from initial to final scale over time
        float currentTime = 0;

        while(currentTime < duration){
            float timeGradient = currentTime / duration;

            float currentScale = Mathf.Lerp(initialScale, finalScale, timeGradient);

            scaledTransform.localScale = currentScale * Vector3.one;

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        scaledTransform.localScale = finalScale * Vector3.one;
    }

    public static IEnumerator ScaleOverDurationUpdate(Transform scaledTransform, float duration, AnimationCurve scaleCurve){
        // Scales the given transform from initial to final scale over time
        float currentTime = 0;

        while(currentTime < duration){
            float timeGradient = currentTime / duration;

            float currentScale = scaleCurve.Evaluate(timeGradient);

            scaledTransform.localScale = currentScale * Vector3.one;

            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        scaledTransform.localScale = scaleCurve.Evaluate(1) * Vector3.one;
    }



    public static IEnumerator ScaleOverDurationFixedUpdate(Transform scaledTransform, float duration, float initialScale, float finalScale){
        // Scales the given transform from initial to final scale over time
        float currentTime = 0;

        while(currentTime < duration){
            float timeGradient = currentTime / duration;

            float currentScale = Mathf.Lerp(initialScale, finalScale, timeGradient);

            scaledTransform.localScale = currentScale * Vector3.one;

            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        scaledTransform.localScale = finalScale * Vector3.one;
    }

    public static IEnumerator ScaleOverDurationFixedUpdate(Transform scaledTransform, float duration, AnimationCurve scaleCurve){
        // Scales the given transform from initial to final scale over time
        float currentTime = 0;

        while(currentTime < duration){
            float timeGradient = currentTime / duration;

            float currentScale = scaleCurve.Evaluate(timeGradient);

            scaledTransform.localScale = currentScale * Vector3.one;

            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        scaledTransform.localScale = scaleCurve.Evaluate(1) * Vector3.one;
    }
}
