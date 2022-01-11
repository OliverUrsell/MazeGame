
using System.Collections.Generic;
using System;
using UnityEngine;

public class Maze {

    private Cell[,] cells;
    private Stack<Cell> cellStack;

    public readonly int width, height;
    private readonly int wallsToBreak;

    public Maze(int height, int width){
        this.height = height;
        this.width = width;
        wallsToBreak = 25;
        resetCells();
        generateMaze();
    }

    private void resetCells(){
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x,y] = new Cell(x, y);
            }
        }
    }

    public Cell getCell(int x, int y){
        if (0 <= x && x < width && 0 <= y && y < height) {
            return cells[x, y];
        }
        return null;
    }

    private Cell[] getNeighbouringCellsLTRB(Cell cell){
        return new Cell[4] {
                getCell(cell.x - 1, cell.y),
                getCell(cell.x, cell.y + 1),
                getCell(cell.x + 1, cell.y),
                getCell(cell.x, cell.y - 1)
            };
    }

    private void generateMaze(){
        System.Random random = new System.Random();

        Cell initialCell = getCell(0, 0);
        initialCell.visited = true;
        cellStack = new Stack<Cell>(new Cell[] {initialCell});
        while (cellStack.Count != 0)
        {
            Cell current = cellStack.Pop();
            List<int> unvisited = new List<int>();

            Cell[] neighbouringLTRB = getNeighbouringCellsLTRB(current);

            for (int i = 0; i < 4; i++)
            {
                if(neighbouringLTRB[i] != null && !neighbouringLTRB[i].visited){
                    unvisited.Add(i);
                }
            }

            if(unvisited.Count != 0){

                cellStack.Push(current);

                int chosenIndex = unvisited[random.Next(unvisited.Count)];

                // Remove the wall on the current cell
                current.wallsLTRB[chosenIndex] = false;

                // Remove the wall on the selected cell
                Cell chosen = neighbouringLTRB[chosenIndex];
                // (chosenIndex + 2) % 4 gets the index of the wall from the opposite side
                chosen.wallsLTRB[(chosenIndex + 2) % 4] = false;
                chosen.visited = true;
                cellStack.Push(chosen);
            }
        }

        // Randomly remove some walls to add loops
        for (int walls = 0; walls < this.wallsToBreak; walls++)
        {
            int x = random.Next(this.width);
            int y = random.Next(this.height);
            Cell cell = getCell(x, y);

            List<int> closedWalls = new List<int>();

            Cell[] neighbouringLTRB = getNeighbouringCellsLTRB(cell);

            for (int i = 0; i < 4; i++)
            {
                if(neighbouringLTRB[i] != null && cell.wallsLTRB[i]){
                    closedWalls.Add(i);
                }
            }

            if(closedWalls.Count != 0){

                int chosenIndex = closedWalls[random.Next(closedWalls.Count)];

                // Remove the wall on the current cell
                cell.wallsLTRB[chosenIndex] = false;

                // Remove the wall on the selected cell
                Cell chosen = neighbouringLTRB[chosenIndex];
                // (chosenIndex + 2) % 4 gets the index of the wall from the opposite side
                chosen.wallsLTRB[(chosenIndex + 2) % 4] = false;
            }
            
        }
        
    }

    private void blockSetTexturePixel(Texture2D texture, int startX, int startY, int width, int height, Color color){
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(startX + x, startY + y, color);
            }
        }
    }

    public byte[] toPNGBytes(){
        Texture2D texture = new Texture2D(this.width*10 + 1, this.height*10 + 1, TextureFormat.ARGB32, false);
        blockSetTexturePixel(texture, 0, 0, texture.width, texture.height, Color.white);
    
        for (int x = 0; x < texture.width - 1; x += 10)
        {
            for (int y = 0; y < texture.height - 1; y += 10)
            {
                Cell cell = getCell(x/10, y/10);
                if(cell.getLeftWall()){
                    blockSetTexturePixel(texture, x, y, 1, 11, Color.black);
                }
                if(cell.getTopWall()){
                    blockSetTexturePixel(texture, x, y + 10, 11, 1, Color.black);
                }
                if(cell.getRightWall()){
                    blockSetTexturePixel(texture, x + 10, y, 1, 11, Color.black);
                }
                if(cell.getBottomWall()){
                    blockSetTexturePixel(texture, x, y, 11, 1, Color.black);
                }
            }
        }
    
        // Apply all SetPixel calls
        texture.Apply();

        return texture.EncodeToPNG();
    }

    public void saveAsPng(string fullPath) {
        byte[] bytes = toPNGBytes();
        
        System.IO.File.WriteAllBytes(fullPath, bytes);
        Debug.Log(bytes.Length/1024  + "Kb was saved as: " + fullPath);
    }
}
