using UnityEngine;
using System.Collections;

public class AreaGeneration : MonoBehaviour {
/*
 Area generation - Generate all the prefab rooms.
 Graeme Murphy cmpt306 Fall 2014
 Using a base set of prefab rooms of a set size (10x10 in this case)
 this code generates multiple areas using a reverse depth-first search
 algorithm AKA a basic maze generation algorithm - with a few tweaks.
 
 The reverse DFS algorithm is run using a MapArrayGenerator.
*/

public bool doAreaGeneration;

	public struct Room {
		public GameObject obj;
		//int area;
		public int layer;
		
		//monsters;
	}

	protected struct Area {
		public Room[] rooms;
		public int width;
		public int height;
		public int x;
		public int y;
	}
	

	//number of tiles in a room
	int width = 10;
	int height = 10;

	//ARRAY OF LITERAL ROOMS AVAILABLE TO GAME
	public Room[] Rooms;
	

	protected Area[] Areas;

	//private MapArrayGenerator.cell [,] areaArray;
	public int areaBaseSize = 10;
	public int areaVariance = 4;

	//start point
	int startxOffset;
	int startyOffset;
	
	public int numAreas = 7;
	
	GameObject[] enemies;
	public float enemyModifer = 0.5f;
	
	Stack s = new Stack();
	GameObject[] items;
	
	int gameDiff;
	
	
	
	//public int areaGap = 5;

	//int numOfAreas = 1;
	
	
	//***
	MapArrayGenerator arrGen;


	// Use this for initialization
	void Start () {
		if(doAreaGeneration) {
			//Apply difficulty settings based on user input from start menu.
			gameDiff =GameObject.Find("Difficulty").GetComponent<Difficulty>().diff;
			
			switch(gameDiff) {
				case 0:
					numAreas=2;
					areaBaseSize = 6;
					areaVariance = 1;
					enemyModifer = 0.175f;
					break;
				case 1:
					areaBaseSize = 7;
					areaVariance = 2;
					numAreas=3;
					enemyModifer = 0.2f;
				
				break;
				case 2:
					areaBaseSize =8;
					areaVariance =3;
					numAreas=5;
					enemyModifer = 0.25f;
				
				break;
				case 3:
					areaBaseSize =9;
					enemyModifer = 0.3f;
					numAreas=7;
					break;
				case 4:
					numAreas=7;
					enemyModifer = 0.4f;
				
				//can't keep keys (in keys file)
					break;
			}
			//reset keys upon death if playing one hardcore mode
			if(GameObject.Find ("Difficulty").GetComponent<Difficulty>().timesDied==0 || gameDiff == 4){
				GameObject.Find ("Difficulty").GetComponent<Difficulty>().keys = new bool[numAreas];}
			else {
				GameObject.Find("Character").GetComponent<move>().flame.SetActive(true);
			}
			generate();
			
		}	
		
	}
	
	//returns true when complete.
	public void generate() {
				
		if(doAreaGeneration) {
			Shader shaderGUItext = new Shader();
			Shader shaderSpritesDefault = new Shader();
			
			
			//loading screen
		
			//create and run MapArray generator
			//Creates an array of integers coresponding to the directions a room can exit
			arrGen = new MapArrayGenerator ();
			Areas = new Area[numAreas];
			
			Vector2 areaArraySize;
	
	
			//load all Room game objects.
			GameObject[] roomObjects = Resources.LoadAll<GameObject> ("Rooms");
			enemies = Resources.LoadAll<GameObject> ("enemies");
			items = Resources.LoadAll<GameObject> ("Items");
			//Instantiate array of prefab rooms 
			Rooms = new Room[roomObjects.Length];
			
			//Apply the correct room - identified by layer == integer from MapArrayGenerator.
			for(int r=0; r<roomObjects.Length; r++) {
				if (((GameObject)roomObjects[r]).layer != 0) {
					int rl = ((GameObject)roomObjects[r]).layer-15;
					Rooms[rl].obj = (GameObject)roomObjects[r];
					Rooms[rl].layer = rl;
				}
			}
				
			int yShift = height*2;
			int xShift = 0;
			int getLayer;
			
			//Run area generation multiple times
			for(int a=0; a<numAreas; a++) {
				//init area
				Areas[a] = new Area();
			
				
				//roll area dimensions
				if( a<3 ||a>4) {
					int temp =Random.Range(areaVariance*-1, areaVariance+1);
					areaArraySize.x = temp + areaBaseSize;
					areaArraySize.y = temp*-1 + areaBaseSize;
				} else {
					int temp =Random.Range(areaVariance*-1, areaVariance+1);
					areaArraySize.x = temp + areaBaseSize;
					areaArraySize.y = Areas[0].height;
					
				}
				//areaArray = new MapArrayGenerator.cell[(int)areaArraySize.x, (int)areaArraySize.y];
				//areaArray = arrGen.makeArray(areaArraySize, areaArraySize);
			
				Areas[a].rooms = new Room[(int) (areaArraySize.x * areaArraySize.y)];
	
				Areas[a].height = (int) areaArraySize.y;
				Areas[a].width = (int) areaArraySize.x;
				
				//Apply area location.
				Vector2 shifts = AreaLoc(areaArraySize, a, xShift, yShift);
				xShift = (int) shifts.x;
				yShift = (int) shifts.y;
				
				//debug info of area
				string outDeb = "area#" + a + ", x:"+Areas[a].x+", y:" + Areas[a].y + "\n width:" + Areas[a].width + ", height:" + Areas[a].height + "\n";
				outDeb += 	"startxoffset: " + startxOffset + ", xshift:" + xShift + ", areas[a].width: " + Areas[a].width + ", * width: " + width + " = " + Areas[a].x;	
				Debug.Log(outDeb);
				
				s.Clear();
				//instantiate rooms
				
				Vector3[,] posArray = new Vector3[(int)(areaArraySize.y),(int) (areaArraySize.x)];
				int[,] difficulty = new int[(int)(areaArraySize.y),(int) (areaArraySize.x)];
				for(int y=0; y< areaArraySize.y; y++) {
					for(int x=0; x< areaArraySize.x; x++) {
						
						getLayer = arrGen.toIntSingle(x,y);
						Areas[a].rooms[x*y].obj = Rooms[getLayer-15].obj;
						Vector3 pos = new Vector3(x*width+Areas[a].x, (y*height*-1)+Areas[a].y, 0f);
						GameObject tempObj = (GameObject)Instantiate(Areas[a].rooms[x*y].obj, pos, Quaternion.identity);
						s.Push(tempObj);
						posArray[y,x] = pos;
						difficulty[y,x] = arrGen.getDifficulty(x,y);
						
						shiftColour(tempObj,a);
						//doDifficultyStuff(arrGen.getDifficulty(x,y) , pos, a);
						//tempObj.GetComponent<SpriteRenderer>().color.a = 1.0f;
						
					}
				}
				
				
				bool hasKey = false;
				if(a == 0)
					hasKey = true;
					
					/*
				if(a>0) {
					//spawn keys after first area
					int hardest=0;
					Stack bestSpots = new Stack();
					for(int ky=0; ky<areaArraySize.y; ky++) {
						for(int kx=0; kx<areaArraySize.x; kx++) {
							if(difficulty[ky,kx]> hardest) {
								bestSpots.Clear();
								hardest = difficulty[ky,kx];
							} 
							if (difficulty[ky,kx] == hardest) {
								bestSpots.Push(new Vector2(ky, kx));
								               
							}
						}
					}
					
					Vector2 spot = new Vector2(0,0);
					int spawnKeyAt = Random.Range(0,bestSpots.Count);
					
						for(int bleh=0; bleh<spawnKeyAt-1; bleh++) {
							spot = (Vector2)bestSpots.Pop();
						}
						
					Debug.Log ("Spot: " +spot);
					GameObject newKey;
					Vector3 newPosition = new Vector3(Areas[a].x+(spot.x*width)+4.5f, Areas[a].y-(spot.y*height) -4.5f, 0f);
					newKey = (GameObject)Instantiate(items[2], newPosition, Quaternion.identity);
					newKey.GetComponent<Keys>().keyNumber = a+1;
				}
				*/
				//Spawn enemies
				int numEnemies=0;
				Debug.Log ("Enemy spawn count for area: "+a+", is: " +(a*enemyModifer+5) );
				while(numEnemies< (a*10*enemyModifer +5+a*2) ) {
					Quaternion id = Quaternion.identity;
					int randX = Random.Range(0,(int)areaArraySize.x-1);
					int randY = Random.Range(0,(int)areaArraySize.y-1);
					
					Vector3 posTemp = posArray[randY, randX];
					//Debug.Log ("caught: " + posTemp);
					int rand2 = difficulty[randY, randX];
					if(rand2 != 0 ) {
						int getEnemy = Random.Range(0, enemies.Length);
						
						if(Random.Range(0,10/rand2) == 0) {
							
							int i2 = Random.Range(1,3);
							switch(i2) {
							case 1:
								posTemp = new Vector3(posTemp.x+5, posTemp.y-3, 0);
								break;
							case 2:
								posTemp = new Vector3(posTemp.x+2, posTemp.y-7, 0);
								break;
							case 3:
								posTemp = new Vector3(posTemp.x+7, posTemp.y-7, 0);
								break;
							}
							//Debug.Log ("spawning: at x:" + posTemp.x + ", y:" + posTemp.y );
							
							//halfway through making enemies apply a key if needed
							if(numEnemies > (a*10*enemyModifer +5+a*2)/2 && !hasKey ) {
								GameObject newKey;
								newKey = (GameObject)Instantiate(items[2], posTemp, Quaternion.identity);
								newKey.GetComponent<Keys>().keyNumber = a+1;
								
								hasKey = true;
							}
								
							Instantiate(enemies[getEnemy], posTemp, id);
							//Debug.Log ("instantiated enemy at : "+ posTemp);
							
							numEnemies += 1;
						}
					}
					
					
				}
				
				
			} //Area generation for-loop
		} //if doAreageneration
		
		GameObject.Find ("Difficulty").GetComponent<Difficulty>().doGui =true;
	} //generate() 
	
	
	Vector2 AreaLoc(Vector2 areaArraySize, int a, int xShift, int yShift) {
		
		if(a ==0) { //First area 
			arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x/2, areaArraySize.y/2), gameDiff);
			
			//first area is always in roughly the same starting coordinate
			startxOffset =Mathf.FloorToInt(((int)areaArraySize.x*width/2 + width/2)*-1);
			startyOffset = Mathf.FloorToInt((int)areaArraySize.y*height/2 + height/2);
			if(startxOffset %2==0)
				startxOffset = startxOffset -5;
			if(startyOffset %2 ==0)
				startyOffset = startyOffset -5;
			Areas[a].x = startxOffset;
			Areas[a].y = startyOffset;
			
			
			

			
			//open to both areas (if not in demo mode);
			if(gameDiff >0)
				arrGen.modSingle(0, Areas[0].height-2, 'L');

			arrGen.modSingle(Areas[0].width-1, Areas[0].height-2, 'R');
			
			//place key in top row always for area1
			int rando = Random.Range(0, Areas[0].width-1);
			GameObject key1;
			Vector3 pos1 = new Vector3(Areas[0].x + width*rando + 4.5f, Areas[0].y -4.5f, 0f);
			key1 = (GameObject) Instantiate(items[2], pos1, Quaternion.identity);
			key1.GetComponent<Keys>().keyNumber = 1;
			
			
		} 
			
			//for areas 2-7 - choose a random side, go to it and make it fit to 
			//the appropriate area
		 else if(a%2 == 1) { //even numbered areas (area# = a+1)
			
			//after area's 1-3, move areas up and out from area 1
			if(a>2) {
				yShift = yShift + startyOffset;
				xShift = xShift + 3*width;
			}
			//roll a random to choose between left and right of starting area
			if(Random.Range(0,10000)%2 ==0 || gameDiff ==0) { //go right
				//generate map array
				
				//for area's 2-3, generate beside area 1's bottom corners
				if(a<=2) {
					arrGen.makeArray(areaArraySize, new Vector2(0, 0), gameDiff); 
					
					Areas[a].x = startxOffset +xShift + Areas[0].width*width;
					Areas[a].y = startyOffset +yShift - (Areas[0].height * height);
					//open path back
					arrGen.modSingle(0, 0, 'L');
					if(gameDiff>1) {
						arrGen.modSingle(Areas[a].width-1, 0, 'U');
					}
					//place door
					Object tempDoor;
					tempDoor = Instantiate(items[0], new Vector3(Areas[a].x, Areas[a].y-4.5f, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
				} else {
					arrGen.makeArray(areaArraySize, new Vector2(0, areaArraySize.y-1), gameDiff); 
					
					if(Areas[a-1].x >startxOffset) { //find area below current
						Areas[a].x = Areas[a-1].x + (Areas[a-1].width * width) - width;
						Areas[a].y = Areas[a-1].y + (Areas[a].height * height);
						
					} else { //a-2
						Areas[a].x = Areas[a-2].x + (Areas[a-2].width * width) - width; //+ Areas[a].width*width;
						Areas[a].y = Areas[a-2].y +(Areas[a].height * height);
					}
					//Only open top if not on last couple areas
					if(numAreas-a >2) {	
						arrGen.modSingle(Areas[a].width-1,0,'U');
					}
					arrGen.modSingle(0, Areas[a].height-1, 'D');
					Object tempDoor;
					tempDoor = Instantiate(items[1], new Vector3(Areas[a].x+4.5f, Areas[a].y-9f - (Areas[a].height-1)*height, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);				
				}
				
			} else { //go left
				
				//for area's 2-3, generate beside area 1's bottom corners
				
				if(a<=2) {
					arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x-1, 0), gameDiff);
					
					Areas[a].x = startxOffset -xShift - (Areas[a].width * width);
					Areas[a].y = startyOffset +yShift - (Areas[0].height * height);
					//open path back
					arrGen.modSingle(Areas[a].width-1, 0, 'R');
					//apply difficulty
					if(gameDiff>1)
						arrGen.modSingle(0, 0, 'U');
					Object tempDoor;
					tempDoor =  Instantiate(items[0], new Vector3(Areas[a].x+(Areas[a].width*width) -1, Areas[a].y-4.5f, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);

				} else {
					arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x-1, areaArraySize.y-1), gameDiff);
					
					if(Areas[a-1].x<startxOffset) {
						Areas[a].x = Areas[a-1].x - (Areas[a].width * width) + width;
						Areas[a].y = Areas[a-1].y + (Areas[a].height * height);
					}else { // 2 back 
						Areas[a].x = Areas[a-2].x - (Areas[a].width * width) + width;
						Areas[a].y = Areas[a-2].y + (Areas[a].height * height);
					}
					//Open paths
					if(numAreas-a >2) {
						arrGen.modSingle(0,0,'U');
					}
					arrGen.modSingle(Areas[a].width-1, Areas[a].height-1, 'D');
					Object tempDoor;
					tempDoor =  Instantiate(items[1], new Vector3(Areas[a].x+(Areas[a].width*width) -5.5f, Areas[a].y-9f- (Areas[a].height-1)*height, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
				}
				
			}
		} else {	//*********flip to the other side of the dungeon*********
			
			if(Areas[a-1].x<startxOffset) {
				//R
				
				if(a<=2) {
					arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x-1, 0), gameDiff);
					
					Areas[a].x = startxOffset +xShift + Areas[0].width*width;
					Areas[a].y = startyOffset +yShift - (Areas[0].height * height);
					arrGen.modSingle(0, 0, 'L');
					if(gameDiff>1)
						arrGen.modSingle(Areas[a].width-1, 0, 'U');
					
					Object tempDoor;
					tempDoor =  Instantiate(items[0], new Vector3(Areas[a].x, Areas[a].y-4.5f, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
					
				} else {
					arrGen.makeArray(areaArraySize, new Vector2(0, areaArraySize.y-1), gameDiff); 
					
					if(Areas[a-2].x > startxOffset) {
						Areas[a].x = Areas[a-2].x + (Areas[a-2].width * width) - width;
						Areas[a].y = Areas[a-2].y + (Areas[a].height * height);
						
					} else {//a-3
						Areas[a].x = Areas[a-3].x + (Areas[a-3].width * width) - width;
						Areas[a].y = Areas[a-3].y + (Areas[a].height * height);
					}
					if(numAreas-a >2) {
						arrGen.modSingle(Areas[a].width-1,0,'U');
					}
					arrGen.modSingle(0, Areas[a].height-1, 'D');		
					Object tempDoor;
					tempDoor =  Instantiate(items[1], new Vector3(Areas[a].x+4.5f, Areas[a].y-9f- (Areas[a].height-1)*height, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
				}
				
				
				
				
			} else {
				//L
				
				if(a<=2) {	
					arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x-1, 0), gameDiff);
					
					Areas[a].x = startxOffset -xShift - (Areas[a].width * width);
					Areas[a].y = startyOffset +yShift - (Areas[0].height * height);
					arrGen.modSingle(Areas[a].width-1, 0, 'R');
					if(gameDiff >1)
						arrGen.modSingle(0, 0, 'U');
					
					//place door
					Object tempDoor = null;
					Vector3 pos = new Vector3(Areas[a].x+(Areas[a].width*width) -1,Areas[a].y-4.5f, 0f);
					tempDoor =  Instantiate(items[0], pos, Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
					
					
				} else {
					arrGen.makeArray(areaArraySize, new Vector2(areaArraySize.x-1, 0), gameDiff);
					
					if(Areas[a-2].x < startxOffset) {
						Areas[a].x = Areas[a-2].x - (Areas[a].width * width) + width;
						Areas[a].y = Areas[a-2].y + (Areas[a].height * height);
					} else { //a-3
						Areas[a].x = Areas[a-3].x - (Areas[a].width * width) + width;
						Areas[a].y = Areas[a-3].y + (Areas[a].height * height);
					}
					if(numAreas-a >2) {
						arrGen.modSingle(0,0,'U');
					}
					arrGen.modSingle(Areas[a].width-1, Areas[a].height-1, 'D');	
					Object tempDoor;
					tempDoor = Instantiate(items[1], new Vector3(Areas[a].x+(Areas[a].width*width) -5.5f,  Areas[a].y-9f- (Areas[a].height-1)*height, 0f), Quaternion.identity);
					((GameObject) tempDoor).GetComponent<LockedDoor>().doColour(a);
				}
			}
		}
		
		return new Vector2(xShift, yShift);
	}
	
	void shiftColour(GameObject room, int a) {
		SpriteRenderer[] rArr = room.GetComponentsInChildren<SpriteRenderer>();
		
		foreach(SpriteRenderer r in rArr) {
		switch(a) {
		case -1:
			break;
		case 1:
			r.color = new Color(1f, 0f, 0f, 0.8f);
			break;
		case 2:
			r.color = new Color(0f, 1f, 0f, 0.8f);
			break;
		case 3:
			r.color = new Color(0f, 0f, 1f, 0.8f);
			break;
		case 4:
			r.color = new Color(1f, 0f, 1f, 0.8f);
			break;
		case 5:
			r.color = new Color(0f, 1f, 1f, 0.8f);
			break;
		case 6:
			r.color = new Color(1f, 1f, 0f, 0.8f);
			break;
		case 7:
			r.color = new Color(1f, 1f, 1f, 0.2f);
			break;
		case 8:
			r.color = Color.black;
			break;	
			}
		}
	}
	
	/*
	void doDifficultyStuff(int difficulty, Vector3 pos, int a) {

		
		
		for(int x=0; x<Areas[a].rooms.Length; x++) {
			 arrGen.getDifficulty(Areas[a].rooms[x].
		}
		
		if(difficulty>0) {
			//aroom.GetComponentInChildren<transform>();
			for(int i=0; i<difficulty; i++) {
				switch(i) {
					case 1:
						pos = new Vector3(pos.x+5, pos.y+3, 0);
						break;
					case 2:
						pos = new Vector3(pos.x+2, pos.y+7, 0);
						break;
					case 3:
						pos = new Vector3(pos.x+7, pos.y+7, 0);
						break;
					
					}
				Quaternion id = Quaternion.identity;
				int getEnemy = Random.Range(0, enemies.Length-1);
				Instantiate(enemies[getEnemy], pos, id);
			
			}
		}
		
	}*/
	
	
	/*	
	public void doThing () {
	/*
		if (Input.GetKeyDown(KeyCode.P)) {
			arrGen = new MapArrayGenerator();
			areaArray = new int[(int)areaArraySize.x, (int)areaArraySize.y];
			areaArray = arrGen.makeArray(areaArraySize);
			
			printArray(areaArray);
		}*/
		/*if (Input.GetKeyDown(KeyCode.L)) {
			Start ();
			}
			
	}*/

/*
	void printArray(int[,] arr) {
		string l = "";
		for (int y=0; y< areaArraySize.y; y++) {
			for(int x=0; x< areaArraySize.x; x++) {
				l += arr[y,x] + " ";
			}
			Debug.Log (l);
			l= "";
		}
	}*/
}
