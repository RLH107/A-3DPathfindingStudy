using System.Collections.Generic;
using UnityEngine;

/// Add this script to a Player GameObject to generate a grid of cells based on the specified grid size. Around the player position. 
/// The grid size can be set in the inspector, and the grid will be generated when the game starts. 

//[ExecuteInEditMode]
public class Grid_Generator : MonoBehaviour
{
    //grid information
    public Vector3 gridSize;  // The size of the grid in terms of number of cells in each dimension (x, y, z)
    public Vector3 gridCenterOffset; // The offset to apply to the grid's center position relative to the GameObject's position
    public float cellSize = 1f; // The size of each cell in the grid
    public float cellHeightOverrite = 0f;
    public Grid_ grid { get; private set; }
    
    ////////////////////////////////////
    /**/ public bool debug = true; /**/         // Debugging flag to enable or disable debug logs & other debug features
    ////////////////////////////////////

    
    ///////////////////////////////////////////////////////////////////////////
    /// ------------------ Unity Lifecycle Methods -----------------
    ///////////////////////////////////////////////////////////////////////////
    void Awake()// Used to initialize the grid and subscribe to events from the GameHandler
    {
        if (gridSize == Vector3.zero)
        {
            if (debug) Debug.LogError("Grid Size NotSetInpo: " + gridSize);
        }
    }

    public void GenerateGrid() // Method to generate the grid, can be called externally if needed
    {
        Vector3 initialOrigin = transform.position;
        grid = new Grid_(gridSize, cellSize, cellHeightOverrite, initialOrigin, this.gameObject);
        UpdateGrid();
    }
    public void FindPath()
    {
        if (grid == null) return;

        grid.pathfinding.FindPath(0,0,0,3,4,4);
    }

    //////////////////////////////////////////////////////////////////////
    // ----------------- Grid methods --------------------------------- //
    //////////////////////////////////////////////////////////////////////
    public void UpdateGrid() // Update the grid's walkable status for all cells
    {
        grid.UpdateGrid();
    }
    /*
    public Vector3 GetCenteredOriginPosition()
    {
        // Metade do tamanho total do grid em X e Z usando apenas cellSize
        float halfWidth = ((gridSize.x * cellSize) * 0.5f);
        float halfDepth = ((gridSize.z * cellSize) * 0.5f);


        // Adiciona metade da extensão em X e Z para centralizar; mantém o Y no nível do objeto
        Vector3 offsetToOrigin = new Vector3(halfWidth, -0.1f, halfDepth);

        return transform.position - offsetToOrigin;
    }
    */

    /////////////////////////////////////////////////////////////
    // ----------------- GridDebugFuiinction ----------------- //               <== UPDATE NEEDET : 
    /////////////////////////////////////////////////////////////
    int x = 0; // Variables to keep track of the current cell coordinates for cycling through spawn positions
    int y = 0;
    int z = 0;
    int changeCase = 0;
    public Vector3 cicleThroghSpawnPos() // This method is for testing purposes (possebly to the game), to cycle through spawn positions and log them // || GridDebugFuiinction || \\
    {
        if (grid != null && grid.CellWorldPosition(x, y, z) != null)
        {
            grid.returnWidthLenghHeight(out int width, out int hight, out int lengh);
            Vector3 worldPos = grid.CellWorldPosition(x, 0, z);
            Debug.Log($"Cell ({x}, {z}) World Position: {worldPos}");
            switch (changeCase)
            {
                case 0:
                    x = x + 3;
                    if (x >= width)
                    {
                        changeCase = 1;
                        x = 0; // Reset x to 0 when switching to the next case
                    }
                    break;
                case 1:
                    z = z + 3;
                    changeCase = 0;
                    if (z >= lengh)
                    {
                        z = 0; // Reset z to 0 when it exceeds the length
                    }

                    break;
            }
            return worldPos; // Return the first position for testing; you can modify this to return other positions as needed
        }
        Debug.LogError($"Grid is null or empty. Cannot cycle through spawn positions.");
        return Vector3.zero; // Return a default value if the grid is null or empty
    }
}
