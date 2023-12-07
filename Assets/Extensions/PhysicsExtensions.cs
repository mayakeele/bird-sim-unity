using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsExtensions
{
    public static bool IsInsideCollider(this Vector3 point, Collider c)
    {
        Vector3 closest = c.ClosestPoint(point);
        if (closest == point){
            return true;
        }
        else{
            return false;
        }
    }


    public static bool ContainsLayer(this LayerMask layerMask, int layer){
        return layerMask == (layerMask | (1 << layer));
    }
    public static bool ContainsLayer(this LayerMask layerMask, string layerName){
        int layer = LayerMask.NameToLayer(layerName);
        return layerMask == (layerMask | (1 << layer));
    }
}
