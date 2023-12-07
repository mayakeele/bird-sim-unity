using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static List<Vector3> AddVector(this List<Vector3> vectorList, Vector3 addedVector){
        // Adds the given vector to every element in the list
        List<Vector3> sumVector = new List<Vector3>();
        foreach(Vector3 vect in vectorList){
            sumVector.Add(vect + addedVector);
        }
        return sumVector;
    }
    public static List<Vector3> AddVector(this List<Vector3> vectorList, float addedX, float addedY, float addedZ){
        // Adds the given vector to every element in the list
        List<Vector3> sumVector = new List<Vector3>();
        foreach(Vector3 vect in vectorList){
            sumVector.Add(vect + new Vector3(addedX, addedY, addedZ));
        }
        return sumVector;
    }

    
    public static Vector3 ProjectHorizontal(this Vector3 inVector){
        // Projects the input vector onto the XZ plane
        return Vector3.ProjectOnPlane(inVector, Vector3.up);
    }


    public static Vector3 Average(this List<Vector3> vectorList){
        // Calculates the average of all vectors in the list
        Vector3 sum = Vector3.zero;
        int numItems = vectorList.Count;

        foreach(Vector3 item in vectorList){
            sum += item;
        }

        return sum / numItems;
    }


    public static Vector3 MaxComponents(this List<Vector3> vectorList){
        // Returns the maximum x, y, and z component of the Vectors contained in the given list
        float xMax = Mathf.NegativeInfinity;
        float yMax = Mathf.NegativeInfinity;
        float zMax = Mathf.NegativeInfinity;

        foreach(Vector3 currVector in vectorList){
            xMax = Mathf.Max(xMax, currVector.x);
            yMax = Mathf.Max(yMax, currVector.y);
            zMax = Mathf.Max(zMax, currVector.z);
        }

        return new Vector3(xMax, yMax, zMax);
    }

    public static Vector3 MinComponents(this List<Vector3> vectorList){
        // Returns the maximum x, y, and z component of the Vectors contained in the given list
        float xMin = Mathf.Infinity;
        float yMin = Mathf.Infinity;
        float zMin = Mathf.Infinity;

        foreach(Vector3 currVector in vectorList){
            xMin = Mathf.Min(xMin, currVector.x);
            yMin = Mathf.Min(yMin, currVector.y);
            zMin = Mathf.Min(zMin, currVector.z);
        }

        return new Vector3(xMin, yMin, zMin);
    }


    public static List<Vector3> TransformPoints(this Transform transform, List<Vector3> localVectors){
        // Transforms a list of positions from world space to local space
        List<Vector3> worldVectors = new List<Vector3>();

        foreach(Vector3 localVector in localVectors){
            Vector3 newVector = transform.TransformPoint(localVector);
            worldVectors.Add(newVector);
        }

        return worldVectors;
    }

    public static List<Vector3> InverseTransformPoints(this Transform transform, List<Vector3> worldVectors){
        /// Transforms a list of positions from world space to local space
        List<Vector3> localVectors = new List<Vector3>();

        foreach(Vector3 worldVector in worldVectors){
            Vector3 newVector = transform.InverseTransformPoint(worldVector);
            localVectors.Add(newVector);
        }

        return localVectors;
    }


    public static bool IsNaN(this Vector3 v){
        // Returns true if any component of the vector is NaN, otherwise return false
        if(float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z)){
            return true;
        }
        else{
            return false;
        }
    }

    public static bool IsRealNumber(this Vector3 v){
        // Returns false if the any components are NaN, positive infinity, or negative infinity
        if(v.x.IsRealNumber() && v.y.IsRealNumber() && v.z.IsRealNumber()){
            return true;
        }
        else{
            return false;
        }
    }


    public static Vector3 ClampMagnitude(this Vector3 v, float min, float max){
        // Returns a vector in the same direction as v, with its magnitude clamped between min and max

        if(v.magnitude > max){ 
           return v.normalized * max;
        }

        else if(v.magnitude < min){ 
            return v.normalized * min; 
        }

        else{
            return v;
        }
    }

    public static Vector2 AltitudeAzimuthBetween(Vector3 startPos, Vector3 endPos, Vector3 perspectivePos){
        // Returns the horizontal and vertical (azimuth and altitude) angle between two vectors when seen from a perspective postion

        Vector3 startDir = (startPos - perspectivePos).normalized;
        Vector3 endDir = (endPos = perspectivePos).normalized;

        Vector3 middlePlaneNormal = startPos - endPos;

        float hAngle = Vector3.Angle(startDir.ProjectHorizontal(), endDir.ProjectHorizontal());    
        float vAngle = Vector3.Angle(Vector3.ProjectOnPlane(startDir, middlePlaneNormal), Vector3.ProjectOnPlane(endDir, middlePlaneNormal));
        
        return new Vector2(hAngle, vAngle);
    }
}
