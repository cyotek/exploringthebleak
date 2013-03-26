/**************************************************************\
 * This function was created by Nathaniel Inman of The Other  *
 * Experiment Studio for use in the 2013 7DRL Three.RL. This  *
 * function performs Cellular Automata using 4:5r on a map    *
 * while saving the variables to the multidimensional map and *
 * returning a boolean on whether the map was made            *
 * successfully or not.                                       *
 * Originally I had to use a map array and then a secondary   *
 * map array to hold the upcoming map so that the current map *
 * information isn't conflicting with itself. This is because *
 * automata relies entirely on the entire maps state. The     *
 * problem with this is performance issues when iterating     *
 * through the entire secondary map information to throw onto *
 * the primary with each pull. To avoid this, i'll alternate  *
 * which map is the primary map each round. This effectively  *
 * just increases the required memory for the function to     *
 * operate instead of increasing additional processing time.  *
 *------------------------------------------------------------*
 *           R E Q U I R E D      C O N S T A N T S           *
 *------------------------------------------------------------*
 * tileUnused - Specifies which tile is the initialized value *
 * tileDirtFloor - The walkable areas of the map              *
 *------------------------------------------------------------*
 *           F U N C T I O N S    C O N T A I N E D           *
 *------------------------------------------------------------*
 * map_generateAGC() returns true/false on success            *
 *------------------------------------------------------------*
 *           R E Q U I R E D      V A R I A B L E S           *
 *------------------------------------------------------------*
 * map  - the main 2 dimensional array                        *
 *  | .type - distinguishes the tile type at the coordinate   *
 * size - the max size of x and y coordinates                 *
 * iterations - determines how many times the cellular        *
 *              automata is ran before returning the result   *
 *------------------------------------------------------------*
 *           R E T U R N E D      V A R I A B L E S           *
 *------------------------------------------------------------*
 * map.type  - Alters from intial value to one of the required*
 *             constants visible above                        *
\**************************************************************/

function map_generateAGC(size,iterations,type){
	iterations=5;
	/* local variable declaration */
	var randomTile;
	/* function to determine moores neighborhood */
	var testSides = function(x,y,size){
		var _amount=0;
		if(x>0&&y>0      &&type!=2){if(iterations%2==0?map[x-1][y-1].type==tileDirtFloor:map2[x-1][y-1].type==tileDirtFloor)_amount++;}
		if(x>0           ){if(iterations%2==0?map[x-1][y  ].type==tileDirtFloor:map2[x-1][y  ].type==tileDirtFloor)_amount++;}
		if(x>0&&y<size   &&type!=2){if(iterations%2==0?map[x-1][y+1].type==tileDirtFloor:map2[x-1][y+1].type==tileDirtFloor)_amount++;}
		if(y>0           ){if(iterations%2==0?map[x  ][y-1].type==tileDirtFloor:map2[x  ][y-1].type==tileDirtFloor)_amount++;}
		if(y<size        ){if(iterations%2==0?map[x  ][y+1].type==tileDirtFloor:map2[x  ][y+1].type==tileDirtFloor)_amount++;}
		if(x<size&&y>0   &&type!=2){if(iterations%2==0?map[x+1][y-1].type==tileDirtFloor:map2[x+1][y-1].type==tileDirtFloor)_amount++;}
		if(x<size        ){if(iterations%2==0?map[x+1][y  ].type==tileDirtFloor:map2[x+1][y  ].type==tileDirtFloor)_amount++;}
		if(x<size&&y<size&&type!=2){if(iterations%2==0?map[x+1][y+1].type==tileDirtFloor:map2[x+1][y+1].type==tileDirtFloor)_amount++;}
		return _amount;
	};
	/* function to remove orphaned locations */
	var clipOrphaned = function(){
		var node = {x:0,y:0};
		var loc_max= {val:0,cur:0,num:0,max:0};
		var unmapped=[];
		var traverse_look = function(i,j){ //look around at location and push unmapped nodes to stack
			if(i>0){if(map[i-1][j].type==tileDirtFloor&&map[i-1][j].loc==0){
				node={x:i-1,y:j};
				unmapped.push(node);map[i-1][j].loc=-1;}}
			if(j>0){if(map[i][j-1].type==tileDirtFloor&&map[i][j-1].loc==0){
				node={x:i,y:j-1};
				unmapped.push(node);map[i][j-1].loc=-1;}}
			if(i<size){if(map[i+1][j].type==tileDirtFloor&&map[i+1][j].loc==0){
				node={x:i+1,y:j};
				unmapped.push(node);map[i+1][j].loc=-1;}}
			if(j<size){if(map[i][j+1].type==tileDirtFloor&&map[i][j+1].loc==0){
				node={x:i,y:j+1};
				unmapped.push(node);map[i][j+1].loc=-1;}}
		};
		var traverse = function(curLoc,i,j){ //traverse a location completely
			var newLoc = node;
			loc_max.val=1; //set the current mas size to 1
			map[i][j].loc=curLoc;
			traverse_look(i,j);
			while(unmapped.length>0){
				newLoc=unmapped.pop();i=newLoc.x;j=newLoc.y;
				traverse_look(i,j);
				map[i][j].loc=curLoc;
				loc_max.val++;
				if(loc_max.val>loc_max.max){
					loc_max.max=loc_max.val;
					loc_max.num=loc_max.cur;
				} //end manage maximum mass
			} //end while
		};
		for(var i=0;i<size;i++){
			for(var j=0;j<size;j++){
				if(map[i][j].type==tileDirtFloor&&map[i][j].loc==0){traverse(++loc_max.cur,i,j);}
			} //end for
		} //end for
		for(var i=0;i<size;i++){
			for(var j=0;j<size;j++){
				if(map[i][j].type==tileDirtFloor&&map[i][j].loc!=loc_max.num){map[i][j].type=tileUnused;}
			} //end for
		} //end for
	}; //end function
	/* cavern generation */
	var runIterations = function(iterations,type){
		var procedure=0;
		do{
			for(var i=0;i<size;i++){
				for(var j=0;j<=size;j++){
					var mooresNeighborhood=testSides(i,j,size,type);
					if(type==0){
						if(iterations%2==0?map[i][j].type==tileDirtFloor:map2[i][j].type==tileDirtFloor){
							if(mooresNeighborhood>=4){
								iterations%2==0?map2[i][j].type=tileDirtFloor:map[i][j].type=tileDirtFloor;
							}else{
								iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
								} //end if
						}else{
							if(mooresNeighborhood>=5){ 
								iterations%2==0?map2[i][j].type=tileDirtFloor:map[i][j].type=tileDirtFloor;
							}else{
								iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
							} //end if
						} //end if
					}else if(type==1){
						if(iterations%2==0?map[i][j].type==tileDirtFloor:map2[i][j].type==tileDirtFloor){
							if(mooresNeighborhood>=4){
								iterations%2==0?map2[i][j].type=tileDirtFloor:map[i][j].type=tileDirtFloor;
							}else{
								iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
								} //end if
						}else{
							if(mooresNeighborhood>=5){ 
								iterations%2==0?map2[i][j].type=tileDirtFloor:map[i][j].type=tileDirtFloor;
							}else{
								iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
							} //end if
						} //end if
					}else if(type==2){
						if(iterations%2==0?map[i][j].type==tileUnused:map2[i][j].type==tileUnused){
							if(mooresNeighborhood==4){
								iterations%2==0?map2[i][j].type=tileDirtFloor:map[i][j].type=tileDirtFloor;
							} //end if
						}else if(iterations%2==0?map[i][j].type==tileDirtFloor:map2[i][j].type==tileDirtFloor){
							if(mooresNeighborhood<=2){
								iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
							} //end if
						} //end if
					} //end if
				} //end for
			} //end for
			iterations--;
		}while(iterations>0);
	}; //end runIterations
	/* start with some noise */
	for(var i=0;i<size;i++){
		for(var j=0;j<size;j++){
			randomTile=(Math.floor(Math.random()*99)<45?tileUnused:tileDirtFloor);
			/* make the primary map gain noise */
			iterations%2==0?map[i][j].type=randomTile:map2[i][j].type=randomTile;
			/* keep the secondary map blank */
			iterations%2==0?map2[i][j].type=tileUnused:map[i][j].type=tileUnused;
		} //end for
	} //end for
	runIterations(1,1);
	runIterations(1,2);
	runIterations(1,1);
	runIterations(1,0);
	clipOrphaned();
	return true;
} //end function