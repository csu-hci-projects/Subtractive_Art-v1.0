using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimarySphereRemove : MonoBehaviour {
    public float radius = 1;
    public GameObject controlObject;
    private ChunkControl control;
    public MeshRenderer meshRenderer;
    public GameObject parent;
   
    public float max = .08f;
    public float min = .01f;
    
    
    void Start() {
        control = controlObject.GetComponent("ChunkControl") as ChunkControl;
    }

    void Update() {
       
        
        Vector3 scale = transform.localScale;
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp)) scale *= 1.2f;
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown)) scale /= 1.2f;
        if(scale[0]>max) scale = new Vector3(max, max, max);
        if(scale[0]<min) scale = new Vector3(min, min, min);
        radius = (0.14f/0.24f)*scale[0];
        transform.localScale = scale;
        
        List<Vector3Int> voxels = new List<Vector3Int>();
        
        Vector3 spherePosition = controlObject.transform.InverseTransformPoint(transform.position) / control.voxelSize;
        Vector3 sphereVoxelPosition = new Vector3(Mathf.Floor(spherePosition.x), Mathf.Floor(spherePosition.y), Mathf.Floor(spherePosition.z));
        Vector3 offset = (spherePosition - sphereVoxelPosition) * control.voxelSize;
        Vector3Int actualVoxelPosition = new Vector3Int(Mathf.FloorToInt(spherePosition.x), Mathf.FloorToInt(spherePosition.y), Mathf.FloorToInt(spherePosition.z));
        int adjustedRadius = Mathf.CeilToInt(radius / control.voxelSize);
        
        for (int x = -adjustedRadius; x <= adjustedRadius; x++) {
            for (int y = -adjustedRadius; y <= adjustedRadius; y++) {
                for (int z = -adjustedRadius; z <= adjustedRadius; z++) {
                    if ((offset + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) * control.voxelSize).magnitude < radius){
                        voxels.Add(actualVoxelPosition + new Vector3Int(x+1, y+1, z+1));
                    }
                }
            }
        }
        control.removeVoxels(voxels.ToArray());
    }
}
