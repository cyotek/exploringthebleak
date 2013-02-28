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
function parseEnvironments(data) {
	xmlDoc = data;
	$(xmlDoc).find("environment").each(function(){
		if (parseInt($(this).attr('number'))>0 && parseInt($(this).attr('number'))<100){
			environmentDB.max++;			
			environmentDB.color.push($(this).attr(     'color'));
			environmentDB.name.push($(this).attr(      'name'));
			environmentDB.wall.water.push($(this).find('obstructions').text().indexOf("Water")==-1?0:1);
			environmentDB.wall.flora.push($(this).find('obstructions').text().indexOf("Flora")==-1?0:1);
			environmentDB.wall.rock.push($(this).find( 'obstructions').text().indexOf("Rock")==-1?0:1);
			environmentDB.wall.shrub.push($(this).find('obstructions').text().indexOf("Shrub")==-1?0:1);
			environmentDB.wall.tree.push($(this).find( 'obstructions').text().indexOf("Tree")==-1?0:1);
			environmentDB.wall.lava.push($(this).find( 'obstructions').text().indexOf("Lava")==-1?0:1);
			environmentDB.wall.wall.push($(this).find( 'obstructions').text().indexOf("Wall")==-1?0:1);
		} //end if
	});
} //end function
function parseWeapons(data) {
	xmlDoc = data;
	$(xmlDoc).find("item").each(function(){
		weaponDB.max++;
		weaponDB.name.push($(this).find(       'name'            ).text());
		weaponDB.character.push($(this).find(  'character'       ).text());
		weaponDB.damageType.push($(this).find( 'damagetype'      ).text());
		weaponDB.description.push($(this).find('description'     ).text());
		weaponDB.classReq.push($(this).find(   'classspecific'   ).text());
		weaponDB.damage.push($(this).find(     'damage'          ).text());
		weaponDB.twoHanded.push($(this).find(  'twohanded'       ).text());
		weaponDB.levelReq.push($(this).find(   'levelrequirement').text());
		weaponDB.worth.push($(this).find(      'worth'           ).text());
	});
} //end function
function parseArmor(data) {
	xmlDoc = data;
	$(xmlDoc).find("item").each(function(){
		armorDB.max++;
		armorDB.name.push($(this).find(          'name'                  ).text());
		armorDB.character.push($(this).find(     'character'             ).text());
		armorDB.equipmentType.push($(this).find( 'equipmenttype'         ).text());
		armorDB.proficiencyReq.push($(this).find('proficiencyrequirement').text());
		armorDB.levelReq.push($(this).find(      'levelrequirement'      ).text());
		armorDB.genderReq.push($(this).find(     'genderrequirement'     ).text());
		armorDB.bashAC.push($(this).find(        'bashac'                ).text());
		armorDB.slashAC.push($(this).find(       'slashac'               ).text());
		armorDB.pierceAC.push($(this).find(      'pierceac'              ).text());
		armorDB.fistAC.push($(this).find(        'fistac'                ).text());
		armorDB.exoticAC.push($(this).find(      'exoticac'              ).text());
		armorDB.worth.push($(this).find(         'worth'                 ).text());
		armorDB.weight.push($(this).find(        'weight'                ).text());
		armorDB.description.push($(this).find(   'description'           ).text());
	});
} //end function
function parseMobiles(data) {
	xmlDoc = data;
	$(xmlDoc).find("item").each(function(){
		mobDB.max++;
		mobDB.nam.push($(this).find(      'name'       ).text());
		mobDB.hei.push($(this).find(      'height'     ).text());
		mobDB.sym.push($(this).find(      'symbol'     ).text());
		mobDB.hit.push($(this).find(      'hitpoints'  ).text());
		mobDB.dam.push($(this).find(      'damage'     ).text());
		mobDB.des.push($(this).find(      'description').text());
		mobDB.tileSet.push($(this).find(  'tileset'    ).text());
		mobDB.tilePos.x.push($(this).find('tileset'    ).attr('x'));
		mobDB.tilePos.y.push($(this).find('tileset'    ).attr('y'));
	});
} //end function
function parseClasses(data) {
	xmlDoc = data;
	$(xmlDoc).find("class").each(function(){
		classDB.max++;
		classDB.name.push($(this).find(       'name'       ).text());
		classDB.description.push($(this).find('description').text());
		classDB.tileSet.push($(this).find(    'tileset'    ).text());
		classDB.tilePos.x.push($(this).find(  'tileset'    ).attr('x'));
		classDB.tilePos.y.push($(this).find(  'tileset'    ).attr('y'));
	});
} //end function
function parseRaces(data) {
	xmlDoc = data;
	$(xmlDoc).find("race").each(function(){
		raceDB.max++;
		raceDB.name.push($(this).find(              'name'       ).text());
		raceDB.description.push($(this).find(       'description').text());
		raceDB.tileSet.push($(this).find(           'tileset'    ).text());
		raceDB.tilePos.x.push($(this).find(         'tileset'    ).attr('x'));
		raceDB.tilePos.y.push($(this).find(         'tileset'    ).attr('y'));
		raceDB.stats.strength.push($(this).find(    'stats'      ).attr('str'));
		raceDB.stats.dexterity.push($(this).find(   'stats'      ).attr('dex'));
		raceDB.stats.constitution.push($(this).find('stats'      ).attr('con'));
		raceDB.stats.intelligence.push($(this).find('stats'      ).attr('int'));
		raceDB.stats.wisdom.push($(this).find(      'stats'      ).attr('wis'));
		raceDB.stats.charisma.push($(this).find(    'stats'      ).attr('cha'));
		raceDB.stats.luck.push($(this).find(        'stats'      ).attr('luc'));
		raceDB.naturalAC.push($(this).find(         'naturalac'  ).text());
		raceDB.height.push($(this).find(            'height'     ).text());
		raceDB.weight.push($(this).find(            'weight'     ).text());
		raceDB.haircolor.push($(this).find(         'haircolor'  ).text());
		raceDB.eyecolor.push($(this).find(          'eyecolor'   ).text());
		raceDB.skincolor.push($(this).find(         'skincolor'  ).text());
	});
} //end function