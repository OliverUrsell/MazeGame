
public class Cell {

    public readonly int x, y;

    public bool[] wallsLTRB;

    public bool visited;

    public Cell(int x, int y){
        this.x = x;
        this.y = y;

        wallsLTRB = new bool[4] {true, true, true, true};
        visited = false;
    }

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
