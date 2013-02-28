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
function startGame(){
	curState='Play';
	document.getElementById("snow").innerHTML=null; //clear snow
	context.fillStyle='black';context.fillRect(1,1,canvasWidth,canvasHeight);
	/* draw the map */
	initializeMap();
	generateMap();
	drawMap();
	//drawMenu();
} //end startGame()
function drawGameOver(){
	curState='GameOver';
	/* Initialize dimensions and locations for menu */
	var xNum,yNum,classNum,classSize=(window.innerWidth>window.innerHeight?window.innerHeight-40:window.innerWidth-40)/(classDB.max+3)*2;
	var stageWidth=218,stageHeight=105;
	var paintLayer=new Kinetic.Layer();
	var colMod = -1; //used to flip columns
	/* Create a new character */
	var intro = document.getElementById("intro")
	intro.style.backgroundColor='#222';
	intro.style.border="2px solid";
	intro.style.left=window.innerWidth/2-stageWidth/2+"px";
	intro.style.width=stageWidth+"px";
	intro.style.height=stageHeight+"px";
	intro.style.top=window.innerHeight/2-50+"px";
	intro.innerHTML="<div id='paintPanel' style='position:absolute;'></div>";
	var stage = new Kinetic.Stage({container:'paintPanel',width:stageWidth,height:stageHeight});
	var gameOver = new Kinetic.Text({
		x:0,
		y:0,
		width:stageWidth,
		height:stageHeight,
		fill:'#222',
		align:'center',
		stroke:'black',
		strokeWidth:1,
		padding:40,
		text:"Game Over!",
		fontSize:18,
		fontFamily: 'Calibri',
		textFill: '#777'
	}); //end readyBtn
	paintLayer.add(gameOver);
	stage.add(paintLayer);
} //end drawGameOver()
function drawPickName(){
	curState='PickName';
	/* Initialize dimensions and locations for menu */
	var xNum,yNum,classNum,classSize=(window.innerWidth>window.innerHeight?window.innerHeight-40:window.innerWidth-40)/(classDB.max+3)*2;
	var stageWidth=218,stageTitleBarHeight=25;
	var classLayer=new Kinetic.Layer();
	var colMod = -1; //used to flip columns
	/* Create a new character */
	var intro = document.getElementById("intro")
	intro.style.backgroundColor='#222';
	intro.style.border="2px solid";
	intro.style.left=window.innerWidth/2-stageWidth/2+"px";
	intro.style.width=stageWidth+"px";
	intro.style.height=105+"px";
	intro.style.top=window.innerHeight/2-50+"px";
	intro.innerHTML="<div id='paintPanel' style='position:absolute;'></div>"+
					"<div id='input' style='position:absolute;left:5px;top:35px;'>"+
					"<input type='text' width='500' value='John Hancock' id='playerName'></div>";
	var playerNameBox = document.getElementById("playerName")
	playerNameBox.style.width=stageWidth-15+'px';
	var stage = new Kinetic.Stage({container:'paintPanel',width:stageWidth,height:125});
	var titleBar = new Kinetic.Text({
		x:0,
		y:0,
		width:stageWidth,
		height:stageTitleBarHeight,
		text:"Pick Your Name",
		fontSize:14,
		padding:4,
		fontFamily:'Calibri',
		align:'center',
		lineHeight:1,
		fill:'#555',
		stroke:'black',
		strokeWidth:1,
		textFill:'#FFF',
		shadow:{
			color:'black',
			blur:10,
			offset:[0,10],
			opacity:0.5
		} //end shadow
	}); //end titleBarTxt
	var readyBtn = new Kinetic.Text({
		x:5,
		y:65,
		width:stageWidth-10,
		height:35,
		fill:'#777',
		align:'center',
		stroke:'black',
		strokeWidth:1,
		padding:5,
		text:"Play Game!",
		fontSize:18,
		fontFamily: 'Calibri',
		textFill: 'black'
	}); //end readyBtn
	readyBtn.on("mouseover",function(){readyBtn.setFill('#AAA');classLayer.draw();});
	readyBtn.on("mouseout" ,function(){readyBtn.setFill('#777');classLayer.draw();});
	readyBtn.on("mousedown",function(){readyBtn.setFill('#600');classLayer.draw();clearIntro();});
	readyBtn.on("mouseup"  ,function(){readyBtn.setFill('#AAA');classLayer.draw();});
	var btns = new Kinetic.Group();
	classLayer.add(readyBtn);
	classLayer.add(titleBar);
	stage.add(classLayer);
} //end drawPickName()
function drawPickRace(){
	curState='PickRace';
	/* Initialize dimensions and locations for menu */
	var xNum,yNum,raceNum,raceSize=(window.innerWidth>window.innerHeight?window.innerHeight-40:window.innerWidth-40)/(raceDB.max+3)*2;
	var stageWidth=raceSize*2+475,stageTitleBarHeight=25;
	var raceLayer=new Kinetic.Layer();
	var colMod = -1; //used to flip columns
	/* Create a new character */
	var intro = document.getElementById("intro")
	intro.style.backgroundColor='#222';
	intro.style.border="2px solid";
	intro.style.left=window.innerWidth/2-stageWidth/2+"px";intro.style.top=25+"px";
	intro.style.width=stageWidth+"px";
	intro.style.height=window.innerHeight-75+"px";
	intro.innerHTML="<div id='paintPanel' style='position:absolute;'></div>";
	var stage = new Kinetic.Stage({container:'paintPanel',width:stageWidth,height:canvasHeight-50});
	var raceNameText = new Kinetic.Text({
		x:10,
		y:stageTitleBarHeight+10,
		width:400,
		text: raceDB.name[0],
		fontSize:24,
		fontFamily: 'Calibri',
		textFill: '#FFF'
	}); //end raceNameText
	var raceDescription = new Kinetic.Text({
		x:10,
		y:stageTitleBarHeight+40,
		width:400,
		text: raceDB.description[0],
		fontSize: 14,
		fontFamily: 'Calibri',
		textFill: '#777'
	}); //end raceDescription
	var titleBar = new Kinetic.Text({
		x:0,
		y:0,
		width:stageWidth,
		height:stageTitleBarHeight,
		text:"Choose Your race",
		fontSize:14,
		padding:4,
		fontFamily:'Calibri',
		align:'center',
		lineHeight:1,
		fill:'#555',
		stroke:'black',
		strokeWidth:1,
		textFill:'#FFF',
		shadow:{
			color:'black',
			blur:10,
			offset:[0,10],
			opacity:0.5
		} //end shadow
	}); //end titleBarTxt
	var readyBtn    = new Kinetic.Group();
	var readyBtnRct = new Kinetic.Rect({
		x:stageWidth-50,
		y:stageTitleBarHeight+5,
		width:45,
		height:window.innerHeight-85-stageTitleBarHeight,
		fill:'#777',
		stroke: 'black',
		strokeWidth: 1
	}); //end readyBtn
	var readyBtnTxt = new Kinetic.Text({
		x:stageWidth-40,
		y:stageTitleBarHeight+window.innerHeight/2,
		text:"Next",
		fontSize:18,
		fontFamily: 'Calibri',
		textFill: 'black',
		rotation:3*Math.PI/2
	}); //end readyTxt
	readyBtn.add(readyBtnRct);readyBtn.add(readyBtnTxt);
	readyBtn.on("mouseover",function(){readyBtnRct.setFill('#AAA');raceLayer.draw();});
	readyBtn.on("mouseout" ,function(){readyBtnRct.setFill('#777');raceLayer.draw();});
	readyBtn.on("mousedown",function(){readyBtnRct.setFill('#600');raceLayer.draw();drawPickName();});
	readyBtn.on("mouseup"  ,function(){readyBtnRct.setFill('#AAA');raceLayer.draw();});
	var btns = new Kinetic.Group();
	for (raceNum=0;raceNum<raceDB.max;raceNum++){
		(function(){ //anonymous function forces current scope for arrays in Kinetic
		var i=raceNum;
		if (i%7==0)colMod++;
		charPanel = new Kinetic.Rect({
			x:5+raceSize*colMod+5*colMod,
			y:stageTitleBarHeight+10+raceSize*i+5*i-7*colMod*raceSize-colMod*35,
			width:raceSize,
			height:raceSize,
			fill:'#333',
			stroke: 'black',
			strokeWidth: 1
		}); //end charPanel
		charrace = new Kinetic.Image({
			x:5+raceSize*colMod+5*colMod,
			y:stageTitleBarHeight+10+raceSize*i+5*i-7*colMod*raceSize-colMod*35,
			image:findRaceImg(i),
			crop:{
				x:raceDB.tilePos.x[i]*32,
				y:raceDB.tilePos.y[i]*32,
				width:32,
				height:32
			}, //end crop
			width:raceSize,
			height:raceSize
		}); //end charrace
		charrace.on("mouseover",function(event){
			event.shape.setFill('#555');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			raceLayer.draw();
		}); //end mouseover
		charrace.on("mouseout" ,function(event){
			event.shape.setFill('#333');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			raceLayer.draw();
		}); //end mouseout
		charrace.on("mouseup"  ,function(event){
			event.shape.setFill('#555');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			raceLayer.draw();
		}); //end mouseup
		charrace.on("mousedown",function(event){
			event.shape.setFill('#600');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			raceNameText.setText(raceDB.name[i]);
			player.prace=             raceDB.name[i];
			player.tilePos.x=         raceDB.tilePos.x[i];
			player.tilePos.y=         raceDB.tilePos.y[i];
			player.stats.strength=    raceDB.stats.strength[i];player.stats.strengthMax=player.stats.strength+5;
			player.stats.dexterity=   raceDB.stats.dexterity[i];player.stats.dexterityMax=player.stats.dexterity+5;
			player.stats.constitution=raceDB.stats.constitution[i];player.stats.constitutionMax=player.stats.constitution+5;
			player.chp=player.stats.constitution*10;player.mhp=player.chp;
			player.stats.intelligence=raceDB.stats.intelligence[i];player.stats.intelligenceMax=player.stats.intelligence+5;
			player.cwp=player.stats.intelligence*10;player.mwp=player.cwp;
			player.stats.wisdom=      raceDB.stats.wisdom[i];player.stats.wisdomMax=player.stats.wisdom+5;
			player.stats.charisma=    raceDB.stats.charisma[i];player.stats.charismaMax=player.stats.charisma+5;
			player.stats.luck=        raceDB.stats.luck[i];player.stats.luckMax=player.stats.luck+5;
			player.ac.natural=        raceDB.naturalAC[i];
			player.height=            raceDB.height[i];
			player.weight=            raceDB.weight[i];
			raceDescription.setText(  raceDB.description[i]);
			raceLayer.draw();
		}); //end mousedown
		btns.add(charPanel);
		raceLayer.add(charPanel);raceLayer.add(charrace);
		})();
	} //end if
	colMod++;
	raceDescription.setX(5+raceSize*colMod+5*colMod);
	raceNameText.setX(5+raceSize*colMod+5*colMod);
	raceLayer.add(raceNameText);
	raceLayer.add(raceDescription);
	raceLayer.add(readyBtn);
	raceLayer.add(titleBar);
	stage.add(raceLayer);
} //end drawPickrace()
function findRaceImg(i){
	if (raceDB.tileSet[i]=='classm'){ //tileset 'classm'
		return monster01Img;
	} else if (raceDB.tileSet[i]=='monster1'){ //tileset monster1
		return monster05Img;
	} else if (raceDB.tileSet[i]=='monster2'){ //tileset monster2
		return monster06Img;
	} else if (raceDB.tileSet[i]=='monster3'){ //tileset monster3
		return monster07Img;
	} else if (raceDB.tileSet[i]=='monster6'){ //tileset monster6
		return monster10Img;
	} else {
		return null; //error!!!
	} //end if
} //end function
function drawPickClass(){
	curState='PickClass';
	/* Initialize dimensions and locations for menu */
	var xNum,yNum,classNum,classSize=(window.innerWidth>window.innerHeight?window.innerHeight-40:window.innerWidth-40)/(classDB.max+3)*2;
	var stageWidth=classSize*2+475,stageTitleBarHeight=25;
	var classLayer=new Kinetic.Layer();
	var colMod = -1; //used to flip columns
	/* Create a new character */
	var intro = document.getElementById("intro")
	intro.style.backgroundColor='#222';
	intro.style.border="2px solid";
	intro.style.left=window.innerWidth/2-stageWidth/2+"px";intro.style.top=25+"px";
	intro.style.width=stageWidth+"px";
	intro.style.height=window.innerHeight-75+"px";
	intro.innerHTML="<div id='paintPanel' style='position:absolute;'></div>";
	var stage = new Kinetic.Stage({container:'paintPanel',width:stageWidth,height:canvasHeight-50});
	var classNameText = new Kinetic.Text({
		x:10,
		y:stageTitleBarHeight+10,
		width:400,
		text: classDB.name[0],
		fontSize:24,
		fontFamily: 'Calibri',
		textFill: '#FFF'
	}); //end classNameText
	var classDescription = new Kinetic.Text({
		x:10,
		y:stageTitleBarHeight+40,
		width:400,
		text: classDB.description[0],
		fontSize: 14,
		fontFamily: 'Calibri',
		textFill: '#777'
	}); //end classDescription
	var titleBar = new Kinetic.Text({
		x:0,
		y:0,
		width:stageWidth,
		height:stageTitleBarHeight,
		text:"Choose Your Class",
		fontSize:14,
		padding:4,
		fontFamily:'Calibri',
		align:'center',
		lineHeight:1,
		fill:'#555',
		stroke:'black',
		strokeWidth:1,
		textFill:'#FFF',
		shadow:{
			color:'black',
			blur:10,
			offset:[0,10],
			opacity:0.5
		} //end shadow
	}); //end titleBarTxt
	var readyBtn    = new Kinetic.Group();
	var readyBtnRct = new Kinetic.Rect({
		x:stageWidth-50,
		y:stageTitleBarHeight+5,
		width:45,
		height:window.innerHeight-85-stageTitleBarHeight,
		fill:'#777',
		stroke: 'black',
		strokeWidth: 1
	}); //end readyBtn
	var readyBtnTxt = new Kinetic.Text({
		x:stageWidth-40,
		y:stageTitleBarHeight+window.innerHeight/2,
		text:"Next",
		fontSize:18,
		fontFamily: 'Calibri',
		textFill: 'black',
		rotation:3*Math.PI/2
	}); //end readyTxt
	readyBtn.add(readyBtnRct);readyBtn.add(readyBtnTxt);
	readyBtn.on("mouseover",function(){readyBtnRct.setFill('#AAA');classLayer.draw();});
	readyBtn.on("mouseout" ,function(){readyBtnRct.setFill('#777');classLayer.draw();});
	readyBtn.on("mousedown",function(){readyBtnRct.setFill('#600');classLayer.draw();drawPickRace();});
	readyBtn.on("mouseup"  ,function(){readyBtnRct.setFill('#AAA');classLayer.draw();});
	var btns = new Kinetic.Group();
	for (classNum=0;classNum<classDB.max;classNum++){
		(function(){ //anonymous function forces current scope for arrays in Kinetic
		var i=classNum;
		if (i%7==0)colMod++;
		charPanel = new Kinetic.Rect({
			x:5+classSize*colMod+5*colMod,
			y:stageTitleBarHeight+10+classSize*i+5*i-7*colMod*classSize-colMod*35,
			width:classSize,
			height:classSize,
			fill:'#333',
			stroke: 'black',
			strokeWidth: 1
		}); //end charPanel
		charClass = new Kinetic.Image({
			x:5+classSize*colMod+5*colMod,
			y:stageTitleBarHeight+10+classSize*i+5*i-7*colMod*classSize-colMod*35,
			image:classesImg,
			crop:{
				x:classDB.tilePos.x[i]*32,
				y:classDB.tilePos.y[i]*32,
				width:32,
				height:32
			}, //end crop
			width:classSize,
			height:classSize
		}); //end charClass
		charClass.on("mouseover",function(event){
			event.shape.setFill('#555');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			classLayer.draw();
		}); //end mouseover
		charClass.on("mouseout" ,function(event){
			event.shape.setFill('#333');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			classLayer.draw();
		}); //end mouseout
		charClass.on("mouseup"  ,function(event){
			event.shape.setFill('#555');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			classLayer.draw();
		}); //end mouseup
		charClass.on("mousedown",function(event){
			event.shape.setFill('#600');
			event.shape.setStroke('black');
			event.shape.setStrokeWidth(1);
			classNameText.setText(classDB.name[i]);player.pclass=classDB.name[i];
			classDescription.setText(classDB.description[i]);
			classLayer.draw();
		}); //end mousedown
		btns.add(charPanel);
		classLayer.add(charPanel);classLayer.add(charClass);
		})();
	} //end if
	colMod++;
	classDescription.setX(5+classSize*colMod+5*colMod);
	classNameText.setX(5+classSize*colMod+5*colMod);
	classLayer.add(classNameText);
	classLayer.add(classDescription);
	classLayer.add(readyBtn);
	classLayer.add(titleBar);
	stage.add(classLayer);
} //end drawPickClass()
function clearIntro(){
	var intro = document.getElementById("intro");
	intro.style.left=-1;intro.style.top=-1;intro.style.width=0;intro.style.height=0;
	intro.style.border=null;
	intro.innerHTML=null;
	startGame();
} //end function
function drawTitleScreen(){
	curState='Title';
	context.drawImage(titleScreen,1,1,canvasWidth,canvasHeight);
	var stage = new Kinetic.Stage({container:'intro',width:canvasWidth-50,height:canvasHeight-50});
	var btnLayer = new Kinetic.Layer();
	var gameVersion = new Kinetic.Text({
		x: canvasWidth/2-79-145,
		y: canvasHeight*.77,
		align: 'center',
		text: 'Version 1.0 Beta\nExploring The Bleak Copyright 2012 by Nathaniel Inman\nAll Rights Reserved',
		fontSize:12,
		fontFamily: 'Arial',
		textFill: '#222'
	}); //end version
	var btn1 = new Kinetic.Group();
	var btn1Text = new Kinetic.Text({
		x: canvasWidth/2-79,
		y: canvasHeight/2-7,
		text: 'Start Game',
		fontSize: 14,
		fontFamily: 'Calibri',
		textFill: 'black'
	}); //end btn1Text
	var btn1Rect = new Kinetic.Rect({
		x: canvasWidth/2-85,
		y: canvasHeight/2-25,
		width:100,
		height:50,
		fill: {start:{x:50,y:0,},end:{x:50,y:50,},colorStops:[0,'#333',0.5,'#555',1,'#444']},
		stroke: 'black',
		strokeWidth: 1,
		cornerRadius:10
	}); //end Btn1Rect
	btn1.add(btn1Rect);btn1.add(btn1Text);
	btnLayer.add(btn1);btnLayer.add(gameVersion);
	btn1.on("mouseover",function(){btn1Rect.setFill({start:{x:50,y:0,},end:{x:50,y:50,},colorStops:[0,'#555',0.5,'#888',1,'#666']});btnLayer.draw();});
	btn1.on("mouseout" ,function(){btn1Rect.setFill({start:{x:50,y:0,},end:{x:50,y:50,},colorStops:[0,'#333',0.5,'#555',1,'#444']});btnLayer.draw();});
	btn1.on("mouseup"  ,function(){btn1Rect.setFill({start:{x:50,y:0,},end:{x:50,y:50,},colorStops:[0,'#555',0.5,'#888',1,'#666']});btnLayer.draw();});
	btn1.on("mousedown",function(){btn1Rect.setFill({start:{x:50,y:0,},end:{x:50,y:50,},colorStops:[0,'#353',0.5,'#585',1,'#464']});btnLayer.draw();drawPickClass();});
	stage.add(btnLayer);
} //end drawTitle()
function init() {
	container = document.getElementById('snow');
	camera = new THREE.PerspectiveCamera( 75, SCREEN_WIDTH / SCREEN_HEIGHT, 1, 10000 );
	camera.position.z = 1000;
	scene = new THREE.Scene();
	scene.add(camera);	
	renderer = new THREE.CanvasRenderer();
	renderer.setSize(SCREEN_WIDTH, SCREEN_HEIGHT);
	var material = new THREE.ParticleBasicMaterial( { map: new THREE.Texture(particleImage) } );	
	for (var i = 0; i < 500; i++) {
		particle = new Particle3D( material);
		particle.position.x = Math.random() * 2000 - 1000;
		particle.position.y = Math.random() * 2000 - 1000;
		particle.position.z = Math.random() * 2000 - 1000;
		particle.scale.x = particle.scale.y =  1;
		scene.add( particle );
		particles.push(particle); 
	} //end for
	container.appendChild( renderer.domElement );
	setInterval( loop, 1000 / 60 );
	drawTitleScreen();
} //end init()
function loop() {
	for(var i = 0; i<particles.length; i++){
		var particle = particles[i]; 
		particle.updatePhysics(); 
		with(particle.position){
			if(y<-1000) y+=2000; 
			if(x>1000) x-=2000; 
			else if(x<-1000) x+=2000; 
			if(z>1000) z-=2000; 
			else if(z<-1000) z+=2000; 
		} //end with		
	} //end for
	camera.position.x += ( mouseX - camera.position.x ) * 0.05;
	camera.position.y += ( - mouseY - camera.position.y ) * 0.05;
	camera.lookAt(scene.position); 
	renderer.render( scene, camera );
} //end loop()
