using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voxelFarm : MonoBehaviour {

    
	public int xVoxels = 9;
	public int yVoxels = 9;
	public int zVoxels = 9;
	
	public int radius = 4;
	

	[Tooltip("True for minecraft style voxels")]
	public bool SnapToGrid = true;

	public Vector3 voxelSize = new Vector3(1f, 1f, 1f);

	void Start () {
		generateGrid ();
	}
	
	void generateGrid(){
	

		GameObject[] voxels = new GameObject[xVoxels * yVoxels * zVoxels];

		Vector3 oPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

		int i = -1;

		for (int x = 0; x < xVoxels; x++){
		
            for(int y = 0;  y < yVoxels; y++){

                for (int z = 0; z < zVoxels; z++){
                    
                    i++;
                    if(
                        ((x == 0 || x == xVoxels-1) && (y == 0 || y == yVoxels-1)) ||
                        ((x == 0 || x == xVoxels-1) && (z == 0 || z == zVoxels-1)) || 
                        ((y == 0 || y == yVoxels-1) && (z == 0 || z == zVoxels-1))
                    ) continue;
                    voxels[i] = GameObject.CreatePrimitive (PrimitiveType.Cube);
                    
                    voxels[i].AddComponent<Selector>();

                    oPos = this.transform.position;
                    oPos.x -= xVoxels/2 * voxelSize.x;
                    oPos.y -= yVoxels/2 * voxelSize.y;
                    oPos.z -= zVoxels/2 * voxelSize.z;

                    oPos.x += x * voxelSize.x;
                    oPos.y += y * voxelSize.y;
                    oPos.z += z * voxelSize.z;

                    voxels[i].transform.position = oPos;
                    voxels[i].transform.localScale = voxelSize;
                    voxels[i].transform.parent = transform;
                    
                }

			}
		}

		combineMeshes();

		for (i = 0; i < voxels.Length; i++){
			Destroy(voxels[i]);
		}
	
	}

	void combineMeshes (){

		MeshFilter[] meshes = GetComponentsInChildren<MeshFilter> ();
		CombineInstance[] combined = new CombineInstance[meshes.Length];

//		if (this.gameObject.GetComponent<MeshCollider>() != null)
//		Destroy (this.gameObject.GetComponent<MeshCollider>());

		for (int i = meshes.Length - 1; i >= 0; i--) {
			combined [i].mesh = meshes [i].sharedMesh;
			combined [i].transform = meshes [i].transform.localToWorldMatrix;
			meshes [i].gameObject.SetActive (false);
		}

		if (this.gameObject.GetComponent<MeshFilter> () == null)
			this.transform.gameObject.AddComponent<MeshFilter> ();

		this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
		this.transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combined, true);
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateBounds ();
		this.transform.GetComponent<MeshFilter> ().mesh.RecalculateNormals ();

		this.transform.gameObject.AddComponent<MeshCollider> ();

		if (this.gameObject.GetComponent<MeshRenderer> () == null)
			this.gameObject.AddComponent<MeshRenderer> ();

		this.transform.gameObject.SetActive (true);
		
	}
	

	void Update () {
        //only need to update when voxel map changes
		if (Input.GetKeyUp (KeyCode.U)) {
			this.transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
			generateGrid ();
		}

	}




}
