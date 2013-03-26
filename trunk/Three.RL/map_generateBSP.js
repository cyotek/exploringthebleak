/**************************************************************\
 * This function was created by Nathaniel Inman of The Other  *
 * Experiment Studio for use in the 2013 7DRL Three.RL. This  *
 * function performs Diffusion Limited Aggregation on a map   *
 * while saving the variables to the multidimensional map and *
 * returning a boolean on whether the map was made            *
 * successfully or not.                                       *
 *------------------------------------------------------------*
 *           R E Q U I R E D      C O N S T A N T S           *
 *------------------------------------------------------------*
 * tileUnused - Specifies which tile is the initialized value *
 * tileDirtWall - Wall in the walkable areas                  *
 * tileDirtFloor - The walkable areas of the map              *
 * tileCorridor - Specify the corridors so they can separated *
 *                from room areas for mobile spawning purposes*
 *                and other events.                           *
 * tileDoor - Specifies the doors on the map                  *
 * tileUpStairs - Specifies the way to the above dungeon      *
 * tileDownStairs - Specifies the way to the next dungeon     *
 *------------------------------------------------------------*
 *           F U N C T I O N S    C O N T A I N E D           *
 *------------------------------------------------------------*
 * setCell(x,y,type) - set a given coordinate of map to type  *
 * getCell(x,y) - get the type at the maps coordinate         *
 * getRand(min,max) - shortcut to random variable             *
 * makeCorridor(x,y,length,direction)                         *
 * makeRoom(x,y,xlength,ylength,direction)                    *
 * surroundCorridors() - surround corridors with walls        *
 * createDungeon(inx,iny,inobj) <return success as boolean>   *
 *------------------------------------------------------------*
 *           R E Q U I R E D      V A R I A B L E S           *
 *------------------------------------------------------------*
 * map  - the main 2 dimensional array                        *
 *  | .type - distinguishes the tile type at the coordinate   *
 * size - the max size of x and y coordinates                 *
 * cx   - the x coordinate of the player                      *
 * cy   - the y coordinate of the player                      *
 * chanceRoom - The chance a room is generated (%1-100)       *
 * chanceCorridor - Second dibs on generation (%1-100)        *
 * objects - generated objects on the map                     *
 *------------------------------------------------------------*
 *           R E T U R N E D      V A R I A B L E S           *
 *------------------------------------------------------------*
 * map.type  - Alters from intial value to one of the required*
 *             constants visible above                        *
\**************************************************************/

/* initialize some temporary "constants" to help with directions */
var _north=2,__north=0;
var _east=3,__east=1;
var _south=0,__south=2;
var _west=1,__west=3;
var _none=-1;

function setCell(x, y, type){
	map[x][y].type=type;
} //end setCell

function getCell(x, y){
	return map[x][y].type;
} //end getCell

function getRand(min, max){
	return Math.floor(Math.random() * (max - min + 1) + min);
} //end getRand

function makeCorridor(x, y, length, direction){
	if (x < 0 || x > size)return false;
	if (y < 0 || y > size)return false;
	var noDir=0;
	if(direction==__north){noDir=__south;
	}else if(direction==__east){noDir=__west;
	}else if(direction==__south){noDir=__north;
	}else if(direction==__west){noDir=__east;}
	var len = length;
	var floor = tileCorridor;
	var dir = direction;
	var xtemp = x;
	var ytemp = y;
	if(getCell(xtemp,ytemp)==tileUnused && xtemp!=0 && ytemp!=0 && xtemp!=size && ytemp!=size){setCell(xtemp, ytemp, floor);}else{return false;}
	do{
		if (dir == 0){ // north
			if (ytemp-1 < 0 && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(ytemp-1<0 &&len!=length){return true;}
			if (getCell(xtemp, ytemp-1)!=tileUnused && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(getCell(xtemp,ytemp-1)!=tileUnused && len!=length){return true;}
			ytemp--;
		}else if(dir == 1){ // east
			if (xtemp+1 > size && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(xtemp+1>size &&len!=length){return true;}
			if (getCell(xtemp+1, ytemp)!=tileUnused && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(getCell(xtemp+1,ytemp)!=tileUnused && len!=length){return true;}
			xtemp++;
		}else if(dir == 2){ // south
			if (ytemp+1 > size && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(ytemp+1>size &&len!=length){return true;}
			if (getCell(xtemp, ytemp+1)!=tileUnused && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(getCell(xtemp,ytemp+1)!=tileUnused && len!=length){return true;}
			ytemp++;
		}else if(dir == 3){ // west
			if (xtemp-1 < 0 && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(xtemp-1<0 &&len!=length){return true;}
			if (getCell(xtemp-1, ytemp)!=tileUnused && len==length){setCell(xtemp,ytemp,tileUnused);return false;
			}else if(getCell(xtemp-1,ytemp)!=tileUnused && len!=length){return true;}
			xtemp--;	
		} //end if
		if(xtemp==0||ytemp==0||xtemp==size||ytemp==size)return true;
		len--;
		setCell(xtemp, ytemp, floor);
		/* choose a different direction with each iteration */
		if(1+Math.floor(Math.random()*100)<=chanceCorridorBend){
			do{
				dir=Math.floor(Math.random()*4);
			}while(dir==noDir); //end while
		} //end if
	}while(len>0); //end do
	return true;
} //end function

function makeRoom(x, y, xlength, ylength, direction){
	//define the dimensions of the room, it should be at least 4x4 tiles (2x2 for walking on, the rest is walls)
	var xlen = getRand(4, xlength);
	var ylen = getRand(4, ylength);
	//tile type its going to be filled with
	var floor = tileDirtFloor;
	var wall = tileDirtWall;
	var dir = 0;
	if (direction > 0 && direction < 4)dir = direction;
	if (dir == 0){
		//north, check if there is enough space left for a room
		for (var ytemp = y; ytemp > (y - ylen); ytemp--){
			if (ytemp < 0 || ytemp > size)return false;
			for (var xtemp = (x - Math.floor(xlen / 2)); xtemp < (x + Math.floor((xlen + 1) / 2)); xtemp++){
				if (xtemp < 0 || xtemp > size)return false;
				if (getCell(xtemp, ytemp) != tileUnused)return false;
			} //end for
		} //end for
		//we're still here, build
		for (var ytemp = y; ytemp > (y - ylen); ytemp--){
			for (var xtemp = (x - Math.floor(xlen / 2)); xtemp < (x + Math.floor((xlen + 1) / 2)); xtemp++){
				//start with the walls
				if(xtemp == (x - Math.floor(xlen / 2))){ 
					setCell(xtemp, ytemp, wall);
				}else if(xtemp == (x + Math.floor((xlen - 1) / 2))){
					setCell(xtemp, ytemp, wall);
				}else if(ytemp == y){
					setCell(xtemp, ytemp, wall);
				}else if (ytemp == (y - ylen + 1)){
					setCell(xtemp, ytemp, wall);
				}else {
					setCell(xtemp, ytemp, floor); //and then fill with the floor
				} //end if
			} //end for
		} //end for
	}else if (dir == 1){
		//east
		for (var ytemp = (y - Math.floor(ylen / 2)); ytemp < (y + Math.floor((ylen + 1) / 2)); ytemp++){
			if (ytemp < 0 || ytemp > size)return false;
			for (var xtemp = x; xtemp < (x + xlen); xtemp++){
				if (xtemp < 0 || xtemp > size)return false;
				if (getCell(xtemp, ytemp) != tileUnused)return false;
			} //end for
		} //end for
		for (var ytemp = (y - Math.floor(ylen / 2)); ytemp < (y + Math.floor((ylen + 1) / 2)); ytemp++){
			for (var xtemp = x; xtemp < (x + xlen); xtemp++){
				if (xtemp == x){
					setCell(xtemp, ytemp, wall);
				}else if(xtemp == (x + xlen - 1)){
					setCell(xtemp, ytemp, wall);
				}else if (ytemp == (y - Math.floor(ylen / 2))){
					setCell(xtemp, ytemp, wall);
				}else if (ytemp == (y + Math.floor((ylen - 1) / 2))){
					setCell(xtemp, ytemp, wall);
				}else{
					setCell(xtemp, ytemp, floor);
				} //end if
			} //end for
		} //end for
	}else if (dir == 2){
		//south
		for (var ytemp = y; ytemp < (y + ylen); ytemp++){
			if (ytemp < 0 || ytemp > size)return false;
			for (var xtemp = (x - Math.floor(xlen / 2)); xtemp < (x + Math.floor((xlen + 1) / 2)); xtemp++){
				if (xtemp < 0 || xtemp > size)return false;
				if (getCell(xtemp, ytemp) != tileUnused)return false;
			} //end for
		} //end for
		for (var ytemp = y; ytemp < (y + ylen); ytemp++){
			for (var xtemp = (x - Math.floor(xlen / 2)); xtemp < (x + Math.floor((xlen + 1) / 2)); xtemp++){
				if (xtemp == (x - Math.floor(xlen / 2))){
					setCell(xtemp, ytemp, wall);
				}else if (xtemp == (x + Math.floor((xlen - 1) / 2))){
					setCell(xtemp, ytemp, wall);
				}else if (ytemp == y){
					setCell(xtemp, ytemp, wall);
				}else if (ytemp == (y + ylen - 1)){
					setCell(xtemp, ytemp, wall);
				}else{
					setCell(xtemp, ytemp, floor);
				} //end if
			} //end for
		} //end for
	}else if(dir == 3){
		//west
		for (var ytemp = (y - Math.floor(ylen / 2)); ytemp < (y + Math.floor((ylen + 1) / 2)); ytemp++){
			if (ytemp < 0 || ytemp > size)return false;
			for (var xtemp = x; xtemp > (x - xlen); xtemp--){
				if (xtemp < 0 || xtemp > size)return false;
				if (getCell(xtemp, ytemp) != tileUnused)return false; 
			} //end for
		} //end for
		for (var ytemp = (y - Math.floor(ylen / 2)); ytemp < (y + Math.floor((ylen + 1) / 2)); ytemp++){
			for (var xtemp = x; xtemp > (x - xlen); xtemp--){
				if (xtemp == x){
					setCell(xtemp, ytemp, wall);
				}else if(xtemp == (x - xlen + 1)){
					setCell(xtemp, ytemp, wall);
				}else if(ytemp == (y - Math.floor(ylen / 2))){
					setCell(xtemp, ytemp, wall);
				}else if(ytemp == (y + Math.floor((ylen - 1) / 2))){
					setCell(xtemp, ytemp, wall);
				}else{
					setCell(xtemp, ytemp, floor);
				} //end if
			} //end for
		} //end for
	} //end if
	return true;
} //end function

function surroundCorridors(){
	for(var y=0;y<size;y++){
		for(var x=0;x<size;x++){
			if(getCell(x,y)==tileDirtFloor||getCell(x,y)==tileCorridor||getCell(x,y)==tileDoor){
				if(x>0            ){if(getCell(x-1,y)==tileUnused)setCell(x-1,y,tileDirtWall);} //to the left
				if(x<size         ){if(getCell(x+1,y)==tileUnused)setCell(x+1,y,tileDirtWall);} //to the right
				if(y>0            ){if(getCell(x,y-1)==tileUnused)setCell(x,y-1,tileDirtWall);} //top
				if(y<size         ){if(getCell(x,y+1)==tileUnused)setCell(x,y+1,tileDirtWall);} //bottom
				if(x>0&&y>0       ){if(getCell(x-1,y-1)==tileUnused)setCell(x-1,y-1,tileDirtWall);} //topleft
				if(x<size&&y<size ){if(getCell(x+1,y+1)==tileUnused)setCell(x+1,y+1,tileDirtWall);} //bottomright
				if(x>0&&y<size    ){if(getCell(x-1,y+1)==tileUnused)setCell(x-1,y+1,tileDirtWall);} //bottomleft
				if(x<size&&y>0    ){if(getCell(x+1,y-1)==tileUnused)setCell(x+1,y-1,tileDirtWall);} //topright
			} //end if
			if(x>0   ){if(getCell(x,y)==tileDoor&&getCell(x-1,y)==tileCorridor)setCell(x,y,tileDirtFloor);}
			if(x<size){if(getCell(x,y)==tileDoor&&getCell(x+1,y)==tileCorridor)setCell(x,y,tileDirtFloor);}
			if(y>0   ){if(getCell(x,y)==tileDoor&&getCell(x,y-1)==tileCorridor)setCell(x,y,tileDirtFloor);}
			if(y<size){if(getCell(x,y)==tileDoor&&getCell(x,y+1)==tileCorridor)setCell(x,y,tileDirtFloor);}
		} //end for
	} //end for
} //end function

function drawCell(x,y){
	var cell=document.getElementById('x'+x+'y'+y);
	var cellType=getCell(x,y);
	var r,g,b;
	if(cellType==tileUnused){
		if(map[x][y].visible==true){ //visible objects
			r=22;g=22;b=22; 
		}else{                       //unseen objects
			r=11;g=11;b=11; 
		} //end if
	}else if(cellType==tileDirtWall){
		if(map[x][y].visible==true){ //visible objects
			r=0;g=0;b=0; 
		}else{                       //unseen objects
			r=0;g=0;b=0; 
		} //end if
	}else if(cellType==tileDirtFloor){
		if(map[x][y].visible==true){ //visible objects
			r=r1;g=g1;b=b1;
		}else{                       //unseen objects
			r=Math.floor(r1*.5);g=Math.floor(g1*.5);b=Math.floor(b1*.5);
		} //end if
	}else if(cellType==tileCorridor){
		if(map[x][y].visible==true){ //visible objects
			r=r1;g=g1;b=b1;
		}else{                       //unseen objects
			r=Math.floor(r1*.5);g=Math.floor(g1*.5);b=Math.floor(b1*.5);
		} //end if
	}else if(cellType==tileDoor){
		if(map[x][y].visible==true){ //visible objects
			r=Math.ceil(r1*1.5);g=Math.ceil(g1*1.5);b=Math.ceil(b1*1.5);
		}else{                       //unseen objects
			r=Math.ceil(r1*.9);g=Math.ceil(g1*.9);b=Math.ceil(b1*.9);
		} //end if
	}else if(cellType==tileUpStairs){
		if(map[x][y].visible==true){ //visible objects
			r=0;g=255;b=33;
		}else{                       //unseen objects
			r=0;g=155;b=15;
		} //end if
	}else if(cellType==tileDownStairs){
		if(map[x][y].visible==true){ //visible objects
			r=255;g=0;b=0;
		}else{                       //unseen objects
			r=155;g=0;b=0;
		} //end if
	} //end if
	cell.style.backgroundColor="rgb("+r+","+g+","+b+")";
} //end if

function generateMap_BSP(size, inobj){
	if(inobj < 1){objects = 10;}else{objects = inobj;}
	/******************************************************************************\
		And now the code of the random-map-generation-algorithm begins!
	\******************************************************************************/
	//start with making a room in the middle, which we can start building upon
	makeRoom(Math.floor(size / 2), Math.floor(size / 2), 8, 6, getRand(0,3));
	var currentFeatures = 1; //+1 for the first room we just made
	for (var countingTries = 0; countingTries < 1000; countingTries++){
		//check if we've reached our quota
		if(currentFeatures == objects){
			break;
		} //end if
		//start with a random wall
		var newx = 0;
		var xmod = 0;
		var newy = 0;
		var ymod = 0;
		var validTileDirection = _none;
		//1000 chances to find a suitable object (room or corridor)..
		for (var testing = 0; testing < 1000; testing++){
			newx = getRand(1, size - 1);
			newy = getRand(1, size - 1);
			validTileDirection = -1;
			if(getCell(newx, newy) == tileDirtWall || getCell(newx, newy) == tileCorridor){
				//check if we can reach the place
				if(getCell(newx, newy + 1) == tileDirtFloor || getCell(newx, newy + 1) == tileCorridor){
					validTileDirection = _south;
					xmod = 0;
					ymod = -1;
				}else if(getCell(newx - 1, newy) == tileDirtFloor || getCell(newx - 1, newy) == tileCorridor){
					validTileDirection = _west;
					xmod = +1;
					ymod = 0;
				}else if (getCell(newx, newy - 1) == tileDirtFloor || getCell(newx, newy - 1) == tileCorridor){
					validTileDirection = _north;
					xmod = 0;
					ymod = +1;
				}else if(getCell(newx + 1, newy) == tileDirtFloor || getCell(newx + 1, newy) == tileCorridor){
					validTileDirection = _east;
					xmod = -1;
					ymod = 0;
				} //end if
				//check that we haven't got another door nearby, so we won't get alot of openings besides
				//each other
				if(validTileDirection!=_none){
					if(getCell(newx, newy + 1) == tileDoor){ //north
						validTileDirection = _none;
					}else if(getCell(newx - 1, newy) == tileDoor){ //east
						validTileDirection = _none;
					}else if(getCell(newx, newy - 1) == tileDoor){ //south
						validTileDirection = _none;
					}else if(getCell(newx + 1, newy) == tileDoor){ //west
						validTileDirection = _none;
					} //end if
				} //end if
				//if we can, jump out of the loop and continue with the rest
				if (validTileDirection!=_none)break;
			} //end if
		} //end for
		if (validTileDirection!=_none){
			//choose what to build now at our newly found place, and at what direction
			var feature = getRand(0, 100);
			if(feature <= chanceRoom){ 
				if(makeRoom((newx + xmod), (newy + ymod), roomSizeMin+Math.floor(Math.random()*(roomSizeMax-roomSizeMin)), roomSizeMin+Math.floor(Math.random()*roomSizeMax-roomSizeMin), validTileDirection)){
					//a new room
					currentFeatures++; //add to our quota
					//then we mark the wall opening with a door
					setCell(newx, newy, tileDoor);
					//clean up infront of the door so we can reach it
					setCell((newx + xmod), (newy + ymod), tileDirtFloor);
				} //end if
			}else if(feature >= chanceRoom){ //new corridor
				if (makeCorridor((newx + xmod), (newy + ymod), corridorSizeMin+Math.floor(Math.random()*(corridorSizeMax-corridorSizeMin)), validTileDirection)){
					//same thing here, add to the quota and a door
					currentFeatures++;
					setCell(newx, newy, tileDoor);	
				} //end if
			} //end if
		} //end if
	} //end for
	/*******************************************************************************
		All done with the building, let's finish this one off
	*******************************************************************************/
	//sprinkle out the bonusstuff (stairs, chests etc.) over the map
	var newx = 0;
	var newy = 0;
	var ways = 0; //from how many directions we can reach the random spot from
	var state = 0; //the state the loop is in, start with the stairs
	while(state != 10){
		for(var testing = 0; testing < 1000; testing++){
			newx = getRand(1, size - 1);
			newy = getRand(1, size - 2); //cheap bugfix, pulls down newy to 0<y<24, from 0<y<25
			ways = 4; //the lower the better
			//check if we can reach the spot
			if(getCell(newx, newy + 1) == tileDirtFloor || getCell(newx, newy + 1) == tileCorridor){
				//north
				if(getCell(newx, newy + 1) != tileDoor)ways--;
			} //end if
			if(getCell(newx - 1, newy) == tileDirtFloor || getCell(newx - 1, newy) == tileCorridor){
				//east
				if(getCell(newx - 1, newy) != tileDoor)ways--;
			} //end if
			if(getCell(newx, newy - 1) == tileDirtFloor || getCell(newx, newy - 1) == tileCorridor){
				//south
				if(getCell(newx, newy - 1) != tileDoor)ways--;
			} //end if
			if(getCell(newx + 1, newy) == tileDirtFloor || getCell(newx + 1, newy) == tileCorridor){
				//west
				if(getCell(newx + 1, newy) != tileDoor)ways--;
			} //end if
			////console.log("ways: " + ways);
			if(state == 0){
				if(ways == 0){ //place upstairs
					setCell(newx, newy, tileUpStairs);
					cx=newx;cy=newy;
					state = 1;
					break;
				} //end if
			}else if(state == 1){
				if(ways == 0){ //place downstairs
					setCell(newx, newy, tileDownStairs);
					state = 10;
					break;
				} //end if
			} //end if
		} //end for
	} //end while
	//wrap the corridors with walls
	surroundCorridors();
	//all done with the map generation, tell the user about it and finish
	return true;
} //end function