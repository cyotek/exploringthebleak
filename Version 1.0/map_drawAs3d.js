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
	pointLight.position.set(playerObject.position.x,157,playerObject.position.z);
	camera.position.set(10*cx,175,10*cy+20);
	renderer.render( scene, camera );
	stats.update();
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
			//if(Math.abs(i-cx)<=3 && Math.abs(j-cy)<=3){
				if(map[i][j].type==tileDirtWall){
					addBlockCustomAllExceptOne(10*i,150,10*j,10,10,10,textureDirt,0,textureDirtBump,textureCobbleBump,4) //4 is top (not wall)
				}else if(map[i][j].type==tileDoor){
					/* Draw the floor */
					addPlane(10*i,145,10*j,10,10,textureDirt);
					/* Find which axis the door resides on, and draw */
					if(map[i-1][j].type==tileDirtWall && map[i+1][j].type==tileDirtWall && map[i][j+1].type==tileDirtFloor && map[i][j-1].type==tileDirtFloor){
						savBlock(10,10,2,textureDoor);
					}else{
						savBlock(2,10,10,textureDoor);
					} //end if
					runBlock(10*i,150,10*j);
				}else if(map[i][j].type==tileDirtFloor){
					addPlane(10*i,145,10*j,10,10,textureWood);
				}else if(map[i][j].type==tileCorridor){
					addPlane(10*i,145,10*j,10,10,textureWood);
				}else if(map[i][j].type==tileDownStairs){
					/* Transparent Overlay to Darken */
					//addTPlane(10*i+10,135,10*j,12,12,darkOverlay);
					/* floor */
					addPlane(10*i+10,135,10*j,12,12,textureDirt);
					/* walls */
					addBlockCustomAllExceptOne(10*i   ,139.99,10*j-5,10,10,1,textureDirt,0,textureDirtBump,textureCobbleBump,4) //front1
					addBlockCustomAllExceptOne(10*i+10,139.99,10*j-5,10,10,1,textureDirt,0,textureDirtBump,textureCobbleBump,4) //front2
					addBlockCustomAllExceptOne(10*i   ,139.99,10*j+5,10,10,1,textureDirt,0,textureDirtBump,textureCobbleBump,4) //back1
					addBlockCustomAllExceptOne(10*i+10,139.99,10*j+5,10,10,1,textureDirt,0,textureDirtBump,textureCobbleBump,4) //back2
					/* stairs */
					for(var u=0;u<10;u++){
						var rPlank=Math.floor(Math.random()*5);
						if(rPlank==0){
							addBlock(10*i-5+u,144-u,10*j,1,1,10,textureWoodPlank1)
						}else if(rPlank==1){
							addBlock(10*i-5+u,144-u,10*j,1,1,10,textureWoodPlank2)
						}else if(rPlank==2){
							addBlock(10*i-5+u,144-u,10*j,1,1,10,textureWoodPlank3)
						}else if(rPlank==3){
							addBlock(10*i-5+u,144-u,10*j,1,1,10,textureWoodPlank4)
						}else{
							addBlock(10*i-5+u,144-u,10*j,1,1,10,textureWoodPlank5)
						} //end if
					} //end for
				}else if(map[i][j].type==tileUpStairs){
					/* floor */
					addPlane(10*i,145,10*j,10,10,textureDirt);
					/* stairs */
					for(var u=0;u<10;u++){
						var rPlank=Math.floor(Math.random()*5);
						if(rPlank==0){
							addBlock(10*i-5+u,154-u,10*j,1,1,10,textureWoodPlank1)
						}else if(rPlank==1){
							addBlock(10*i-5+u,154-u,10*j,1,1,10,textureWoodPlank2)
						}else if(rPlank==2){
							addBlock(10*i-5+u,154-u,10*j,1,1,10,textureWoodPlank3)
						}else if(rPlank==3){
							addBlock(10*i-5+u,154-u,10*j,1,1,10,textureWoodPlank4)
						}else{
							addBlock(10*i-5+u,154-u,10*j,1,1,10,textureWoodPlank5)
						} //end if
					} //end for
				}else if(map[i][j].type==tileUnused){
					addPlane(10*i,155,10*j,10,10,textureWood);
				} //end if
				if(mobMap[i][j]!=0){
					text=mobs[mobMap[i][j]].symbol
					var textMaterial = new THREE.MeshPhongMaterial({color:0x220505, ambient: 0x662222,diffuse: 0x330605,specular: 0x440505,shininess:72});
					var text3d = new THREE.TextGeometry(text,{
						size:5,
						height:3,
						curveSegments:6,
						font:"helvetiker"
					});
					text=new THREE.Mesh(text3d,textMaterial);
					text.position.set(10*i-3,147,10*j+2);
					text.rotation.x=-0.75;
					scene.add(text);
				} //end if
			//} //end if
		} //end for
	} //end for
	/* draw the player */
	var text="@";
	var textMaterial = new THREE.MeshPhongMaterial({color:0x052205, ambient: 0x226622,diffuse: 0x053305,specular: 0x054405,shininess:12});
	var text3d = new THREE.TextGeometry(text,{
		size:5,
		height:3,
		curveSegments:6,
		font:"helvetiker"
	});
	text=new THREE.Mesh(text3d,textMaterial);
	text.position.set(10*cx-3,147,10*cy+2);
	text.rotation.x=-0.75;
	scene.add(text);
	
	playerObject=text;
	/* Set up the Camera */
	camera.useTarget=false;
	camera.rotation.x=-1.0;
	/* fog */
	//scene.fog = new THREE.FogExp2( 0x999999, 0.015 );
	/* Lighting */
	//ambient light to darken scene
	//var light = new THREE.AmbientLight( 0x000000 ); //0x668866
	//scene.add( light );
	//spot light to cast shadows on side of walls
	//var spotLight = new THREE.SpotLight( 0x332233 );
	//spotLight.position.set( 100, 800, 100 );
	//spotLight.castShadow = true; 
	//spotLight.shadowMapWidth = 1024;
	//spotLight.shadowMapHeight = 1024; 
	//spotLight.shadowCameraNear = 500;
	//spotLight.shadowCameraFar = 4000; 
	//spotLight.shadowCameraFov = 30;  
	//scene.add( spotLight );
	//point light to illuminate player
	//light1 = new THREE.PointLight( 0xff0040, 2, 50 );
	//			scene.add( light1 );
    pointLight = new THREE.PointLight(0xaa9955,5,30);
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