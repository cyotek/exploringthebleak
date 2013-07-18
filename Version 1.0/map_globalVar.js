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

/* Constants */
var tileUnused     = 0;
var tileDirtWall   = 1;
var tileDirtFloor  = 2;
var tileCorridor   = 3;
var tileDoor       = 4;
var tileUpStairs   = 5;
var tileDownStairs = 6;

/* Main Variables and Declarations */
var size = 41,cx,cy,px,py;
var map = new Array(size);
var map2 = new Array(size);
var mobMap = new Array(size);
var mob = {
	symbol:"b",
	location:{x:   0,y:  0},
	health:  {cur:10,max:10},
	damage:  {min: 1,max:5}
}; //end mob
var mobs = new Array(Math.floor(size/2));

/* Set the standard arrays */
for (var i=0;i<=size;i++){
	map[i] = new Array(size);
	map2[i] = new Array(size);
	mobMap[i] = new Array(size);
	if(i<=Math.floor(size/2)){ //make sure that the mob indexs don't exceed the boundary.
		mobs[i]=mob;
	} //end if
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
		mobMap[i][j]=0;
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

/* dimensionalize variables */
var container, stats;
var camera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 1, 1000 );
var tex_Particle    = THREE.ImageUtils.loadTexture('img-highRes/particle2.png');
var tex_Dirt_D      = THREE.ImageUtils.loadTexture('img-highRes/textureDirt.png');
var tex_Dirt_N      = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_NORM.png');
var tex_Dirt_S      = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_SPEC.png');
var tex_Dirt_B      = THREE.ImageUtils.loadTexture('img-highRes/textureDirt_DISP.png');
var tex_Dirt        = {diffuse:tex_Dirt_D,normal:tex_Dirt_N,specular:tex_Dirt_S,bump:tex_Dirt_B};
var tex_DirtC_D     = THREE.ImageUtils.loadTexture('img-highRes/textureDirtC.png');
var tex_DirtC_N     = THREE.ImageUtils.loadTexture('img-highRes/textureDirtC_NORM.png');
var tex_DirtC_S     = THREE.ImageUtils.loadTexture('img-highRes/textureDirtC_SPEC.png');
var tex_DirtC_B     = THREE.ImageUtils.loadTexture('img-highRes/textureDirtC_DISP.png');
var tex_DirtC       = {diffuse:tex_DirtC_D,normal:tex_DirtC_N,specular:tex_DirtC_S,bump:tex_DirtC_B};
var tex_Cobble_D    = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone.png');
var tex_Cobble_N    = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_NORM.png');
var tex_Cobble_S    = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_SPEC.png');
var tex_Cobble_B    = THREE.ImageUtils.loadTexture('img-highRes/textureCobblestone_DISP.png');
var tex_Cobble      = {diffuse:tex_Cobble_D,normal:tex_Cobble_N,specular:tex_Cobble_S,bump:tex_Cobble_B};
var tex_Door        = THREE.ImageUtils.loadTexture('img-highRes/doorDungeonWood.png');
var tex_Wood_D      = THREE.ImageUtils.loadTexture('img-highRes/textureWoodWall.png');
var tex_Wood_N      = THREE.ImageUtils.loadTexture('img-highRes/textureWoodWall_NORM.png');
var tex_Wood_S      = THREE.ImageUtils.loadTexture('img-highRes/textureWoodWall_SPEC.png');
var tex_Wood_B      = THREE.ImageUtils.loadTexture('img-highRes/textureWoodWall_DISP.png');
var tex_Wood        = {diffuse:tex_Wood_D,normal:tex_Wood_N,specular:tex_Wood_S,bump:tex_Wood_B};
var tex_WoodPlank1  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodPlank.png');
var tex_WoodPlank2  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodPlank2.png');
var tex_WoodPlank3  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodPlank3.png');
var tex_WoodPlank4  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodPlank4.png');
var tex_WoodPlank5  = THREE.ImageUtils.loadTexture('img-highRes/textureWoodPlank5.png');
var pointLight = new THREE.PointLight(0xFFFFFF,10,50);
var stairsBuilt=0;
var particleCount = 100,particles;
var scene, renderer, particleSystem;
var cube, plane, playerObject;
var targetRotation = 0;
var targetRotationOnMouseDown = 0;
var mouseX = 0;
var mouseXOnMouseDown = 0;
var windowHalfX = window.innerWidth / 2;
var windowHalfY = window.innerHeight / 2;