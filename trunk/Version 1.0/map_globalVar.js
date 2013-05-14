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

/* Main Mapping Variables and declarations */
var tileUnused     = 0;
var tileDirtWall   = 1;
var tileDirtFloor  = 2;
var tileCorridor   = 3;
var tileDoor       = 4;
var tileUpStairs   = 5;
var tileDownStairs = 6;

var size = 41,cx,cy,px,py;
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
var chanceRoom = 70;
var chanceCorridor = 30;
var objects = 0;

/* Place the Player */
function placePlayer(){
	do{
		cx=1+Math.floor(Math.random()*size);
		cy=1+Math.floor(Math.random()*size);
	}while(map[cx][cy].type!=tileDirtFloor);
} //end function

/* dimensionalize variables */
var container, stats;
var camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 1, 1000 );
var textureDirt  = THREE.ImageUtils.loadTexture('img-highRes/textureDirt.png');
var textureDirtNormal = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_NORM.bmp');
var textureDirtBump = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_DISP.bmp');
var textureDirtSpecular = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_SPEC.bmp');
var textureCobble = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone.png');
var textureCobbleNormal = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_NORM.bmp');
var textureCobbleBump = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_DISP.bmp');
var textureCobbleSpecular = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_SPEC.bmp');
var textureWater  = THREE.ImageUtils.loadTexture('img-highRes/textureWater.png');
var textureMoss   = THREE.ImageUtils.loadTexture('img-highRes/textureMoss.png');
var textureDoor   = THREE.ImageUtils.loadTexture('img-highRes/textureDoor.png');
var textureLava   = THREE.ImageUtils.loadTexture('img-highRes/textureLavaCalm.png');
var textureDecal  = THREE.ImageUtils.loadTexture('img-highRes/decalStones1.png');
var textureDecal2 = THREE.ImageUtils.loadTexture('img-highRes/decalStones2.png');
var textureDecal3 = THREE.ImageUtils.loadTexture('img-highRes/decalStones3.png');
var textureWood   = THREE.ImageUtils.loadTexture('img-highRes/textureWood.png');
var textureWood2  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodDark.png');
var darkOverlay   = THREE.ImageUtils.loadTexture('img-highRes/textureStairsOverlay.png');
var pointLight = new THREE.PointLight(0xFFFFFF,10,50);
var stairsBuilt=0;
var scene, renderer;
var cube, plane, playerObject;
var targetRotation = 0;
var targetRotationOnMouseDown = 0;
var mouseX = 0;
var mouseXOnMouseDown = 0;
var windowHalfX = window.innerWidth / 2;
var windowHalfY = window.innerHeight / 2;

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
var map2 = new Array(size);

/* Set the map array */
for (var i=0;i<=size;i++){
	map[i] = new Array(size);
	map2[i] = new Array(size);
} //end for

/* Initialize the Map Array to Zeros */
for (i=0;i<=size;i++){
	for(j=0;j<=size;j++){
		map[i][j]={
			type:0
		}; //end map[i][j]
		map2[i][j]={
			type:0
		}; //end map2[i][j]
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