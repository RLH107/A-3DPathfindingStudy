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

        Vector3 initialOrigin = GetCenteredOriginPosition();
        grid = new Grid_(gridSize, cellSize, cellHeightOverrite, initialOrigin, this.gameObject);
        UpdateGrid();
    }




    //////////////////////////////////////////////////////////////////////
    // ----------------- Grid methods --------------------------------- //
    //////////////////////////////////////////////////////////////////////
    public void UpdateGrid() // Update the grid's walkable status for all cells
    {
        CenterGridOnSelf();
    }
    public void CenterGridOnSelf()
    {
        if (grid == null) return;

        Vector3 newOrigin = GetCenteredOriginPosition();
        grid.UpdateGrid(newOrigin);
    }
    public Vector3 GetCenteredOriginPosition()
    {
        // Metade do tamanho total do grid em X e Z usando apenas cellSize
        float halfWidth = ((gridSize.x * cellSize) * 0.5f);
        float halfDepth = ((gridSize.z * cellSize) * 0.5f);


        // Adiciona metade da extensão em X e Z para centralizar; mantém o Y no nível do objeto
        Vector3 offsetToOrigin = new Vector3(halfWidth, -0.1f, halfDepth);

        return transform.position - offsetToOrigin;
    }
}
