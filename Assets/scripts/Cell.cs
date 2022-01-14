using UnityEngine;

public class Cell {

    public readonly int x, y;

    public bool[] wallsLTRB;

    public bool visited = false;

    private bool goal = false;
    private float[] goalRelativePosition = new float[2]{-1, -1};
    private Quaternion goalRelativeRotation;

    public Cell(int x, int y){
        this.x = x;
        this.y = y;

        wallsLTRB = new bool[4] {true, true, true, true};
    }

    public void setGoal(int wallIndex){
        goal = true;
        if(wallIndex < 4 && wallsLTRB[wallIndex]){
            switch(wallIndex){
                case 0:
                    goalRelativePosition = new float[2]{1f - 0.5f, Maze.cellHeight/2 + 1f};
                    goalRelativeRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 1:
                    goalRelativePosition = new float[2]{Maze.cellWidth/2 + 1f, Maze.cellHeight + 0.5f};
                    goalRelativeRotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 2:
                    goalRelativePosition = new float[2]{Maze.cellWidth + 0.5f, Maze.cellHeight/2 + 1f};
                    goalRelativeRotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 3:
                    goalRelativePosition = new float[2]{Maze.cellWidth/2 + 1f, 1f - 0.5f};
                    goalRelativeRotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
        }else{
            throw new System.Exception("Cannot set goal wall to a wall that doesn't exist");
        }
    }

    public float[] getGoalRelativePosition(){return goalRelativePosition;}
    public Quaternion getGoalRelativeRotation(){return goalRelativeRotation;}

    public bool isGoal(){return goal;}

    public void setLeftWall(bool value){
        wallsLTRB[0] = value;
    }

    public void setTopWall(bool value){
        wallsLTRB[1] = value;
    }

    public void setRightWall(bool value){
        wallsLTRB[2] = value;
    }

    public void setBottomWall(bool value){
        wallsLTRB[3] = value;
    }

    public bool getLeftWall(){
        return wallsLTRB[0];
    }

    public bool getTopWall(){
        return wallsLTRB[1];
    }

    public bool getRightWall(){
        return wallsLTRB[2];
    }

    public bool getBottomWall(){
        return wallsLTRB[3];
    }
}
