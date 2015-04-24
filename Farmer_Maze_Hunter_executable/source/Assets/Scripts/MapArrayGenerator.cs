using UnityEngine;
using System.Collections;

public class MapArrayGenerator : MonoBehaviour {
	/*
 Area generation - Generate all the prefab rooms.
 Graeme Murphy cmpt306 Fall 2014
 Using a base set of prefab rooms of a set size (10x10 in this case)
 this code generates multiple areas using a reverse depth-first search
 algorithm AKA a basic maze generation algorithm - with a few tweaks.
 
 The reverse DFS algorithm is ran here upon sending the command to 'makeArray'
 using input dimensions and a vector2 starting location as paramaters.
*/

	Vector2 areaArraySize;
	cell[,] areaArray;
	Stack s;

	//for use with depth first search algorithm. Can also be modified
	//after to your content.
	public struct cell {
		public float i;
		public bool visited;
		
		public int x;
		public int y;
		
		public int difficulty;

		
		public bool U;
		public bool D;
		public bool L;
		public bool R;
		
		
	}


	/*
	makeArray (dimensions, start, difficulty)
	Dimensions are the size the array is built to.
	Start point is for generating the difficulty submap.
	(difficulty is actually not used in this function.
	*/
	public void makeArray(Vector2 dimensions, Vector2 start, int difficulty) {
		s = new Stack();
		areaArraySize = dimensions;
		//MakeBlankArray ();
		FillArray (start);
		
		//ensure corners
		//AssurePath(0,0, (int)areaArraySize.x-1,(int) areaArraySize.y-1);
		//AssurePath(0,(int)areaArraySize.y-1, (int)areaArraySize.x-1, 0);
		
		assureAllPaths();
	}
	
	private void assureAllPaths() {
		for(int y=0; y<areaArraySize.y; y++) {
			for(int x=0; x<areaArraySize.x; x++) {
				AssurePath(0,0, x,y);
			}
		}
	}
	
	//send the generated array to an integer array and return
	int[,] toInt() {
		int[,] ret = new int[(int)areaArraySize.y, (int)areaArraySize.x];

		for(int y=0; y< areaArraySize.y; y++) {
			for(int x=0; x< areaArraySize.x; x++){
				if(areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
					//up
					ret[y,x] = 15;
				} else if(!areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
					//down
					ret[y,x] = 16;
				} else if(!areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
					//left
					ret[y,x] = 17;
				} else if(!areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
					//right
					ret[y,x] = 18;
				} else if(areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
					//updown
					ret[y,x] = 19;
				} else if(areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
					//upleft
					ret[y,x] = 20;
				} else if(areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
					//upright
					ret[y,x] = 21;
				} else if(!areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
					//downleft
					ret[y,x] = 22;
				} else if(!areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
					//downright
					ret[y,x] = 23;
				} else if(!areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
					//leftright
					ret[y,x] = 24;
				} else if(areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
					//udl
					ret[y,x] = 25;
				} else if(areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
					//udr
					ret[y,x] = 26;
				} else if(areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
					//ulr
					ret[y,x] = 27;
				} else if(!areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
					//dlr
					ret[y,x] = 28;
				} else if(areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
					//fourway
					ret[y,x] = 29;
				} else {
					//zero ways
					//Debug.Log("bad");
				}
				
			}
		}

		return ret;
	}
	
	//NO BUGCHECKING HERE
	//modify a single direction of a room
	public void modSingle(int x, int y, char d) {
		if(d == 'U')
			areaArray[y,x].U = true;
		if(d == 'D')
			areaArray[y,x].D = true;
		if(d == 'L')
			areaArray[y,x].L = true;
		if(d == 'R')	
			areaArray[y,x].R = true;
	
	}
	public void closeSingle(int x, int y, char d) {
		if(d == 'U')
			areaArray[y,x].U = false;
		if(d == 'D')
			areaArray[y,x].D = false;
		if(d == 'L')
			areaArray[y,x].L = false;
		if(d == 'R')	
			areaArray[y,x].R = false;
	}
	
	public int getDifficulty(int x, int y) {
		return areaArray[y,x].difficulty;
	}
	

	
	//return the int value of a specific room from cell array
	public int toIntSingle(int x, int y) {
		int ret =-1;
	
		if(areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
			//up
			ret = 15;
		} else if(!areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
			//down
			ret = 16;
		} else if(!areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
			//left
			ret = 17;
		} else if(!areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
			//right
			ret = 18;
		} else if(areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && !areaArray[y, x].R) {
			//updown
			ret = 19;
		} else if(areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
			//upleft
			ret = 20;
		} else if(areaArray[y, x].U & !areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
			//upright
			ret = 21;
		} else if(!areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
			//downleft
			ret = 22;
		} else if(!areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
			//downright
			ret = 23;
		} else if(!areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
			//leftright
			ret = 24;
		} else if(areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && !areaArray[y, x].R) {
			//udl
			ret = 25;
		} else if(areaArray[y, x].U & areaArray[y, x].D && !areaArray[y, x].L && areaArray[y, x].R) {
			//udr
			ret = 26;
		} else if(areaArray[y, x].U & !areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
			//ulr
			ret = 27;
		} else if(!areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
			//dlr
			ret = 28;
		} else if(areaArray[y, x].U & areaArray[y, x].D && areaArray[y, x].L && areaArray[y, x].R) {
			//fourway
			ret = 29;
		} else {
			//zero ways
			//Debug.Log("bad");
		}
		return ret;
	}
	


	//Depth-first search using backtracking LIMITED
	void FillArray(Vector2 start) {
		areaArray = new cell[(int)areaArraySize.y, (int)areaArraySize.x];
		cell blank = new cell ();
		blank.i = -1;
		blank.visited = false;
		
		//fill difficulty map
		// - used to spawn more enemies further away from the entrance.
		string diff ="";
		for(int y=0; y<areaArraySize.y; y++) {
			for(int x=0; x<areaArraySize.x; x++) {
				int diffMod = ((int)areaArraySize.x)/4;
				areaArray[y,x] = blank;
				areaArray[y,x].difficulty = Mathf.FloorToInt(Mathf.Sqrt((x-start.x)*(x-start.x) + (y-start.y)*(y-start.y)))/diffMod;
				diff += areaArray[y,x].difficulty + ",";
			}
			diff += "\n";
		}
		
		Debug.Log ("Difficulty map: \n" + diff);
	
		//for generation initialization
		int unused = numUnused();
		
		int ax = (int)start.x;
		int ay = (int)start.y;
		int rand =0;
		int popcount =0;
		int exec =0;
		string res = "";

		//continue until there are no unused cells in the array left.
		while(numUnused() > 0) {
			
			res = getUnvisited(ax, ay);
			if(res != "") {
				areaArray[ay, ax].visited = true;				
				
				rand = Random.Range(0, res.Length);
				
				char check = res.ToCharArray()[rand];
				
				if(check == 'U') {
					areaArray[ay, ax].U = true;
					areaArray[ay-1, ax].D = true;
					ay -= 1;
				} else if (check == 'D') {
					areaArray[ay, ax].D = true;
					areaArray[ay+1, ax].U = true;
					ay += 1;
				} else if (check == 'L') {
					areaArray[ay, ax].L = true;
					areaArray[ay, ax-1].R = true;
					ax -= 1;
				
				} else if (check == 'R') {
					areaArray[ay, ax].R = true;
					areaArray[ay, ax+1].L = true;
					ax += 1;
				} else {
					Debug.Log ("error");
				}
				s.Push(areaArray[ay,ax]);
			} else if (s.Count>0) {
				popcount ++;
				cell temp = (cell)s.Pop();
				//temp.visited = true;
				ay = temp.y;
				ax = temp.x;
			} else {
				Vector2 r = findUnunsed();
				if (r.x == -1) {
					Application.Quit();}
				else {
				ay =(int) r.y;
				ax =(int) r.x;
				areaArray[ay, ax].visited = true;
				}
			}
			
			exec++;
			if(exec> 1000)
				break;
		} //initial call
		
		//if for some reason a cell has no exits, give it as many exists
		//as possible.
		//This may seem like a cheap fix, but it opens the maze up nicely i've found.
		for(int y=0; y<(int)areaArraySize.y; y++) {
			for(int x=0; x<(int)areaArraySize.x; x++) {
				if(!areaArray[y,x].U && !areaArray[y,x].D && !areaArray[y,x].R && !areaArray[y,x].L) {
					if(y != 0) {
						areaArray[y,x].U = true;
						areaArray[y-1,x].D = true;
					}
					
					if(y != (int) areaArraySize.y-1) {
						areaArray[y,x].D = true;
						areaArray[y+1,x].U = true;
						}
					
					if(x != 0) {
						areaArray[y,x].L = true;
						areaArray[y,x-1].R = true;
						}
					if(x != (int) areaArraySize.x-1) {
						areaArray[y,x].R = true;
						areaArray[y,x+1].L = true;
					}
				}
			}
		}	
		
	}
	
	//start at end location, attempt to map back to start
	//opens a room up if no path exists.
	public void AssurePath(int x, int y, int x2, int y2) {
		Debug.Log("area array sizes, x y: " + areaArraySize.x + ", " + areaArraySize.y);
	
		//first reset visited boolean on array
		for(int ix=0; ix<areaArraySize.x; ix++) {
			for(int iy=0; iy<areaArraySize.y; iy++) {
				areaArray[iy,ix].visited = false;
			}
		}
		//reset stack to hold visited cells
		s.Clear();
		
		//execute helper - returns false if destination found
		//true otherwise. Information about visited cells stored
		//in stack.
		bool ret = assurePathHelper(x,y,x2,y2);

		//loop finding and opening shortest paths until destination is found.
		while(!ret) {
				
			//go through and find closest cell
			cell o = (cell) s.Pop();
			while(s.Count>0) {
				if(((cell)s.Peek()).i<o.i) {
					o= (cell)s.Pop();
				} else {
					s.Pop();
				}
			}
			
			//Debug.Log ("cell found: " + o.x + "x, " + o.y + "y, i:" + o.i);
			
			//guess and find correct direction to open pathway
			int guess =Mathf.Abs(o.x-x2);
			if(guess<Mathf.Abs(o.y-y2)) {
				guess = Mathf.Abs (o.y-y2);
				if(o.y>y2) {
					//U
					//Debug.Log ("opening U");
					areaArray[o.y,o.x].U = true;
					areaArray[o.y-1,o.x].D = true;
				} else {
					//D
					//Debug.Log ("opening D");
					
					areaArray[o.y,o.x].D = true;
					areaArray[o.y+1,o.x].U = true;
				}
				
			} else {
				if(o.x<x2) {
					//r
				//	Debug.Log ("opening R");
					
					areaArray[o.y,o.x].R = true;
					areaArray[o.y,o.x+1].L = true;
				} else {
					//l	
					//Debug.Log ("opening L");
					
					areaArray[o.y,o.x].L = true;
					areaArray[o.y,o.x-1].R = true;
				}
			}
			
			//Now that a path has been opened up, try again to find the destination
			ret = assurePathHelper(o.x,o.y,x2,y2);
			//ran ++;
		}
	}
	
	//assurePathHelper - recursive helper function for assuring a path.
	//starts at x,y and attempts to find a path to x2,y2
	bool assurePathHelper(int x, int y, int x2, int y2) {
		//set current cell to visited
		areaArray[y,x].visited = true;
		//assume path doesn't complete for now
		bool ret = false;
		
		//When destination is found, recurse answer back to initial call
		if(x==x2 && y==y2)
			return true;
		
		//this is was to double check this value was initially set
		areaArray[y,x].x = x;
		areaArray[y,x].y = y;
		
		//store the calculated distance between this cell and the destination.
		areaArray[y,x].i = Mathf.Sqrt((x-x2)*(x-x2) + (y-y2)*(y-y2));
		
		//store this cell in the stack 's'
		s.Push(areaArray[y,x]);
		

		/** Step through the map in the order up, right, left, then down.
		* Step in as long as the destination hasn't been found,
		* or the neighbor cell is visited. Can only step into cells
		* if they have an opening in that direction, which is stored
		* in the UDLR bools in the cell struct.
		* Recurse until all visitable cells are visited or the destination
		* is found.
		**/
		if(areaArray[y,x].U && !ret) {
			if(!areaArray[y-1,x].visited) {
				ret = assurePathHelper(x,y-1, x2,y2);
			}
		}if (areaArray[y,x].R && !ret) {
			if(!areaArray[y,x+1].visited) {
				ret = assurePathHelper(x+1,y, x2,y2);
				
			}
		}if (areaArray[y,x].L && !ret) {
			if(!areaArray[y,x-1].visited) {
				ret = assurePathHelper(x-1,y,x2,y2);
				
			}
		}if (areaArray[y,x].D && !ret) {
			if(!areaArray[y+1,x].visited) {
				ret =assurePathHelper(x,y+1,x2,y2);
				
			}
		}
		return ret;
		
	}
	
	//for debug
	//print the generated map out as annoying to read integers.
	void printArray(int[,] arr) {
		string l = "";
		for (int y=0; y< areaArraySize.y; y++) {
			for(int x=0; x< areaArraySize.x; x++) {
				l += arr[y,x];
			}
			Debug.Log (l);
			l= "";
		}
	}
	
	//Returns a string combination stating each neighbor cell that is unvisited.
	string getUnvisited(int ax, int ay) {
		string outR = "";
		
		int mx = (int) areaArraySize.x-1;
		int my = (int) areaArraySize.y-1;
		
		if(ax >0 && ax<mx){
		if(ay > 0) {
			if(!areaArray[ay-1, ax].visited)
				outR += "U";}
		
		if(ay < my) {
			if(!areaArray[ay+1, ax].visited)
				outR += "D";}
		}
		
		if(ay >0 && ay<my) {		
		if(ax > 0) {
			if(!areaArray[ay, ax-1].visited)
				outR += "L";}
				
		if(ax < mx) {
			if(!areaArray[ay, ax+1].visited)
				outR += "R";}
	    }
		
		return outR;
	}
	

	
	

	//for DFS algorithm
	//Find the number of cells in the array that
	//are not visited and return that number.
	int numUnused() {
		int outR =0;
		for (int y=0; y< areaArraySize.y; y++) {
			for(int x=0; x< areaArraySize.x; x++) {
				if(!areaArray[y,x].visited) {
					outR += 1;	
				}
			}
		}
		
		return outR;
	}
	
	//for DFS algorithm
	//Find all unvisted cells in the array and return
	//the x,y coordinates of a randomly chosen one.
	Vector2 findUnunsed() {
		int i=0;
		
		if(numUnused() !=0) {
		
		Vector2[] hits = new Vector2[numUnused()];
		
		for (int y=0; y< areaArraySize.y; y++) {
			for(int x=0; x< areaArraySize.x; x++) {
				if(!areaArray[y,x].visited) {
					
					hits[i] = new Vector2(x,y);
					i++;
				}
			}
		}
		
		if(i ==0)
			Application.Quit();
		
		int f = Random.Range(0, i-1); 
		//Debug.Log(hits[f].x+ " " + hits[f].y);
		
		return hits[f];
		}
		
		Application.Quit();
		return new Vector2(-1f,-1f);
	}
}

/****OLD CODE****/

/*
	//takes a subsection of the area array by paramaterized bounds and excludes it from outlying cells
	//min size 2x2
	//INCOMPLETE
	void cutZone(int X, int Y, int sizeX, int sizeY) {
	cell temp;
		for(int y=Y; y<sizeY; y++) {
			for(int x=X; x<sizeX; x++) {
				temp = areaArray[y,x];
				
				//top of zone	
				if(y==Y) {
					temp.U = false;
					temp.D = true;
					areaArray[y+1,x].U = true;
					//top left
					if(x==X) {
						temp.L = false;
					} else {
						temp.L = true;
						areaArray[y,x-1].R = true;
					}
					
					//top right
					if(x==sizeX) {
						temp.R = false;
					} else {
						temp.R = true;
						areaArray[y,x+1].R = true;
					}
					
				} else if (y==sizeY) {
					//bottom row
					temp.U = true;
					temp.D = false;
					areaArray[y-1,x].D = true;
					//bottom left
					if(x==X) {
						temp.L = false;
					} else {
						temp.L = true;
						areaArray[y,x-1].R = true;
					}
					
					//bottom right
					if(x==sizeX) {
						temp.R = false;
					} else {
						temp.R = true;
						areaArray[y,x+1].R = true;
					}
				} else {
					//all middle rows
				
				}
			}
		}
	}
	
	//make blank array ( not really used anymore)
	void MakeBlankArray() {
		//Fill array with tunel down the middle to the centre
		areaArray = new cell[(int)areaArraySize.y, (int)areaArraySize.x];
		int hx = (int)areaArraySize.x / 2;
		int hy = (int)areaArraySize.y / 2;
		//Debug.Log (hx + ", " +hy);

		cell blank = new cell ();
		blank.i = -1;
		blank.visited = false;

		cell ud = new cell ();
		ud.i = 19;
		ud.visited = true;
		ud.U = true;
		ud.D = true;

		cell landing = new cell ();
		landing.i = -1;
		landing.visited = true;
		landing.U = true;
		
		//Debug.Log (areaArraySize);
		
		for(int y=0; y<areaArraySize.y; y++) {
			for(int x=0; x<areaArraySize.x; x++) {
				areaArray[y,x] = blank;
				
				
				
				if(x == hx) { //on midwaypoint fill tunnel
					if(y<hy) {
						if ( y == 0)
							ud.U = false;
						ud.x = x;
						ud.y = y;
						areaArray[y,x] = ud;
						s.Push(areaArray[y, x]);
					} else if(y==hy) {
						landing.x = x;
						landing.y = y;
						areaArray[y, x] = landing;
						s.Push(areaArray[y, x]);
					} else if (y>hy) {
						blank.x = x;
						blank.y = y;
						areaArray[y, x] = blank;
			

}
*/
