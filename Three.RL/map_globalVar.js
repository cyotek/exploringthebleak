/* Main Mapping Variables and declarations */
var tileUnused     = 0;
var tileDirtWall   = 1;
var tileDirtFloor  = 2;
var tileCorridor   = 3;
var tileDoor       = 4;
var tileUpStairs   = 5;
var tileDownStairs = 6;

var size = 41,cx,cy;
var map = new Array(size);

/* Set the map array */
for (var i=0;i<=size;i++){
	map[i] = new Array(size);
} //end for

/* Initialize the Map Array to Zeros */
for (i=0;i<=size;i++){
	for(j=0;j<=size;j++){
		map[i][j]={
			type:0
		}; //end map[i][j]
	} //end for
} //end for

/* variables for BSP */
var chanceRoom = 75;
var chanceCorridor = 30;
var chanceCorridorBend = 80;
var corridorSizeMin = 3;
var corridorSizeMax = 7;
var roomSizeMin = 5;
var roomSizeMax = 7;
var objects = 40;

/* Place the Player */
function placePlayer(){
	do{
		cx=1+Math.floor(Math.random()*size);
		cy=1+Math.floor(Math.random()*size);
	}while(map[cx][cy].type!=tileDirtFloor);
} //end function