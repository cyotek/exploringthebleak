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
/* Calls the two main functions */
function drawMap(){
	init();
	animate();
}//end function

/* Main animation loop */
function animate() {
	requestAnimationFrame( animate );
	adjustCamera();
	pointLight.position.z=10*cy;
	pointLight.position.x=10*cx;
	renderer.render( scene, camera );
	stats.update();
} //end function

/* Make sure camera is correct */
function adjustCamera(){
	camera.position.set(10*cx,200,10*cy);
} //end function

/* Adjust drawing information on window resize */
function onWindowResize() {
	windowHalfX = window.innerWidth / 2;
	windowHalfY = window.innerHeight / 2;
	camera.aspect = window.innerWidth / window.innerHeight;
	camera.updateProjectionMatrix();
	renderer.setSize( window.innerWidth, window.innerHeight );
} //end function

/* Main initialization function to prepare THREE.js scene */
function init() {
	container = document.createElement( 'div' );
	document.body.appendChild( container );
	scene = new THREE.Scene();
	for(var i=0;i<=size;i++){
		for(var j=0;j<=size;j++){
			//if(Math.abs(i-cx)<=4 && Math.abs(j-cy)<=4){
				if(map[i][j].type==tileDirtWall){
					var randomType = Math.floor(Math.random()*10);
					if(randomType==0){      randomType=textureCobble2;
					}else if(randomType==1){randomType=textureCobble3;
					}else if(randomType==2){randomType=textureCobble4;
					}else if(randomType==3){randomType=textureCobble5;
					}else{                  randomType=textureCobble;
					} //end if
					addBlockCustomAllExceptOne(10*i,150,10*j,10,10,10,textureDirt,randomType,4) //4 is top (not wall)
				}else if(map[i][j].type==tileDoor){
					/* Draw the floor */
					addPlane(10*i,145,10*j,10,10,textureDirt);
					/* Find which axis the door resides on, and draw */
					if(map[i-1][j].type==tileDirtWall && map[i+1][j].type==tileDirtWall && map[i][j+1].type==tileDirtFloor && map[i][j-1].type==tileDirtFloor){
						savBlock(10,10,2,textureWood);
					}else{
						savBlock(2,10,10,textureWood);
					} //end if
					runBlock(10*i,150,10*j);
				}else if(map[i][j].type==tileDirtFloor){
					/* draw the floor */
					savPlane(10,10,textureDirt);
					runPlane(10*i,145,10*j);
					/* now see if it's populated with flora */
					var randomType = Math.floor(Math.random()*10);
					if(randomType==0){      repPlaneT(textureDecal,true);
					}else if(randomType==1){repPlaneT(textureDecal2,true);
					}else if(randomType==2){repPlaneT(textureDecal3,true);
					}else{randomType=0;
					} //end if
					if(randomType!=0){
						runPlane(10*i,146,10*j);
					} //end if
				}else if(map[i][j].type==tileDownStairs){
					/* Transparent Overlay to Darken */
					addTPlane(10*i+10,135,10*j,12,12,darkOverlay);
					/* floor */
					addPlane(10*i+10,135,10*j,12,12,textureDirt);
					/* walls */
					savBlock(10,10,1,textureCobble);
					runBlock(10*i   ,139,10*j-5); //front1
					runBlock(10*i+10,139,10*j-5); //front2
					runBlock(10*i   ,139,10*j+5); //back1
					runBlock(10*i+10,139,10*j+5); //back2
					/* stairs */
					savBlock(1,1,10,textureWood);
					for(var u=0;u<10;u++){
						runBlock(10*i-5+u,144-u,10*j);
					} //end for
				}else if(map[i][j].type==tileUpStairs){
					/* floor */
					addPlane(10*i,145,10*j,10,10,textureDirt);
					/* stairs */
					savBlock(1,1,10,textureWood2);
					for(var u=0;u<10;u++){
						runBlock(10*i-5+u,154-u,10*j);
					} //end for
				}else if(map[i][j].type==tileUnused){
					addPlane(10*i,155,10*j,10,10,textureDirt);
				} //end if
			//} //end if
		} //end for
	} //end for
	/* draw the player */
	var text="@";
	var textMaterial = new THREE.MeshPhongMaterial({color:0x052205, ambient: 0x226622,diffuse: 0x053305,specular: 0x054405,shininess:12});
	var text3d = new THREE.TextGeometry(text,{
		size:5,
		height:10,
		curveSegments:2,
		font:"helvetiker"
	});
	text=new THREE.Mesh(text3d,textMaterial);
	text.position.set(10*cx-3,142,10*cy+2);
	text.rotation.x=-1.5;
	scene.add(text);
	playerObject=text;
	/* Set up the Camera */
	adjustCamera();
	camera.useTarget=false;
	camera.rotation.x=-1.5;
	/* Lighting */
	//ambient light to darken scene
	var light = new THREE.AmbientLight( 0x000000 ); //0x668866
	scene.add( light );
	//spot light to cast shadows on side of walls
	var spotLight = new THREE.SpotLight( 0x776677 );
	spotLight.position.set( 100, 800, 100 );
	spotLight.castShadow = true; 
	spotLight.shadowMapWidth = 1024;
	spotLight.shadowMapHeight = 1024; 
	spotLight.shadowCameraNear = 500;
	spotLight.shadowCameraFar = 4000; 
	spotLight.shadowCameraFov = 30;  
	scene.add( spotLight );
	//point light to illuminate player
	pointLight.position.x = cx*10;
	pointLight.position.y = 156;
	pointLight.position.z = cy*10;
	scene.add(pointLight);
	/* render everything */
	renderer = new THREE.WebGLRenderer();
	renderer.setSize( window.innerWidth, window.innerHeight );
	container.appendChild( renderer.domElement );
	stats = new Stats();
	stats.domElement.style.position = 'absolute';
	stats.domElement.style.top = '0px';
	container.appendChild( stats.domElement );
	window.addEventListener( 'resize', onWindowResize, false );
} //end function