using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtensions
{
    
    public static void ScrollTexture(this Material material, float xDistance, float yDistance){
        // Scrolls the given material in the given dimension(s)
        float newOffsetX = material.mainTextureOffset.y + (xDistance * material.mainTextureScale.x);
        float newOffsetY = material.mainTextureOffset.y + (yDistance * material.mainTextureScale.y);
        
        material.mainTextureOffset = new Vector2(newOffsetX % 1, newOffsetY % 1);
    }

    public static void ScrollTexture(this Material material, float xSpeed, float ySpeed, float deltaT){
        float deltaOffsetX = xSpeed * deltaT * material.mainTextureScale.x;
        float deltaOffsetY = ySpeed * deltaT * material.mainTextureScale.y;

        float newOffsetX = material.mainTextureOffset.x + deltaOffsetX;
        float newOffsetY = material.mainTextureOffset.y + deltaOffsetY;
        
        material.mainTextureOffset = new Vector2(newOffsetX % 1, newOffsetY % 1);
    }
}
