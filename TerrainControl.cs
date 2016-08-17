using UnityEngine;
using System.Collections;

public class TerrainControl : MonoBehaviour
{
    public TerrainData terrain;

    //简单的地面过度
    private void drawTerrainTexture()
    {
        int height = terrain.alphamapHeight;
        int width = terrain.alphamapWidth;

        float[, ,] alphamap = new float[width , height , 2];

        for ( int y = 0 ; y < height ; y++ )
        {
            for ( int x = 0 ; x < width ; x++ )
            {
                alphamap[y , x , 0] = (float)x / width;
                alphamap[y , x , 1] = 1 - (float)x / width;
            }
        }

        terrain.SetAlphamaps( 0 , 0 , alphamap );
    }

    void OnGUI()
    {
        if ( GUI.Button( new Rect( 0 , 0 , 150 , 50 ) , "DrawTexture" ) )
        {
            drawTerrainTexture();
        }
    }
}
