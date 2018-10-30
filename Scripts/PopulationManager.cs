using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

	public string[] bestAgent;

	public GameObject botPrefab;
	public GameObject startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 5;
	public float trialDuration;
	int generation = 1;

	private SaveData save;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI(){
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup(new Rect(10, 10, 250, 150));
		GUI.Box(new Rect(0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect(10,25,200,30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect(10,50,200,30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
		GUI.Label(new Rect(10,75,200,30), "Population: " + population.Count, guiStyle);
		GUI.Label(new Rect(10,100,200,30), "Press G to save", guiStyle);
		GUI.EndGroup();
	}

	// Use this for initialization
	void Start () {
		bestAgent = new string[4]{"-1","-1","-1","-1"};

		//SaveData(string[] headerRow, int numberOfColumns, string fileName){
		int numberOfColumns = 10;
		string[] headerRow = new string[]{"ElapsedTime", "Epoch",
										  "BestGene0_CurEpoch", "BestGene1_CurEpoch", 
										  "MaxDistanceTravelled_CurEpoch", "AvgDistanceTravelled_CurEpoch",
										  "BestMaxDistance_Run", "BestBotGene0_Run", "BestBotGene1_Run", "GenerationOfBestBot"};
		save = new SaveData(headerRow, numberOfColumns, "MazeWalker");

		for (int i = 0; i < populationSize; i++){
			GameObject b = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
			Brain botBrain = b.GetComponent<Brain>();
			botBrain.Init();
			population.Add(b);
		}

		trialDuration = 0;
	}

	private string[] EpochInformation(){
		float[] allDists = new float[populationSize];
		//Get furthest travel distance
		for(int i = 0; i < populationSize; i++)
			allDists[i] = population[i].GetComponent<Brain>().distanceTravelled;
		

		float maxDistance = allDists.Max();
		float avgDistance = allDists.Average();
		Debug.Log("Max distance: " + maxDistance);
		Debug.Log("Avg distance: " + avgDistance);



		int indexMaxValue = System.Array.IndexOf(allDists, maxDistance);
		GameObject bestBot = population[indexMaxValue];
		Brain bestBotBrain = bestBot.GetComponent<Brain>();

		int bestBotGene0 = bestBotBrain.dna.GetGene(0);
		int bestBotGene1 = bestBotBrain.dna.GetGene(1);

		Debug.Log("genes of best bot: " + bestBotGene0 +", " + bestBotGene1);

		//Save best agent
		if(bestAgent[0] != null && float.Parse(bestAgent[0]) < maxDistance){
			bestAgent[0] = maxDistance.ToString();
			bestAgent[1] = bestBotGene0.ToString();
			bestAgent[2] = bestBotGene1.ToString();
			bestAgent[3] = generation.ToString();
		}


		return new string[]{trialDuration.ToString(), generation.ToString(),
							bestBotGene0.ToString(), bestBotGene1.ToString(), 
							maxDistance.ToString(), avgDistance.ToString(),
							bestAgent[0].ToString(), bestAgent[1].ToString(),
							bestAgent[2].ToString(), bestAgent[3].ToString()};
	}

	
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		trialDuration += Time.deltaTime;

		if( elapsed >= trialTime ){
			save.addRow(EpochInformation());
			BreedNewPopulation();
			elapsed = 0;
		}
	}

	void LateUpdate(){
		if (Input.GetKey("g")) save.exportCSV();
	}

	GameObject Breed(GameObject parent1, GameObject parent2){
		GameObject offspring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
		Brain b = offspring.GetComponent<Brain>();
		
		//Mutate 1 in 100
		if(Random.Range(0,100) == 1){
			b.Init();
			b.dna.Mutate();
		} else {
			b.Init();
			b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
		}

		return offspring;
	}

	void BreedNewPopulation(){
		List<GameObject> sortedList = population.OrderBy( o => o.GetComponent<Brain>().distanceTravelled).ToList();
		population.Clear();

		for(int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count -1; i++){
			
			population.Add(Breed(sortedList[i], sortedList[i+1]));
			population.Add(Breed(sortedList[i+1], sortedList[i]));
		}

		//Destroy all parents and previous population
		for(int i = 0; i < sortedList.Count; i++){
			Destroy(sortedList[i]);
		}

		generation++;
	}
}
