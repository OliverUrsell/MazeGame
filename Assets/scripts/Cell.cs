
public class Cell {

    public readonly int x, y;

    public bool[] wallsLTRB;

    public bool visited = false;

    private bool goal = false;
    private int goalWall = -1;

    public Cell(int x, int y){
        this.x = x;
        this.y = y;

        wallsLTRB = new bool[4] {true, true, true, true};
    }

    public void setGoal(int wallIndex){
        goal = true;
        if(wallsLTRB[wallIndex]){
            goalWall = wallIndex;
        }else{
            throw new System.Exception("Cannot set goal wall to a wall that doesn't exist");
        }
    }

    public int getGoalWall(){return goalWall;}

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
