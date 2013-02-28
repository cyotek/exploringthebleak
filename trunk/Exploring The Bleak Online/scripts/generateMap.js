/***********************************************************************************\
 * Copyright (c) 2012, Nathaniel Inman                                             *
 * All rights reserved.                                                            *
 *                                                                                 *
 * Redistribution and use in source and binary forms, with or without              *
 * modification, are permitted provided that the following conditions are met:     *
 *     * Redistributions of source code must retain the above copyright            *
 *       notice, this list of conditions and the following disclaimer.             *
 *     * Redistributions in binary form must reproduce the above copyright         *
 *       notice, this list of conditions and the following disclaimer in the       *
 *       documentation and/or other materials provided with the distribution.      *
 *     * Neither the name of the The Other Experiment Studio nor the               *                
 *       names of its contributors may be used to endorse or promote products      *
 *       derived from this software without specific prior written permission.     *
 *                                                                                 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND *
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED   *
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE          *
 * DISCLAIMED. IN NO EVENT SHALL NATHANIEL INMAN BE LIABLE FOR ANY                 *
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES      *
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;    *
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND     *
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT      *
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS   *
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                    *
\***********************************************************************************/
/* Main Mapping Variables & declarations */
function initializeMap(){
	map = new Array(size);
	mobMap = new Array(size);
	itemMap = new Array(size);
	deadMap = new Array(size);
	wallMap = new Array(size);
	visMap = new Array(size);
	// Set the map array
	for (var i = 0; i <= size * size; ++i){
		map[i] = new Array(size);
		mobMap[i] = new Array(size);
		itemMap[i] = new Array(size);
		deadMap[i] = new Array(size);
		wallMap[i] = new Array(size);
		visMap[i] = new Array(size);
	}
	// Initialize the Map Array to Zeros 
	for (x=1;x<=size;++x){
		for (y=1;y<=size;++y){
			map[x][y]=0;
			mobMap[x][y]=0;
			itemMap[x][y]=0;
			deadMap[x][y]=0;
			wallMap[x][y]=0;
			visMap[x][y]=0;
		} //end for
	} //end for
} //end function
function clearMap(){
	context.fillStyle='black';
	var wall,randomWall; //used to get a random wall type
	// Set the map to zeros (walls)
	for (x=1;x<=size;++x){
		for (y=1;y<=size;++y){
			map[x][y]=0;
			mobMap[x][y]=0;
			itemMap[x][y]=0;
			deadMap[x][y]=0;
			wallMap[x][y]=0;
			visMap[x][y]=0;
			wall=false; //reset wall before we try getting next
			do{
				randomWall=Math.floor(Math.random()*8) //randomly choose a wall and make sure it's available in environment
				if (       randomWall==0 && environment.wall.floor==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==1 && environment.wall.water==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==2 && environment.wall.flora==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==3 && environment.wall.rock ==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==4 && environment.wall.shrub==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==5 && environment.wall.tree ==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==6 && environment.wall.lava ==1){wallMap[x][y]=randomWall;wall=true;
				} else if (randomWall==7 && environment.wall.wall ==1){wallMap[x][y]=randomWall;wall=true;}
			} while (!wall)
			context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize+1,roomsize+1);
		} //end for
	} //end for
} //end function
function resetMap(){
	context.fillStyle='black';
	// Set the map to zeros (walls)
	for (x=1;x<=size;++x){
		for (y=1;y<=size;++y){
			map[x][y]=0;
			mobMap[x][y]=0;
			itemMap[x][y]=0;
			deadMap[x][y]=0;
			wallMap[x][y]=0;
			context.fillRect(x*roomsize-roomsize+3,y*roomsize-roomsize+3,roomsize+1,roomsize+1);
		} //end for
	} //end for
} //end function
function generateMap(){
	pickEnvironment(); //get the environment so that clearmap can set the random wall types
	clearMap();
	/* Functional Variables */
		var builderSpawned=0;
		var builderMoveDirection=0;
		var curBlocks=0,maxBlocks=0.1; //maxBlocks is percentage of map filled 0-1
		var rootX=12,rootY=12; //this is where the growth starts from. Currently center of map
		var curStep=0,maxStep=5; //this is how long corridors can be
		var orthogonalAllowed=0; //Orthogonal movement allowed? If not, it carves a wider cooridor on diagonal
		var carveCardinal=1; //carve Cardinal directions allowed? Either carve Cardinal or Orthogonal MUST be enabled
		var carveOrthogonal=1; //carve Orthogonal directions allowed?
	/* The Diffusion Limited Aggregation Loop */
	while (curBlocks<(size*size*maxBlocks)){ //quit when an eighth of the map is filled
		if (builderSpawned!=1){
			//Spawn at random position
			cx = 2+Math.floor(Math.random()*size-2);
			cy = 2+Math.floor(Math.random()*size-2);
			//See if builder is ontop of root
			if (Math.abs(rootX - cx)<=0 && Math.abs(rootY-cy)<=0){
				//builder was spawned too close to root, clear that floor and respawn
				if (map[cx][cy]!=1){
					map[cx][cy]=1;
					curBlocks++;
				} //end if
			} else {
				builderSpawned = 1;
				builderMoveDirection = Math.floor(Math.random()*8);
				curStep=0;
			} //end if
		} else { //builder already spawned and knows it's direction, move builder
			/* North     */        if (builderMoveDirection==0 && cy>0              ){cy--;curStep++;
			/* East      */ } else if (builderMoveDirection==1 && cx<size           ){cx++;curStep++;
			/* South     */ } else if (builderMoveDirection==2 && cy<size           ){cy++;curStep++;
			/* West      */ } else if (builderMoveDirection==3 && cx>0              ){cx++;curStep++;
			/* Northeast */ } else if (builderMoveDirection==4 && cx<size && cy>0   ){cy--;cx++;curStep++;
			/* Southeast */ } else if (builderMoveDirection==5 && cx<size && cy<size){cy++;cx++;curStep++;
			/* Southwest */ } else if (builderMoveDirection==6 && cx>0 && cy<size   ){cy++;cx--;curStep++;
			/* Northwest */ } else if (builderMoveDirection==7 && cx>0 && cy>0      ){cy--;cx--;curStep++;}
			/* ensure that the builder is touching an existing spot */
			if (cx<size && cy<size && cx>1 && cy>1 && curStep<=maxStep){
			/* East      */        if (map[cx+1][cy]==1  && carveCardinal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;}
			/* West      */ } else if (map[cx-1][cy]==1  && carveCardinal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;} 
			/* South     */ } else if (map[cx][cy+1]==1  && carveCardinal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;}
			/* North     */ } else if (map[cx][cy-1]==1  && carveCardinal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;}
			/* Northeast */ } else if (map[cx+1][cy-1]==1 && carveOrthogonal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;
								 if (!orthogonalAllowed){map[cx+1][cy]=1;curBlocks++;}}
			/* Southeast */ } else if (map[cx+1][cy+1]==1 && carveOrthogonal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;
								 if (!orthogonalAllowed){map[cx+1][cy]=1;curBlocks++;}}
			/* Southwest */ } else if (map[cx-1][cy+1]==1 && carveOrthogonal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++; 
								 if (!orthogonalAllowed){map[cx-1][cy]=1;curBlocks++;}}
			/* Northwest */ } else if (map[cx-1][cy-1]==1 && carveOrthogonal){
							if (map[cx][cy]!=1){map[cx][cy]=1;curBlocks++;
								 if (!orthogonalAllowed){map[cx-1][cy]=1;curBlocks++;}}}
			} else { builderSpawned=0; }
		} //end if
	} //end while
	positionLocations();
} //end generateMap
function positionLocations(){
	/* Find a suitable location for the player */
	if (player.cDepth==1){
		do {
			player.mapPos.x=Math.floor(Math.random()*size+1);
			player.mapPos.y=Math.floor(Math.random()*size+1);
		} while(map[player.mapPos.x][player.mapPos.y]!=1)
		stairsUpX=-1;stairsUpY=-1;
		textOutput("<font color='55AA55'>"+player.pname+", a "+player.prace+" "+player.pclass+" descends the stairs and begins exploring the bleak.</font>");
		textOutput("<font color='5555AA'>Press ? or h for help.</font>");
		displayDepth();
	} else { //Only draw the stairs up if the floor is below the main floor 
		do {
			cx=Math.floor(Math.random()*size+1);
			cy=Math.floor(Math.random()*size+1);
		} while(map[cx][cy]!=1 || cx==player.mapPos.x && cy==player.mapPos.y)
		stairsUpX=cx;stairsUpY=cy;
		player.mapPos.x=cx;player.mapPos.y=cy;
		displayDepth();
	} //end if
	do {
		cx=Math.floor(Math.random()*size+1);
		cy=Math.floor(Math.random()*size+1);
	} while(map[cx][cy]!=1 || cx==stairsUpX && cy==stairsUpY || cx==player.mapPos.x && cy==player.mapPos.y)
	stairsDownX=cx;stairsDownY=cy;
	generateItems();
	generateMobs();
	playerFOV(player.fieldOfViewRange,player.mapPos.x,player.mapPos.y,1);
	drawHP();drawWP();
} //end function
function displayDepth(){
	var vowel="a"
	if (environmentName.charAt(0)=="A")vowel="an";if (environmentName.charAt(0)=="E")vowel="an";
	if (environmentName.charAt(0)=="I")vowel="an";if (environmentName.charAt(0)=="O")vowel="an";
	if (environmentName.charAt(0)=="U")vowel="an";
	textOutput("You've reached "+vowel+" <font color='"+environment.color+"'>"+environment.name+"</font> on depth "+player.cDepth+" of the Bleak.");
} //end function
function pickEnvironment(){
	/* Main Mapping Variables & declarations */
	var randomEnvironment;
	randomEnvironment=Math.floor(Math.random()*environmentDB.max) //truncate the last two (player & none)
	environment.color=new String(environmentDB.color[randomEnvironment].replace('0x',''));
	environment.name=environmentDB.name[randomEnvironment];
	environment.wall.floor=environmentDB.wall.floor[randomEnvironment];
	environment.wall.water=environmentDB.wall.water[randomEnvironment];
	environment.wall.flora=environmentDB.wall.flora[randomEnvironment];
	environment.wall.rock= environmentDB.wall.rock[ randomEnvironment];
	environment.wall.shrub=environmentDB.wall.shrub[randomEnvironment];
	environment.wall.tree= environmentDB.wall.tree[ randomEnvironment];
	environment.wall.lava= environmentDB.wall.lava[ randomEnvironment];
	environment.wall.wall= environmentDB.wall.wall[ randomEnvironment];
} //end function

