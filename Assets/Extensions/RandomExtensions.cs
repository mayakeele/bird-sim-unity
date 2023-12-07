using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExtensions
{
    public static int[] ChooseRandomIndices(int numToChoose, int arrayLength, bool allowRepeats = false){
        numToChoose = Mathf.Clamp(numToChoose, 0, arrayLength-1);

        int numChosen = 0;
        int[] choices = new int[numToChoose];

        while (numChosen < numToChoose){
            int thisNum = UnityEngine.Random.Range(0, arrayLength);

            if (allowRepeats){
                choices[numChosen] = thisNum;
                numChosen++;
            }
            else{
                if(System.Array.IndexOf(choices, thisNum) == -1){
                    choices[numChosen] = thisNum;
                    numChosen++;
                }
            }
        }

        return choices;
    }

    public static T RandomChoice<T>(this T[] input){
        // Chooses and returns a random value from a given array of any type
        int index = Random.Range(0, input.Length);

        return input[index];
    }

    public static T RandomChoice<T>(this List<T> input){
        // Chooses and returns a random value from a given array of any type

        int index = Random.Range(0, input.Count);

        return input[index];
    }

    public static bool RandomBool(){
        int rand = Random.Range(0, 2);
        if (rand == 0){
            return false;
        }
        else{
            return true;
        }
    }

    public static bool RandomChance(float chance){
        float rand = Random.Range(0f, 1f);

        if (rand < chance){
            return true;
        }
        else{
            return false;
        }   
    }

    public static T WeightedRandomChoice<T>(this T[] input, int[] weightArray){

        int totalWeight = weightArray.Sum();
        int rand = Random.Range(0, totalWeight);

        int currentWeight = 0;
        for (int i = 0; i < input.Length; i++){

            currentWeight += weightArray[i];      
            if (rand < currentWeight){
                return input[i];
            }
            else{
                continue;
            }
        }
        return default(T);
    }
    public static T WeightedRandomChoice<T>(this T[] input, float[] weightArray){

        float totalWeight = weightArray.Sum();
        float rand = Random.Range(0, totalWeight);

        float currentWeight = 0;
        for (int i = 0; i < input.Length; i++){

            currentWeight += weightArray[i];      
            if (rand < currentWeight){
                return input[i];
            }
            else{
                continue;
            }
        }
        return default(T);
    }

    // Test code for Weighted Random, put in Start() to run
        /*int[] testItems =   {0, 1, 2, 3, 4, 5};
        int[] testWeights = {10, 5, 1, 1, 5, 10};
        int[] count = new int[testItems.Length];

        for (int i = 0; i < testWeights.Sum(); i++){
            int num = testItems.WeightedRandomChoice(testWeights);
            count[num] += 1;
        }
        foreach (int c in count){
            Debug.Log(c);
        }*/


    public static void Shuffle<T>(this List<T> list){
        int count = list.Count;
        int last = count - 1;

        for (int i = 0; i < last; i++) {
            int r = UnityEngine.Random.Range(i, count);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
