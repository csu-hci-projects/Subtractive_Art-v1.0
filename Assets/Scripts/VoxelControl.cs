using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelControl : MonoBehaviour {

    public GameObject worldGeometry;
    private ChunkControl control;
	private int chunkSize;
    private float voxelSize;
	public int chunkX;
	public int chunkY;
	public int chunkZ;
    
	private Mesh mesh;
    private MeshRenderer render;
    
	private List<Vector3> addVerticies = new List<Vector3> ();
	private List<int> addTriangles = new List<int> ();
    
    void Start() {
		control = worldGeometry.GetComponent("ChunkControl") as ChunkControl;
        chunkSize = control.chunkSize;
        voxelSize = control.voxelSize;
		mesh = GetComponent<MeshFilter> ().mesh;
        render = GetComponent<MeshRenderer>();
		generateMesh();
    }

    bool voxel(int x, int y, int z){
		return control.voxel (x + chunkX, y + chunkY, z + chunkZ);
	}
    
    void addFace() {
        int startPoint = addVerticies.Count - 4;
		addTriangles.Add (startPoint);
		addTriangles.Add (startPoint + 1);
		addTriangles.Add (startPoint + 2);
		addTriangles.Add (startPoint);
		addTriangles.Add (startPoint + 2);
		addTriangles.Add (startPoint + 3);
    }
    
    void populateLists() {
    	for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					if (voxel (x, y, z) == true) {
                        // Top
                        if (voxel (x, y+1, z) == false) {
                            addVerticies.Add ((new Vector3 (x, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y, z)) * voxelSize);
                            addFace();
						}
                        
                        // Bottom
                        if (voxel (x, y-1, z) == false) {
                            addVerticies.Add ((new Vector3 (x, y - 1, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y - 1, z + 1)) * voxelSize);
                            addFace();
                        }
                        
                        // X+
                        if (voxel (x+1, y, z) == false) {
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z + 1)) * voxelSize);
                            addFace();
                        }
                        
                        // X-
                        if (voxel (x-1, y, z) == false) {
                            addVerticies.Add ((new Vector3 (x, y - 1, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y - 1, z)) * voxelSize);
                            addFace();
                        }
                        
                        // Z+
                        if (voxel (x, y, z+1) == false) {
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y, z + 1)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y - 1, z + 1)) * voxelSize);
                            addFace();
                        }
                        
                        // Z-
                        if (voxel (x, y, z-1) == false)  {
                            addVerticies.Add ((new Vector3 (x, y - 1, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x, y, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y, z)) * voxelSize);
                            addVerticies.Add ((new Vector3 (x + 1, y - 1, z)) * voxelSize);
                            addFace();
                        }
                    }
                }
            }
        }
    }
    
    public void generateMesh() {
		mesh.Clear();
        populateLists();
		mesh.vertices = addVerticies.ToArray ();
		mesh.triangles = addTriangles.ToArray ();
		mesh.RecalculateNormals();
		addVerticies.Clear();
		addTriangles.Clear();
        render.material.color = new Color(0.27F, 0.62F, 0.79F);
    }
    
    public (string, int) toObj(string name, int indexOffset) {
        populateLists();
        if (addVerticies.Count == 0) return ("", 0);
        string value = "o " + name;
        for (int i = 0; i < addVerticies.Count; i++) value +="\nv " + (addVerticies[i].x + transform.position.x).ToString() + " " + (addVerticies[i].y + transform.position.y).ToString() + " " + (addVerticies[i].z + transform.position.z).ToString();
        for (int i = 0; i < addTriangles.Count; i+=3) value +="\nf " + (addTriangles[i] + indexOffset).ToString() + " " + (addTriangles[i + 1] + indexOffset).ToString() + " " + (addTriangles[i + 2] + indexOffset).ToString();
        int numberOfVerticies = addVerticies.Count;
        addVerticies.Clear();
		addTriangles.Clear();
        return (value + "\n", numberOfVerticies);
    }
}
