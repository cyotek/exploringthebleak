 /*************************************************************\
 * This function was created by Nathaniel Inman of The Other  *
 * Experiment Studio for use in the 2013 7DRL Three.RL. This  *
 * function performs Diffusion Limited Aggregation on a map   *
 * while saving the variables to the multidimensional map and *
 * returning a boolean on whether the map was made            *
 * successfully or not.                                       *
 *------------------------------------------------------------*
 *           R E Q U I R E D      C O N S T A N T S           *
 *------------------------------------------------------------*
 * tileUnused - specifies which value is default unused       *
 * tileDirtFloor - specifies what value to set the floor to   *
 * tileDirtWall - specifies which value is the wall           *
 *------------------------------------------------------------*
 *              P A S S E D       V A R I A B L E S           *
 *------------------------------------------------------------*
 * map  - the main 2 dimensional array                        *
 *  | .type - contains the type at the coordinate specifies   *
 *  | ...   - other variables won't be affected
 * size - the max size of x and y coordinates                 *
 * cx   - the x coordinate of the player                      *
 * cy   - the y coordinate of the player                      *
 * drawLength - specifies maximum cooridors length            *
 * drawTypes:                                                 *
 *   0  - Both are allowed, but cut wide for cardinal         *
 *   1  - Both are allowed                                    *
 *   2  - Diagonal only, but cut wide to allow cardinal move  *
 *   3  - Diagonal only                                       *
 *   4  - Cardinal only                                       *
 *------------------------------------------------------------*
 *           R E T U R N E D      V A R I A B L E S           *
 *------------------------------------------------------------*
 * map.type  - Alters from intial value to tileDirtFloor      *
 * generateMap_DLA - returns true if successful, false if not *
\**************************************************************/
function generateMap_DLA(map,size,cx,cy,drawLength,drawType){
	/* Functional Variables */
	var builderSpawned=0;
	var builderMoveDirection=0;
	var allocatedBlocks=0; //variable used to track the percentage of the map filled
	var rootX=Math.floor(size/2);rootY=Math.floor(size/2); //this is where the growth starts from, currently center of map
	var stopped=drawLength; //this is how long cooridors can be
	/* The Diffusion Limited Aggregation Loop */
	while (allocatedBlocks<((size*size)/8)){ //quit when an eighth of the map is filled
		if (builderSpawned!=1){
			//Spawn at random position
			cx=2+Math.floor(Math.random()*size-2);
			cy=2+Math.floor(Math.random()*size-2);
			//See if builder is ontop of root
			if (Math.abs(rootX-cx)<=0 && Math.abs(rootY-cy)<=0){
				//builder was spawned too close to root, clear the floor and respawn
				if (map[cx][cy].type!=1){
					map[cx][cy].type=tileDirtFloor;
					allocatedBlocks++;
				} //end if
			} else {
				builderSpawned=1;
				builderMoveDirection = (drawType==3||drawType==2?4:0)+Math.floor(Math.random()*(drawType<2?8:4));
				stepped=0;
			} //end if
		} else{ //builder already spawned and knows it's direction, move builder
			/* North    */	     if(builderMoveDirection==0 && cy>0              ){cy--;     stepped++;
			/* East	    */} else if(builderMoveDirection==1 && cx<size           ){cx++;     stepped++;
			/* South    */} else if(builderMoveDirection==2 && cy<size           ){cy++;     stepped++;
			/* West	    */} else if(builderMoveDirection==3 && cx>0              ){cx++;     stepped++;
			/* Northeast*/} else if(builderMoveDirection==4 && cx<size && cy>0   ){cy--;cx++;stepped++;
			/* Southeast*/} else if(builderMoveDirection==5 && cx<size && cy<size){cy++;cx++;stepped++;
			/* Southwest*/} else if(builderMoveDirection==6 && cx>0 && cy<size   ){cy++;cx--;stepped++;
			/* Northwest*/} else if(builderMoveDirection==7 && cx>0 && cy>0      ){cy--;cx--;stepped++;}
			/* ensure that the builder is touching an existing spot */
			if (cx<size && cy<size && cx>1 && cy>1 && stepped<=stopped){
			/* East		*/       if(map[cx+1][cy  ].type==tileDirtFloor){if (map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;}
			/* West		*/} else if(map[cx-1][cy  ].type==tileDirtFloor){if (map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;}
			/* South	*/} else if(map[cx  ][cy+1].type==tileDirtFloor){if (map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;}
			/* North	*/} else if(map[cx  ][cy-1].type==tileDirtFloor){if (map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;}
			/* Northeast*/} else if(map[cx+1][cy-1].type==tileDirtFloor){if (    map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;
			                                          if (drawType%2==0){        map[cx+1][cy].type =tileDirtFloor;                                allocatedBlocks++;}}
			/* Southeast*/} else if(map[cx+1][cy+1].type==tileDirtFloor){if (    map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;
			                                          if (drawType%2==0){        map[cx+1][cy].type =tileDirtFloor;                                allocatedBlocks++;}}
			/* Northwest*/} else if(map[cx-1][cy-1].type==tileDirtFloor){if (    map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;
			                                          if (drawType%2==0){        map[cx-1][cy].type =tileDirtFloor;                                allocatedBlocks++;}}
			/* Southwest*/} else if(map[cx-1][cy-1].type==tileDirtFloor){if (    map[cx  ][cy].type!=tileDirtFloor){map[cx][cy].type=tileDirtFloor;allocatedBlocks++;
			                                          if (drawType%2==0){        map[cx-1][cy].type =tileDirtFloor;                                allocatedBlocks++;}}}
			} else {builderSpawned=0;}
		} //end if
	} //end while
	/* now draw walls */
	var found=0,type=2,maxType=1;
		for(var i=0;i<size;i++){
			for(var j=0;j<size;j++){
				if(map[i][j].type==tileDirtFloor){
					if(i>0            ){if(map[i-1][j  ].type==tileUnused)map[i-1][j  ].type=tileDirtWall;} //to the left
					if(i<size         ){if(map[i+1][j  ].type==tileUnused)map[i+1][j  ].type=tileDirtWall;} //to the right
					if(j>0            ){if(map[i  ][j-1].type==tileUnused)map[i  ][j-1].type=tileDirtWall;} //top
					if(j<size         ){if(map[i  ][j+1].type==tileUnused)map[i  ][j+1].type=tileDirtWall;} //bottom
					if(i>0&&j>0       ){if(map[i-1][j-1].type==tileUnused)map[i-1][j-1].type=tileDirtWall;} //topleft
					if(i<size&&j<size ){if(map[i+1][j+1].type==tileUnused)map[i+1][j+1].type=tileDirtWall;} //bottomright
					if(i>0&&j<size    ){if(map[i-1][j+1].type==tileUnused)map[i-1][j+1].type=tileDirtWall;} //bottomleft
					if(i<size&&j>0    ){if(map[i+1][j-1].type==tileUnused)map[i+1][j-1].type=tileDirtWall;} //topright
				} //end if
			} //end for
		} //end for
	return true;
} //end function