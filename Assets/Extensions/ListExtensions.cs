using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class ListExtensions
{
    
    public static List<int> FindIndices<T>(this T[] array, T target){
        List<int> indices = new List<int>();

        for(int i = 0; i < array.Length; i++){
            if (array[i].Equals(target)){
                indices.Add(i);
            }
        }
        
        return indices;
    }

    public static List<int> FindIndices<T>(this List<T> list, T target){
        List<int> indices = new List<int>();

        for(int i = 0; i < list.Count; i++){
            if (list[i].Equals(target)){
                indices.Add(i);
            }
        }
        
        return indices;
    }

    public static void SetAllValues<T>(this List<T> list, T value){
        for(int i = 0; i < list.Count; i++){
            list[i] = value;
        }
    }


    public static void ActivateOneDeactivateOthers(this List<GameObject> objectList, int activeIndex){
        // Activates the object at the specified index, and deactivates all other objects in the list

        foreach(GameObject otherObject in objectList){
            otherObject.SetActive(false);
        }

        objectList[activeIndex].SetActive(true);
    }


    /*public static List<T> GetProperties<T>(this List<object> objectList, string propertyName){
        // Returns a list of the specified property from a given list of objects

        if(objectList.Count > 0){

            List<T> propertiesList = new List<T>();

            for(int i = 0; i < objectList.Count; i++){
                object thisObject = objectList[i];
                T thisParameter = (T) thisObject.GetType().GetProperty(propertyName).GetValue(thisObject);
                propertiesList.Add(thisParameter);
            }

            return propertiesList;
        }
        else{
            return new List<T>();
        }  
    }*/

}
