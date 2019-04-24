using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkControl : MonoBehaviour {	

    public int xChunks = 8;
    public int yChunks = 8;
    public int zChunks = 8;
    
    public int chunkSize = 16;
    
    public float voxelSize = 0.1f;

    public int xVoxels;
	public int yVoxels;
	public int zVoxels;
    
    public GameObject chunk;
    
    public bool[,,] data;
    public VoxelControl[,,] chunks;
    
    
    void Start() {
        xVoxels = xChunks * chunkSize;
        yVoxels = yChunks * chunkSize;
        zVoxels = zChunks * chunkSize;
        
        data = new bool[xVoxels, yVoxels, zVoxels];
		for (int x = 0; x < xVoxels; x++) {for (int y = 0; y < yVoxels; y++) {for (int z = 0; z < zVoxels; z++) {if (Random.Range(0.0f, 1.0f) > 0) {data [x, y, z] = true;} else {data [x, y, z] = false;}}}};

		chunks = new VoxelControl[xChunks, yChunks, zChunks];

		for (int x = 0; x < xChunks; x++) {
			for (int y = 0; y < yChunks; y++) {
				for (int z = 0; z < zChunks; z++) {
					GameObject newChunk = Instantiate (chunk, new Vector3 (x * chunkSize * voxelSize, y * chunkSize * voxelSize, z * chunkSize * voxelSize) + transform.position, new Quaternion (0, 0, 0, 0)) as GameObject;
                    newChunk.name = "Chunk " + x.ToString() + y.ToString() + z.ToString();
					chunks [x, y, z] = newChunk.GetComponent("VoxelControl") as VoxelControl;
					chunks [x, y, z].worldGeometry = gameObject;
					chunks [x, y, z].chunkX = x * chunkSize;
					chunks [x, y, z].chunkY = y * chunkSize;
					chunks [x, y, z].chunkZ = z * chunkSize;
				}
			}
		}
        
    }
    
    bool voxelExists(int x, int y, int z) {
        return (x >= 0 && x < xVoxels && y >= 0 && y < yVoxels && z >= 0 && z < zVoxels);
    }
    
	public bool voxel(int x, int y, int z){
		if (voxelExists(x, y, z)) return data[x, y, z];
		return false;
	}
    
    void checkAdd(Dictionary<int, VoxelControl> chunkUpdates, Vector3Int voxel) {
        int chunkIndex = Mathf.FloorToInt(voxel.x / chunkSize) + Mathf.FloorToInt(voxel.y / chunkSize) * xChunks  + Mathf.FloorToInt(voxel.z / chunkSize) * xChunks * yChunks;
        if (!chunkUpdates.ContainsKey(chunkIndex)) chunkUpdates.Add(chunkIndex, chunks[Mathf.FloorToInt(voxel.x / chunkSize), Mathf.FloorToInt(voxel.y / chunkSize), Mathf.FloorToInt(voxel.z / chunkSize)]);
    }
    
    public void removeVoxels(Vector3Int[] voxels) {
        bool redrawRequired = false;
        
        Dictionary<int, VoxelControl> chunkUpdates = new Dictionary<int, VoxelControl>();
        
        for (int i = 0; i < voxels.Length; i++) {
            Vector3Int voxel = voxels[i];
            if (!voxelExists(voxel.x, voxel.y, voxel.z)) continue;
            if (data[voxel.x, voxel.y, voxel.z]) {
                data[voxel.x, voxel.y, voxel.z] = false;
                redrawRequired = true;
                checkAdd(chunkUpdates, voxel);
                // Check edge cases for rendering
                // X
                if (voxel.x % chunkSize == 0) if (voxelExists(voxel.x - 1, voxel.y, voxel.z)) checkAdd(chunkUpdates, new Vector3Int(voxel.x - 1, voxel.y, voxel.z));
                if (voxel.x % chunkSize == chunkSize - 1) if (voxelExists(voxel.x + 1, voxel.y, voxel.z)) checkAdd(chunkUpdates, new Vector3Int(voxel.x + 1, voxel.y, voxel.z));
                // Y
                if (voxel.y % chunkSize == 0) if (voxelExists(voxel.x, voxel.y - 1, voxel.z)) checkAdd(chunkUpdates, new Vector3Int(voxel.x, voxel.y - 1, voxel.z));
                if (voxel.y % chunkSize == chunkSize - 1) if (voxelExists(voxel.x, voxel.y + 1, voxel.z)) checkAdd(chunkUpdates, new Vector3Int(voxel.x, voxel.y + 1, voxel.z));
                // Z
                if (voxel.z % chunkSize == 0) if (voxelExists(voxel.x, voxel.y, voxel.z - 1)) checkAdd(chunkUpdates, new Vector3Int(voxel.x, voxel.y, voxel.z - 1));
                if (voxel.z % chunkSize == chunkSize - 1) if (voxelExists(voxel.x, voxel.y, voxel.z + 1)) checkAdd(chunkUpdates, new Vector3Int(voxel.x, voxel.y, voxel.z + 1));
            }
        }
        
        if (redrawRequired) foreach (KeyValuePair<int, VoxelControl> chunk in chunkUpdates) chunk.Value.generateMesh();
    }
    
    void toFile() {
        string output = "";
        int offset = 1;
		for (int x = 0; x < xChunks; x++) {
			for (int y = 0; y < yChunks; y++) {
				for (int z = 0; z < zChunks; z++) {
                    int chunkIndex = x + y * xChunks  + z * xChunks * yChunks;
                    (string newOutput, int newOffset) = chunks[x, y, z].toObj("Chunk" + chunkIndex.ToString(), offset);
                    output += newOutput;
                    offset += newOffset;
                }
            }
        }
        System.IO.File.WriteAllText("C:\\Users\\alex\\Desktop\\Output.obj", output);
        Debug.Log("Done!");
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) toFile();
    }
}
