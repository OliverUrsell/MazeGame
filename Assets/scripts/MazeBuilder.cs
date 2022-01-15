using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshCollider))]
public class MazeBuilder : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private Vector3[] newVertices;
    private int[] newTriangles;

    [SerializeField] private Transform floor;

    [SerializeField] private Material wallsMaterial;
    [SerializeField] private GameObject goalPrefab;

    void Start () {
        Maze m = new Maze(3, 3);
        HTTPClient.client.PutRequest("maze", m.toPNGBytes());
        Texture2D mazeTexture = m.toTexture();
        applyMeshFromTexture(mazeTexture);
        resizeFloor(mazeTexture.width, mazeTexture.height);
        generateGoal(m);
    }

    private void resizeFloor(int width, int height){
        floor.localScale = new Vector3(width, 1, height);
        floor.position = new Vector3(width/2, -2, height/2);
    }

    private void generateGoal(Maze maze){
        Cell goal = maze.getGoal();
        float[] goalRelativePosition = goal.getGoalRelativePosition();
        float x = goal.x * (Maze.cellWidth+1) + goalRelativePosition[0];
        float y = goal.y * (Maze.cellHeight+1) + goalRelativePosition[1];
        Instantiate(goalPrefab, new Vector3(x, 0, y), goal.getGoalRelativeRotation());
    }

    private void applyMeshFromTexture(Texture2D texture){

        List<CombineInstance> combine = new List<CombineInstance>();
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if(texture.GetPixel(x, y) == Color.black){
                    CombineInstance instance = new CombineInstance();
                    instance.mesh = CreateCube(x, y, 3);
                    instance.transform = transform.localToWorldMatrix;

                    combine.Add(instance);
                }
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.CombineMeshes(combine.ToArray());
        mesh.Optimize();

        GetComponent<Renderer>().material = wallsMaterial;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // This function is heavily borrowed from https://gist.github.com/prucha/866b9535d525adc984c4fe883e73a6c7
    private Mesh CreateCube(float x, float z, float height)
    {

        float length = 1;
        float width = 1;

        //3) Define the co-ordinates of each Corner of the cube 
        Vector3[] c = new Vector3[8];

        c[0] = new Vector3(x + -length * .5f, -height * .5f, z + width * .5f);
        c[1] = new Vector3(x + length * .5f, -height * .5f, z + width * .5f);
        c[2] = new Vector3(x + length * .5f, -height * .5f, z + -width * .5f);
        c[3] = new Vector3(x + -length * .5f, -height * .5f, z + -width * .5f);

        c[4] = new Vector3(x + -length * .5f, height * .5f, z + width * .5f);
        c[5] = new Vector3(x + length * .5f, height * .5f, z + width * .5f);
        c[6] = new Vector3(x + length * .5f, height * .5f, z + -width * .5f);
        c[7] = new Vector3(x + -length * .5f, height * .5f, z + -width * .5f);


        //4) Define the vertices that the cube is composed of:
        //I have used 16 vertices (4 vertices per side). 
        //This is because I want the vertices of each side to have separate normals.
        //(so the object renders light/shade correctly) 
        Vector3[] vertices = new Vector3[]
        {
	        c[0], c[1], c[2], c[3], // Bottom
	        c[7], c[4], c[0], c[3], // Left
	        c[4], c[5], c[1], c[0], // Front
	        c[6], c[7], c[3], c[2], // Back
	        c[5], c[6], c[2], c[1], // Right
	        c[7], c[6], c[5], c[4]  // Top
        };


        //5) Define each vertex's Normal
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;


        Vector3[] normals = new Vector3[]
        {
	        down, down, down, down,             // Bottom
	        left, left, left, left,             // Left
	        forward, forward, forward, forward,	// Front
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        up, up, up, up	                    // Top
        };


        //6) Define each vertex's UV co-ordinates
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	        uv11, uv01, uv00, uv10, // Bottom
	        uv11, uv01, uv00, uv10, // Left
	        uv11, uv01, uv00, uv10, // Front
	        uv11, uv01, uv00, uv10, // Back	        
	        uv11, uv01, uv00, uv10, // Right 
	        uv11, uv01, uv00, uv10  // Top
        };


        //7) Define the Polygons (triangles) that make up the our Mesh (cube)
        //IMPORTANT: Unity uses a 'Clockwise Winding Order' for determining front-facing polygons.
        //This means that a polygon's vertices must be defined in 
        //a clockwise order (relative to the camera) in order to be rendered/visible.
        int[] triangles = new int[]
        {
	        3, 1, 0,        3, 2, 1,        // Bottom	
	        7, 5, 4,        7, 6, 5,        // Left
	        11, 9, 8,       11, 10, 9,      // Front
	        15, 13, 12,     15, 14, 13,     // Back
	        19, 17, 16,     19, 18, 17,	    // Right
	        23, 21, 20,     23, 22, 21,	    // Top
        };


        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.Optimize();
        
        return mesh;
    }
}
