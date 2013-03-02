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
/* Document Dimensions */
	var xmlDoc; //variable name for the xml document handler that will hold mobiles & items before they're parsed
/* Graphics or ascii */
	var graphics=true;
	var redraw=false; //determines whether a panel will be flagged as a redrawn panel (drop item) so the save context doesn't go twice
/* Keep track of the current state */
	var curState='Title'; //start out on the title screen
/* Map Dimensions */
	var size=24; //map size
	var x,y,cx,cy; //map location variables (reused)
	var map,mobMap,itemMap,wallMap,visMap; //stores map data including floor/wall && mobmap for mob numbers and references
	var stairsDownX,stairsDownY,stairsUpX,stairsUpY; //stairs location on the map
	var initialized=0; //whether or not the ui has been initialized yet
/* snow specific variables */
	var SCREEN_WIDTH = window.innerWidth-25;     //used locally in the function only
	var SCREEN_HEIGHT = window.innerHeight-25;   //used locally in the function only
	var container;                            //holds the div id
	var particle;                             //the current selected particle of array particles[]
	var camera;                               //the 3d camera and position
	var scene;                                //the scene that everything is rendered upon
	var renderer;                             //three.js specific 3d renderer declaration
	var mouseX=5,mouseY=5;                        //variables used to determine mouse position
	var windowHalfX = window.innerWidth / 2;  //used locally in function only
	var windowHalfY = window.innerHeight / 2; //used locally in function only
	var particles = [];                       //contains an array of each particle used in the title screen
/* Art specific variables */
	var particleImage = new Image();particleImage.src = 'art/ParticleSmoke.png'; 
	var titleScreen = new Image();titleScreen.src='art/titleScreen.png';
	var classesImg = new Image();classesImg.src='art/dg_humans32.png';
	var playerImg = new Image();playerImg.src='art/Character Boy.png';
	var wall01Img = new Image();wall01Img.src='art/Dirt Block.png';
	var wall02Img = new Image();wall02Img.src='art/Lava Block.png';
	var wall03Img = new Image();wall03Img.src='art/Rock.png';
	var wall04Img = new Image();wall04Img.src='art/Stone Block Tall.png';
	var wall05Img = new Image();wall05Img.src='art/Tree Short.png';
	var wall06Img = new Image();wall06Img.src='art/Tree Tall.png';
	var wall07Img = new Image();wall07Img.src='art/Tree Ugly.png';
	var wall08Img = new Image();wall08Img.src='art/Water Block.png';
	var stairuImg = new Image();stairuImg.src='art/Ramp Up.png';
	var stairdImg = new Image();stairdImg.src='art/Ramp Down.png';
	var chestCImg = new Image();chestCImg.src='art/Chest Closed.png';
	var shadow01Img = new Image();shadow01Img.src='art/Shadow North.png';
	var shadow02Img = new Image();shadow02Img.src='art/Shadow Northwest.png';
	var shadow03Img = new Image();shadow03Img.src='art/Shadow West.png';
	var shadow04Img = new Image();shadow04Img.src='art/Shadow.png';
	var monster01Img = new Image();monster01Img.src='art/dg_classm32.png';
	var monster02Img = new Image();monster02Img.src='art/dg_dragon32.png';
	var monster03Img = new Image();monster03Img.src='art/dg_undead32.png';
	var monster04Img = new Image();monster04Img.src='art/dg_unique32.png';
	var monster05Img = new Image();monster05Img.src='art/dg_monster132.png';
	var monster06Img = new Image();monster06Img.src='art/dg_monster232.png';
	var monster07Img = new Image();monster07Img.src='art/dg_monster332.png';
	var monster08Img = new Image();monster08Img.src='art/dg_monster432.png';
	var monster09Img = new Image();monster09Img.src='art/dg_monster532.png';
	var monster10Img = new Image();monster10Img.src='art/dg_monster632.png';
	var monster11Img = new Image();monster11Img.src='art/dg_monster732.png';
/* Class specific Database variables */
	/* choose class state */
	var charGroup = new Array(),charClass = new Array(),charPanel = new Array();
	/* regional database only variables */
	var classDB = {
		max:0,                   //the maximum number of classes in the database
		tileSet:    new Array(), //tileset of each specific class
		tilePos:{                //grab the tileposition within the tileset
			x:      new Array(), //the tile position x
			y:      new Array()  //the tile position y
		}, //end tilePos
		name:       new Array(), //the name of the class
		description:new Array()  //the description of the class
	}; //end classDB
/* race specific variables */
	var raceDB = {
		max:0,                         //the maximum number of classes in the database
		tileSet:          new Array(), //tileset name for the current race tile
		tilePos:{                      //the tile position within the tileset
			x:            new Array(), //x location of tile on tileset
			y:            new Array()  //y location of tile on tileset
		}, //end tilePos
		stats:{                        //all of the corestats for each race
			strength:     new Array(), //core stat strength for the current race
			dexterity:    new Array(), //core stat dexterity for the current race
			constitution: new Array(), //core stat constitution for the current race
			intelligence: new Array(), //core stat intelligence for the current race
			wisdom:       new Array(), //core stat wisdom for the current race
			charisma:     new Array(), //core stat charisma for the current race
			luck:         new Array()  //core stat luck for the current race
		}, //end stats
		naturalAC:        new Array(), //natural armor class for the current race
		height:           new Array(), //height range for the current race
		weight:           new Array(), //weight range for the currrent race
		haircolor:        new Array(), //hair color options for the current race
		eyecolor:         new Array(), //eye color options for the current race
		skincolor:        new Array(), //skin color options for the current race
		name:             new Array(), //the name of the class
		description:      new Array()  //the description of the class
	}; //end classDB
/* Mobiles specific variables */
	var mobs = []; //will be an array of mobiles
	var mobDB = {
		max:0,               //the maximum number of mobiles in the database
		tileSet:new Array(), //the tileset for the specific mob tile
		tilePos:{            //the location of the tile within the tileset
			x:  new Array(), //x position of tile within tilset for mmob
			y:  new Array()  //y position of tile within tileset for mob
		}, //end tilePos	
		num:    new Array(), //the current number of the mobile
		nam:    new Array(), //the current name of the mobile
		hei:    new Array(), //the current height of the mobile
		sym:    new Array(), //the current ascii symbol of the mobile
		hit:    new Array(), //the current hitpoints of the mobile
		dam:    new Array(), //the current damage roll of the mobile
		des:    new Array() //the current description of the mobile
	}; //end mobDB
/* Item specific variables */
	/* Items currently being used within the game */
	var items = {
		curItem:new Array(), //the cur item will dictate what number of weapon or armor the curitem is listed under
		weapons:new Array(), //will be an array of the weapons
		armor  :new Array()  //will be an array of the armor
	}; //end items
	/* weapon database items */
	var weaponDB = {
		max        :0,           //the maximum number of weapons in the database
		name       :new Array(), //the weapon name
		character  :new Array(), //the weapon character displayed
		damageType :new Array(), //the weapons damage type
		description:new Array(), //the weapons general description
		classReq   :new Array(), //the weapons class requirement
		damage     :new Array(), //the weapons damage
		twoHanded  :new Array(), //the weapons two-handed requirement
		worth      :new Array(), //the weapons worth / cost
		levelReq   :new Array()  //the weapons level requirement
	}; //end weaponDB
	/* armor database items */
	var armorDB = {
		max           :0,           //the maximum number of armor in the database
		name          :new Array(), //the armor name
		character     :new Array(), //the armor character displayed
		equipmentType :new Array(), //the armors equipment type
		proficiencyReq:new Array(), //the armors proficiency requirement
		levelReq      :new Array(), //the armors level requirement
		genderReq     :new Array(), //the armors gender requirement
		bashAC        :new Array(), //the armors bash armor class
		slashAC       :new Array(), //the armors slash armor class
		pierceAC      :new Array(), //the armors slash armor class
		fistAC        :new Array(), //the armors fist armor class
		exoticAC      :new Array(), //the armors exotic armor class
		worth         :new Array(), //the armors worth
		weight        :new Array(), //the armors weight
		description   :new Array()  //the armors description
	}; //end armorDB
/* Environment specific variables */
	/* current environment loaded variables */
	var environmentName='none',environmentColor,playerColor; //current environment and colors therein
	var c1,c2,c3,p1,p2,p3; //this is the color of the floor (used in playermovement &drawmap)
	var environment = {
		name:'none', //the name of the current environment
		color:'none',//the color of the current environment floor
		wall:{       //collection of the wall types randomly allowable within current environment type
			floor:0, //floor is a type allowable as wall
			water:0, //water is a type allowable as wall
			flora:0, //flora (random environment features) is a type allowable as wall
			rock:0,  //rock is a type allowable as wall
			shrub:0, //shrub is a type allowable as wall
			tree:0,  //tree is a type allowable as wall
			lava:0,  //lava is a type allowable as wall
			wall:0   //wall is a type allowable as wall
		} //end wall
	}; //end environment
	/* entire environment database variables */
	var environmentDB = { 
		max:0,                 //the maximum number of environments in the database
		name:new Array(),      //the name of the environment
		color:new Array(),     //the color of the floor
		wall:{                 //collection of wall types allowable in cur environment
			floor:new Array(), //floor is a type allowable as wall
			water:new Array(), //water is a type allowable as wall
			flora:new Array(), //flora (random environment features) is a type allowable as wall
			rock: new Array(), //rock is a type allowable as wall
			shrub:new Array(), //shrub is a type allowable as wall
			tree: new Array(), //tree is a type allowable as wall
			lava: new Array(), //lava is a type allowable as wall
			wall: new Array()  //wall is a type allowable as wall
		} //end wall
	}; //end environmentDB
/* Player Specific variables */
	/* current Player inventory  */
	var playerInventory= {              //collection of player inventory specific variables
		cur:0,                          //current inventory amount filled
		max:10,                         //max fillable inventory slots
		itemName:new Array(),			//name of the item
		itemType:new Array(),           //current item type (armor or weapon)
		itemNum:new Array(),            //item number of the current item type
		itemCharacter:new Array(),      //item character represented on the map if dropped
		itemWeight:new Array(),         //current item weight
		itemEquipmentType:new Array(),  //current item equipment type if equipable
		itemDamageType:new Array(),     //current item damage type if weapon
		itemDescription:new Array(),    //current item description
		itemDamage:new Array(),         //current item damage variable if type is weapon
		itemTwoHanded:new Array(),      //current item two handed type if weapon
		itemWorth:new Array(),          //current item worth
		/* Armor class */ 
		itemBashAC:new Array(),         //current item bash armor class if type is armor
		itemSlashAC:new Array(),        //current item slash armor class if type is armor
		itemPierceAC:new Array(),       //current item pierce armor class if type is armor
		itemFistAC:new Array(),         //current item fist armor class if type is armor
		itemExoticAC:new Array(),       //current item exotic armor class if type is armor
		/* Requirements */
		itemLevelReq:new Array(),       //current item level requirement
		itemGenderReq:new Array(),      //current item gender requirement
		itemClassReq: new Array(),      //current item class requirement
		itemProficiencyReq:new Array()  //current item proficiency requirement
	}; //end playerInventory
	/* current player stats */
	var player={                              //collection of player related stats
		chp:80,mhp:80,                          //current player hitpoints and max player hitpoints
		cwp:100,mwp:100,                          //current player willpower and max player willpower
		cxp:0,mxp:25,                         //current player experience and max player experience required to level
		prace:"Kobold",                       //current player race
		pclass:"Elementalist",                //current player class
		pname:"John Hancock",                 //current player name
		level:1,                              //current player level
		height:4,                             //current player height
		weight:80,                             //current player weight
		ac:{                                  //collection of armor class damage reductions
			natural:20,                        //armor class natural
			bash:0,                           //armor class bash
			slash:0,                          //armor class slash
			pierce:0,                         //armor class pierce
			fist:0,                           //armor class fist
			exotic:0                          //armor class exotic
		}, //end ac
		stats:{                               //collection of core stats
			strength:8,strengthMax:13,         //current and max strength amounts for core stat
			dexterity:8,dexterityMax:13,       //current and max dexterity amounts for core stat
			constitution:8,constitutionMax:13, //current and max constitution amounts for core stat
			intelligence:10,intelligenceMax:15, //current and max intelligence amounts for core stat
			wisdom:14,wisdomMax:19,             //current and max wisdom amounts for core stat
			charisma:13,charismaMax:18,         //current and max charisma amounts for core stat
			luck:9,luckMax:14                  //current and max luck amounts for core stat
		}, //end stats
		tilePos:{                             //player tile position for the tileset
			x:4,                              //player tile x position
			y:10                               //player tile y position
		}, //end tilePos
		mapPos:{                              //collection of x and y positions for player map coordination
			x:0,                              //Player x location on the map
			y:0                               //Player y location on the map
		}, //end mapPos
		fieldOfViewRange:4,                   //The size of the players view range.
		cMove:1,                              //keep track of the player movements for a "score"
		cDepth:1,                             //first depth won't draw stairs up. Depth is kept track of for a "score" gets progressively harder
		baseDamage:"1d4"                      //base damage player deals without weapons (AKA hand-to-hand combat)
	}; //end player
/* Panel related variables for game play */
	var inventoryOpen=0; //toggled inventory panel
	var equipOpen=0;     //toggled equipment panel
	var dropOpen=0;      //toggled drop item panel
	var characterOpen=0; //toggled character panel
	var lookOpen=0;      //toggled look at   panel
	var shootOpen=0;     //toggled shoot at  panel
	var panelOpen=0;     //based toggled variable to determine whether a panel is already open
/* Declare variables for textOutput */
	var textLine= new Array(10);
/* Main functional variables */
	var isPlayerLoc=function(x,y){return(player.mapPos.x==x&&player.mapPos.y==y?true:false);}; //is true if the position requested is the player