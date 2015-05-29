# ETB #
**Exploring The Bleak** is a Graphical **Roguelike** videogame created by **Nathaniel Inman** of _The Other Experiment Studio._ **ETB** was released on September 19th 2010 for **ARRP**, or the _Annual Roguelike Release Party_. The project was originally released as closed source, but is now available in open source. Information on the project and other projects by **Nathaniel Inman** or **TOES** respectively can be accessed at it's [homepage](http://www.theoestudio.com).

## Play The Games ##
The games are available for download [here](http://code.google.com/p/exploringthebleak/downloads/list).

## Screenshots ##

[![](http://i282.photobucket.com/albums/kk243/p03tic5ickn355/Exploring%20The%20Bleak/examplewaters.png)](http://www.theoestudio.com)
[![](http://i282.photobucket.com/albums/kk243/p03tic5ickn355/Exploring%20The%20Bleak/examplelavas.png)](http://www.theoestudio.com)

## Features ##
  * **Current**
    * Randomly Generated Dungeons (4 Presets : Ruins, Dungeons, Tunnels, Ordinal Tunnels)
    * 10 Dungeon Graphical Tilesets
    * 10 Simple Creatures
    * 100+ Randomly Generated Items
    * 20+ Unique Skills
    * Top Scores List
    * A Very Simple, Beatable Game.
  * **Version 2.4.x Update Log**
    * Added: 2 new map types : Tunnels, and Ordinal Tunnels (diagonal tunnels)
    * Added: river drawing mechanics.
    * Added: lava, water, and ice mechanics.
      * _Water reduces by 10 wp each round, and 10 hp once wp is gone._
      * _Ice reduces by 20 wp each round, and 20 hp once wp is gone._
      * _Lava reduces by 30 wp each round, and 30 hp once wp is gone._
    * Added: a quick inventory view which only shows the first 10 items in your inventory.
    * Added: hotkeys
      * _Added 'i' for inventory._
      * _Added 'ctrl' + 'q' to exit program._
      * _Added '?' & 'h' for help._
      * _Added 'ctrl' + '1' to use/equip inventory item 1._
      * _Added other ctrls up to 0 for inventory item 10._
      * _Added 'alt' + '1' to drop/destroy inventory item 1._
      * _Added other alts up to 0 for inventory item 10._
    * Added: 1 new item category : potions.
      * _Added drown immune potion._
      * _Added freeze immune potion._
      * _Added burn immune potion._
    * Changed: AI pathing adjusted with water, lava, and ice mechanics.
      * _Mobiles will now drown, freeze, and burn._
      * _Mobiles will chase player into water, lava, and ice but won't naturally enter it otherwise._
    * Changed: Moved hitpoints and willpower preview in creation to top of form.
    * Changed: Luck increases the items spawned on map a lot more.
      * _1-8 luck=0 items_
      * _9 luck=1 item_
      * _10 luck=2 items (Average)_
      * _11 luck=3 items etc.._
    * Changed: Luck increases the chance a mobile will drop items
      * _1 luck=5% chance to drop item_
      * _2 luck=10% chance to drop item ...etc.._
      * _10 luck=50% chance to drop item (Average)_
    * Changed: Wearing and destroying items is logged.
    * Changed: Chance to critical strike with a skill is now Intelligence\*2 percent (Greatly increased)
    * Changed: Experience has been equalized throughout the game.
    * Changed: Stairs up tile to be more appropriate.
    * Fixed: 131 weapon item drops on map so they show their correct label.
    * Fixed: Prohibited items to be dropped ontop of items.
    * Fixed: Mobiles no longer drop blank items.
    * Fixed: Prevented duplicate scores after character dies.
    * Fixed: high scores list to not include current character until it's dead and saved.
    * Fixed: Prevented dead flag from occuring more than once.
    * Fixed: target empty location indicator redraw property.
    * Removed: Neutral gender.

## Storyline ##
Under the land of Sedia lies a world entirely it's own, untouched by civilized hands and mysterious in origins. You are an average under-paid soldier sent on a suicide quest like many before you to investigate and retrieve the last remnant shard of the Everspark, a relic of magic said to have enough power to bring the world to it's knees in destruction and end quickly the Age of Man. Maybe you'll get a raise.
## Play The Game ##
The game is available for download on the bar to the right under "Featured Download."
### How To Play ###
Delve into the depths of the dungeons to retrieve the Everspark. In order to level up you kill enemies by walking into them. Strength makes your regular attacks stronger. Dexterity gives you the chance to dodge attacks. Constitution gives you damage reduction on all attacks. Wisdom gives you more points when you level to increase stats. Intelligence increases your skill damage. Luck increases the amount of loot that you find. Charisma increases the chance to cause enemies to flee.

There is a **class skills list** available at the local wiki [here](http://code.google.com/p/exploringthebleak/wiki/ClassSkills).
### Controls ###
In order to level up you kill enemies by walking into them. use the num pad or arrow keys to move. Numpad keys 7,9,3, and 1 move diagnally.

  * 'p' or 't' picks up items.
  * 'i' shows your inventory.
  * '?' or 'h' shows and hides this help.
  * '5' on the num pad waits a turn and allows you to heal 1HP/1WP.
  * '1' is your primary skill
  * '2' is your secondary skill
  * '3' is your support skill
  * '<' goes up stairs.
  * '>' goes down stairs.
  * 'f1' shows the high scores.
  * 'f2' shows your character stats.
  * 'f3' shows or hides the activity log.
  * 'f4' exits game and restarts character creation.
  * 'f5' enables graphical mode.
  * 'f6' enables ascii mode.
  * 'f7' turns on image filtering.
  * 'f8' turns off image filtering.

## The Source Code : VB.NET ##
**Exploring The Bleak** was created using VB.NET which is different than most Roguelikes that generally use Java, C derivatives, Python, etc. It more-or-less was a proof of concept. The language was readily available as Nathaniel Inman was using it at work for a customer.
## Open Source Licensing ##
All source code to the game is GNU GPL v3, all artwork is protect copyright of it's owner Nathaniel Inman. Permission may be granted to use artwork of the game by only Nathaniel Inman himself either in person or in writing to his email address available on **TOES** [homepage](http://www.theoestudio.com).
# Kepler-10b #
Kepler-10b is a fork of Exploring The Bleak. A fork is a derivative of a video game based of it's parents framework and are often called branches. Kepler-10b was created for 7DRLC 2011 which was held on March 5th to March 12th of 2011.

**March 4th, 2011 (1 day before 7DRL Competition)** - _I'm anticipating another successful 7DRL this year. Having made so many graphical games lately I felt it necessary to ensure that this 7DRL I release something a little more... classic. What might make the displaying of this game unique is its step away from ASCII and towards Unicode 6. What if those Ethiopian characters where aliens? Long live the @!_
## Storyline ##
You are on the Cygnas System exploration team, sent to explore and take samples from the world Kepler-10b which was recently discovered to have an atmosphere coherent to livable standards. K10b is mostly rocky with little foliage, and a much higher gravity pull than Earth. It rotates around it's sun K10 every 19 1/4 Earth hours. K10b was discovered 827 years ago today, in February of 2011.

You are on Earth-week 2 of 3 of the split-team exploration.  There was, unbeknownst to  you, alien life  on K10b and they, during your second week, amounted resistance and your core team barely escaped alive.

In order to return home you have to retrieve the ship access keys. There are five. One is already in your possession as you are captain. You must find the 4 other pilots key cards to access the main drive. The ship had split into 5 teams, each under command of a pilot. You haven't heard from the other teams since landing and hadn't planned to meet with them until week 3 where all teams were to rendezvous at the ship rally point. Wide-band communications won't reach past a thousand or so feet due to strange wave emulsions that pulse every few seconds from the K10b core.

Command your team to whatever style suits you. Wait at the ship to see if all teams will return, and possibly die of starvation from local resistance (technically, you're the aliens,) or trudge the designated locations the teams were supposed to take samples from and hopefully obtain the access keys to return home. The choice is yours to make.

### How To Play ###
Retrieve all of the access key cards and head to the drop ship to return home. Strength increases your melee attack strength, Dexterity increases your chances to dodge attacks, Constitution gives damage reduction on all attacks, Wisdom gives you more points to distribute when you level up, Intelligence increases your ranged damage, Luck increases the amount of loot that you find, and Charisma increases the chance you'll cause enemies to flee in horror. HP is your health points, when it reaches 0 you die. EN is your suit energy which is used for your ranged weapon, when it reaches 0 you are left with melee attacks only until you recharge your energy by resting or items.

### Controls ###
In order to level up you kill enemies by walking into them (attacking them with melee) or shooting them. use the num pad or arrow keys to move. Numpad keys 7,9,3, and 1 move diagonally.

  * 'p' or 't' picks up items.
  * 'i' shows your inventory.
  * '?' or 'h' shows and hides this help.
  * 's' toggles shooting the enemy (targets) press spacebar to shoot.
  * '5' on the num pad waits a turn and allows you to heal 1HP/1EN.
  * '1' is your primary skill
  * '2' is your secondary skill
  * '3' is your support skill
  * 's' shoots your ranged weapon, which consumes energy.
  * '<' goes up stairs.
  * '>' goes down stairs.
  * 'f1' shows the high scores.
  * 'f2' shows or hides the activity HUD.
  * 'f3' shows your character stats.
  * 'f4' shows level up panel.
  * 'f5' exits current game or screensaver and starts new game.

## Group Types ##
### Stealth Squad ###
Stealth squad focuses on ranged attacks and dealing damage before enemeis can react. When forced into melee combat the stealth squad can react quickly, dexterously, and dodge attacks and run to ranged formations to deal damage once again. They are deadly and feared in later levels, their charismatic formation causes enemies to flee.
### Munitions Squad ###
Munitions squad is a full-force combat squad directed to be the first contact of extraterrestrial resistance. Munitions squad employs direct damage in both melee and ranged scenarios and are equiped with more vitality than other squads, allowing a higher percentage of survival in stressed situations. THough they are not versatile, they do instill intense fear in opposition.. rightfully so.
### Research Squad ###
Research squad is aimed to be primarily focused on item retrieval and are incredibly versatile in learning, progressing both in damage or defense faster than any other squad if it is their objective while gaining experience during exploration. Research squad employs the greatest ranged damage due to their precise strikes. This efficiency and the naturally higher energy output of the group marks them quite useful.
## Classes ##
### Captain ###
The captains upgrades indicate how many rank points he will receive to upgrade his team every time they level up. These rank points can not only level the captains rank up but other class ranks within his group as well. Rank points are important and there are a variety of strategies on which to distribute into first depending on the players style and preference.
### Mule ###
The mules upgrades increase the total items he can carry for the group. No other character carries items within the group other than what they have equiped which consists of a ranged weapon and armor.
### Scout ###
The scout increases the visibility range of the group with each upgraded rank.
### Tank ###
The tank becomes more proficient in grabbing aggression and damage from the enemies as they attack and he levels in ranks. Eventually the tank grabs all aggression from the enemies and can cause them to flee in terror which breaks ranks of enemies and reduces overall damage that the tank receives for a period of time.
### Benefactor ###
The benefactor becomes better at regenerating energy of the group naturally each round, and eventually can regenerate and cure the groups health as she becomes more adept in ranks.
## The Source Code : VB.NET ##
**Kepler-10b** was created using VB.NET and was forked from it's mother **Exploring The Bleak**. Code from k10b that is found to be a potential extension of ETB will be reviewed and added as it is approved in it's perspective issue number.
## Open Source Licensing ##
All source code to the game is GNU GPL v3, all artwork is protect copyright of it's owner Nathaniel Inman. Permission may be granted to use artwork of the game by only Nathaniel Inman himself either in person or in writing to his email address available on **TOES** [homepage](http://www.theoestudio.com).