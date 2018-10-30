using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour {

	public GameObject blockPrefab;
	public int width = 40;
	public int depth = 40;
	
	void Awake(){
		for (int w = 0; w < width; w++){
			for (int d = 0; d < depth; d++){

				//Border walls: bottom || left 
				if( w==0 || d==0 ){
					Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y, d + this.transform.position.z), Quaternion.identity);
				} 
				//Safe spawn area
				else if( w < 3 && d < 3){
					continue;
				}
				//Border walls: top || right
				else if(w == width-1 || d == depth-1){
					Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y, d + this.transform.position.z), Quaternion.identity);	
				}
				else if(Random.Range(0,5) < 1){
					Instantiate(blockPrefab, new Vector3(w + this.transform.position.x, this.transform.position.y, d + this.transform.position.z), Quaternion.identity);
				}

			}
		}
		
	}
}
