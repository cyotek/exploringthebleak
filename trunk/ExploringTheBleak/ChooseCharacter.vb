Public Class ChooseCharacter
#Region "Race Information"
    Private Sub Race_Human_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Human.CheckedChanged
        If Race_Human.Checked = True Then
            description.Text = "Most of the continent of Sedia contains humans, the newest race, and are breeding out the rest. Humans can be any class as they are most versatile."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "4' to 6'"
            weight.Text = "120' to 240'"
            PrimaryStat.Text = "STR"
            SecondaryStat.Text = "DEX"
            str.Text = "10"
            dex.Text = "10"
            int.Text = "10"
            wis.Text = "10"
            con.Text = "10"
            cha.Text = "10"
            luc.Text = "10"
        End If
    End Sub
    Private Sub Race_Dwarf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Dwarf.CheckedChanged
        If Race_Dwarf.Checked = True Then
            description.Text = "Little are seen of the dwarves outside of Jewall in the Northern Reach besides King Mishnal Stonecleaver of Tharsis. Dwarves are usually priests, monks, headhunters, plainsmen, pages, and hermits."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "3' to 5'"
            weight.Text = "120' to 230'"
            PrimaryStat.Text = "STR"
            SecondaryStat.Text = "CON"
            str.Text = "12"
            dex.Text = "8"
            int.Text = "9"
            wis.Text = "11"
            con.Text = "12"
            cha.Text = "9"
            luc.Text = "9"
        End If
    End Sub
    Private Sub Race_Gnome_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Gnome.CheckedChanged
        If Race_Gnome.Checked = True Then
            description.Text = "Slaves of Dwarves because of racial hatred, they are recently breaking into freedom into other parts of the continent. Gnomes are usually magelings, gravediggers, minstrels, elementalists, or pickpockets."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = True : Sex_Male.Checked = True
            height.Text = "2' to 4'"
            weight.Text = "60' to 170'"
            PrimaryStat.Text = "WIS"
            SecondaryStat.Text = "CON"
            str.Text = "11"
            dex.Text = "9"
            int.Text = "11"
            wis.Text = "9"
            con.Text = "10"
            cha.Text = "9"
            luc.Text = "11"
        End If
    End Sub
    Private Sub Race_Elf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Elf.CheckedChanged
        If Race_Elf.Checked = True Then
            description.Text = "Habitually in warmer climate such as Darkwood, this secluded race communicates little with the rest of the world. Elves are usually trackers, magelings, priests, monks, scouts, hermits, pickpockets, or runescribes."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "4' to 5'"
            weight.Text = "70' to 130'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "CHA"
            str.Text = "9"
            dex.Text = "12"
            int.Text = "11"
            wis.Text = "11"
            con.Text = "7"
            cha.Text = "11"
            luc.Text = "8"
        End If
    End Sub
    Private Sub Race_Halfelf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Halfelf.CheckedChanged
        If Race_Halfelf.Checked = True Then
            description.Text = "A rare race found occasionally in Tharsis, generally hard to identify because most Half-elfs hide their elven heritage. Half-elves are generally trackers, runescribes, minstrels, pickpockets, or elementalists."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = True : Sex_Male.Checked = True
            height.Text = "4' to 5'"
            weight.Text = "90' to 140'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "WIS"
            str.Text = "10"
            dex.Text = "12"
            int.Text = "10"
            wis.Text = "10"
            con.Text = "9"
            cha.Text = "10"
            luc.Text = "9"
        End If
    End Sub
    Private Sub Race_Halfling_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Halfling.CheckedChanged
        If Race_Halfling.Checked = True Then
            description.Text = "Half the height of a normal man, halflings can sometimes be mischievous but generally well-rounded. They are said to have originated from the War of Souls. Halflings excel at being monks, minstrels, hermits, or pickpockets."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = True : Sex_Male.Checked = True
            height.Text = "2' to 3'"
            weight.Text = "60' to 120'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "LUC"
            str.Text = "9"
            dex.Text = "11"
            int.Text = "10"
            wis.Text = "10"
            con.Text = "9"
            cha.Text = "10"
            luc.Text = "11"
        End If
    End Sub
    Private Sub Race_Goblin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Goblin.CheckedChanged
        If Race_Goblin.Checked = True Then
            description.Text = "Approximately half the height of a human, goblins are discolored, disfigured, criticized, and misbehaved intelligent half-breed creatures spawned of Daemune. Goblins excel at being magelings, gravediggers, elementalists, pickpockets, and hermits."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "2' to 4'"
            weight.Text = "70' to 80'"
            PrimaryStat.Text = "CHA"
            SecondaryStat.Text = "INT"
            str.Text = "8"
            dex.Text = "8"
            int.Text = "12"
            wis.Text = "9"
            con.Text = "12"
            cha.Text = "12"
            luc.Text = "9"
        End If
    End Sub
    Private Sub Race_Troll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Troll.CheckedChanged
        If Race_Troll.Checked = True Then
            description.Text = "Trolls are generally tall and residential of desert climates. They are a family-based race dependant upon the movement of their tribe. Trolls are generally magelings, priests, scouts, plainsmen, headhunters, and runescribes."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "5' to 8'"
            weight.Text = "280' to 420'"
            PrimaryStat.Text = "STR"
            SecondaryStat.Text = "LUC"
            str.Text = "11"
            dex.Text = "12"
            int.Text = "11"
            wis.Text = "9"
            con.Text = "11"
            cha.Text = "9"
            luc.Text = "10"
        End If
    End Sub
    Private Sub Race_Orc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Orc.CheckedChanged
        If Race_Orc.Checked = True Then
            description.Text = "Orcs are dark-figured misshapen offspring of Daemune and Giants. Their unorthodox small height is made up by their concrete attitude. Orcs are generally magelings, plainsmen, headhunters, pages, and gravediggers."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "5' to 6'"
            weight.Text = "160' to 280'"
            PrimaryStat.Text = "STR"
            SecondaryStat.Text = "INT"
            str.Text = "12"
            dex.Text = "8"
            int.Text = "11"
            wis.Text = "8"
            con.Text = "12"
            cha.Text = "9"
            luc.Text = "10"
        End If
    End Sub
    Private Sub Race_Halforc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Halforc.CheckedChanged
        If Race_Halforc.Checked = True Then
            description.Text = "Half-orcs are sometimes seen in the Northern Reach where humans have been raped countless times by the ruthless hordes of invading Orcs. Half-orcs may be pages, headhunters, and plainsmen."
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "5' to 6'"
            weight.Text = "150' to 250'"
            PrimaryStat.Text = "STR"
            SecondaryStat.Text = "CON"
            str.Text = "12"
            dex.Text = "9"
            int.Text = "10"
            wis.Text = "8"
            con.Text = "12"
            cha.Text = "9"
            luc.Text = "10"
        End If
    End Sub
    Private Sub Race_Quickling_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Quickling.CheckedChanged
        If Race_Quickling.Checked = True Then
            description.Text = "Quicklings are a third the size of a human. Quicklings are bald and always mischievous and self-centered, with the occasion of talking too fast. Quicklings are generally monks, minstrels, or pickpockets"
            Sex_Male.Enabled = True : Sex_Female.Enabled = True : Sex_Neutral.Enabled = True : Sex_Male.Checked = True
            height.Text = "2' to 3'"
            weight.Text = "20' to 50'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "DEX"
            str.Text = "8"
            dex.Text = "14"
            int.Text = "8"
            wis.Text = "8"
            con.Text = "9"
            cha.Text = "12"
            luc.Text = "11"
        End If
    End Sub
    Private Sub Race_Pixie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Pixie.CheckedChanged
        If Race_Pixie.Checked = True Then
            description.Text = "Pixies are a small mysterious creature created by Lythantos during the War of Souls. They now wander the Bleak aimlessly. Pixies are generally magelings and priests."
            Sex_Male.Enabled = False : Sex_Female.Enabled = True : Sex_Neutral.Enabled = False : Sex_Female.Checked = True
            height.Text = "1' to 2'"
            weight.Text = "8' to 15'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "INT"
            str.Text = "8"
            dex.Text = "12"
            int.Text = "11"
            wis.Text = "12"
            con.Text = "8"
            cha.Text = "11"
            luc.Text = "8"
        End If
    End Sub
    Private Sub Race_Sprite_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Race_Sprite.CheckedChanged
        If Race_Sprite.Checked = True Then
            description.Text = "Sprites are a small mysterious creature created by Lythantos during the War of Souls. They now wander the Bleak aimlessly. Sprites are generally magelings and gravediggers."
            Sex_Male.Enabled = True : Sex_Female.Enabled = False : Sex_Neutral.Enabled = False : Sex_Male.Checked = True
            height.Text = "1' to 2'"
            weight.Text = "9' to 20'"
            PrimaryStat.Text = "DEX"
            SecondaryStat.Text = "INT"
            str.Text = "8"
            dex.Text = "12"
            int.Text = "12"
            wis.Text = "11"
            con.Text = "9"
            cha.Text = "10"
            luc.Text = "8"
        End If
    End Sub
#End Region
#Region "Class Information"
    Private Sub Priest_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Priest.CheckedChanged
        If Priest.Checked = True Then
            ClassDescription.Text = "A Priest grows through a well organized foundation of beliefs and political structure and bases much of his or her authority to secure religious structure amongst their people. A Priest is unfamiliar with combat and is likewise extremely skilled in politics and healing the wounded and sick. Their primary stat is Charisma and their Secondary stat is Intelligence."
            HealthValue.Text = "40"
            WillpowerValue.Text = "60"
            Skill1.Image = My.Resources.Punch : Skill1Desc.Text = "Punch : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Heal : Skill2Desc.Text = "Heal : Heals 20 hitpoints. Costs 20 willpower. Causes a 5 round global cooldown of skills."
            Skill3.Image = My.Resources.HolyBolt : Skill3Desc.Text = "Holy Bolt : Strikes target for 10 damage in addition to a normal attack. Costs 20 willpower. Causes a 4 round global cooldown of skills."
        End If
    End Sub
    Private Sub Woodsman_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Woodsman.CheckedChanged
        If Woodsman.Checked = True Then
            ClassDescription.Text = "A Woodsman has little need for society and its petty quanderings, and instead relies on his intuition and well-versed knowledge of nature to procure the needed inertia to modulate any outcome of a battle or encounter. Some druids form basic communication skills depending on their background but lack the necessary skills to effectively gather sufficient information or persuade and develop constructive insight on civilization-based problems. Their primary stat is Wisdom and their secondary stat is Constitution."
            HealthValue.Text = "60"
            WillpowerValue.Text = "40"
            Skill1.Image = My.Resources.Shoot : Skill1Desc.Text = "Shoot : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Hide : Skill2Desc.Text = "Hide : Hide from all enemies for 6 turns. Costs 10 willpower. Causes a 6 round global cooldown of skills."
            Skill3.Image = My.Resources.FireArrow : Skill3Desc.Text = "Fire Arrow : Strike target for 3 damage in addition to a regular attack. Costs 15 willpower. Causes a 3 round global cooldown of skills."
        End If
    End Sub
    Private Sub Gravedigger_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gravedigger.CheckedChanged
        If Gravedigger.Checked = True Then
            ClassDescription.Text = "A Gravedigger feels the nee to expand his knowledge of the mostly unknown and mystic subject of death. They sometimes find that pain can be delightful and relish their own death, bnut would hope that surely it could be strung out a bit longer if possible so they coud assess it's many developments upon the way. Gravediggers can be somewhat social at times, but it isn't uncommon to see a hermetic gravedigger. Their primary stat is Intelligence and their Secondary, Charisma."
            HealthValue.Text = "40"
            WillpowerValue.Text = "60"
            Skill1.Image = My.Resources.Kick : Skill1Desc.Text = "Kick : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.BoneShield : Skill2Desc.Text = "Bone Shield : Protects gravedigger from all damage for 5 rounds. Costs 30 willpower. Causes a 10 round global cooldown of skills."
            Skill3.Image = My.Resources.Leech : Skill3Desc.Text = "Leech : Gravedigger receives hitpoints in the amount of his attack score, leeched from the enemy. Costs the gravediggers attack score in amount of willpower."
        End If
    End Sub
    Private Sub Mageling_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mageling.CheckedChanged
        If Mageling.Checked = True Then
            ClassDescription.Text = "Mages form a well-structured society of knowledge harbringers and socialites. Mages form colleges and go throughout life and their travels always looking for anothe rpiece of information that they know nothing about. There is little they would not delve their fingers into except the forbidden art of necromancy. Their primary stat is Intelligence and their secondary is Wisdom."
            HealthValue.Text = "30"
            WillpowerValue.Text = "70"
            Skill1.Image = My.Resources.Punch : Skill1Desc.Text = "Punch : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.MagicShield : Skill2Desc.Text = "Magic Shield : Reduces 1 damage from all attacks for 10 rounds. Costs 30 willpower. Causes a 10 round global cooldown of skills."
            Skill3.Image = My.Resources.Fireball : Skill3Desc.Text = "Fireball : Attacks target for 10 damage in addition to a regular attack. Costs 30 willpower. Causes a 3 round global cooldown of skills."
        End If
    End Sub
    Private Sub Hermit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Hermit.CheckedChanged
        If Hermit.Checked = True Then
            ClassDescription.Text = "Hermits live always curious about the many weaves they can see and distinguish amongst the world. ALl elemental weaves, spirit, deraconian sub-elemental magic is assessable to them, and with all the knowledge in the world there is still more meaning, a question tha tbegs the quandering of their feeble brains. They search long and wide for questions, not answers, something they cannot solve. Their primary stat is Wisdom and their secondary, Intelligence."
            HealthValue.Text = "30"
            WillpowerValue.Text = "70"
            Skill1.Image = My.Resources.Hit : Skill1Desc.Text = "Hit : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Counter : Skill2Desc.Text = "Counter : Counters all attacks from all enemies for 3 rounds. Counters can miss or be dodged. Costs 25 willpower. Causes a 5 round global cooldown of skills."
            Skill3.Image = My.Resources.Clumsiness : Skill3Desc.Text = "Clumsiness : Reduces a targets chance to hit by 50 percent. Costs 20 willpower. Causes a 10 round global cooldown of skills."
        End If
    End Sub
    Private Sub Elementalist_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Elementalist.CheckedChanged
        If Elementalist.Checked = True Then
            ClassDescription.Text = "Elementalists emblazon themselves with powerful elemental auras to protect themselves from their enemies. Elementalists are chaotic mages, their magic naturally occurring and in much greater power, but less control. They are warriors of a rare kind and relish the chaotic part of nature, the systematic pseudo-meme. They leech on the real world to humor the imagination. Their primary stat is Strength, and secondary: Intelligence."
            HealthValue.Text = "30"
            WillpowerValue.Text = "70"
            Skill1.Image = My.Resources.Strike : Skill1Desc.Text = "Strike : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.MagicShield : Skill2Desc.Text = "Magic Shield : Reduces 1 damage from all attacks for 10 rounds. Costs 30 willpower. Causes a 10 round global cooldown of skills."
            Skill3.Image = My.Resources.Immolate : Skill3Desc.Text = "Immolate : Elementalist immolates a fire shield that causes 1 damage to all attackers for 5 rounds. Costs 20 willpower. Causes a 10 round global cooldown of skills."
        End If
    End Sub
    Private Sub Headhunter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Headhunter.CheckedChanged
        If Headhunter.Checked = True Then
            ClassDescription.Text = "Headhunters are most prominently warriors who find little difficulty in disposing o thieves and law-breakers. They use the shadows to hide and break their opponents, and give orders easily to others forming controlled combat situations. They develop great authority quickly and at times can be an extension of the law itself. Their primary stat is Strength and their secondary is Charisma."
            HealthValue.Text = "70"
            WillpowerValue.Text = "30"
            Skill1.Image = My.Resources.Stab : Skill1Desc.Text = "Stab : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Wound : Skill2Desc.Text = "Wound : Attack an enemy causing 5 damage in addition to a normal attack. Costs 20 willpower. Causes a 5 round global cooldown of skills."
            Skill3.Image = My.Resources.Stun : Skill3Desc.Text = "Stun : Stun target for 3 rounds. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        End If
    End Sub
    Private Sub PLainsman_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PLainsman.CheckedChanged
        If PLainsman.Checked = True Then
            ClassDescription.Text = "Plainsmen are the warriors of the real world, well-versed in natural combat with any weapon and with little remorse or tactical  stipulations. Plainsmen have simpel concepts of yes or no, live or die, and think little of consequences more-as the basic reason and basis of survival. Their primary stat is Strength and their secondary is Dexterity."
            HealthValue.Text = "80"
            WillpowerValue.Text = "20"
            Skill1.Image = My.Resources.Slice : Skill1Desc.Text = "Slice : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Fury : Skill2Desc.Text = "Fury : Increases damage in regular attacks by 1 for 5 rounds. Costs 20 willpower. Causes a 5 round global cooldown of skills."
            Skill3.Image = My.Resources.Sacrifice : Skill3Desc.Text = "Sacrifice : Sacrifice 10 HP and deal regular damage plus 10. Causes a 5 round global cooldown of skills."
        End If
    End Sub
    Private Sub Pickpocket_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Pickpocket.CheckedChanged
        If Pickpocket.Checked = True Then
            ClassDescription.Text = "Pickpockets are simply the thieves. They think around the corner, they are aware at all times, they have luck, live for pleasure, and think little of consequences. Pickpockets are a rare breed in the law-making cities of the new world, but are a remnant of history and fade in and out of the shadows, always there, but rarely seen. Their primary stat is Dexterity and secondary stat is Luck."
            HealthValue.Text = "70"
            WillpowerValue.Text = "30"
            Skill1.Image = My.Resources.Punch : Skill1Desc.Text = "Punch : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Trip : Skill2Desc.Text = "Trip : Trip target preventing them from moving or attacking for 2 rounds. Costs 10 willpower. Causes a 2 round global cooldown of skills."
            Skill3.Image = My.Resources.Backstab : Skill3Desc.Text = "Backstab : Attack target for regular attack damage plus 10. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        End If
    End Sub
    Private Sub Page_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Page.CheckedChanged
        If Page.Checked = True Then
            ClassDescription.Text = "Pages are well-versed in armor and protection. They are defenders of their cause, whatever it may be, and are fully capable, if any one person is, to stand up to the strongest foe with the smallest amount of effort. They protect for it is just to value others before yourself. Their primary stat is Constitution and secondary stat is Strength."
            HealthValue.Text = "60"
            WillpowerValue.Text = "40"
            Skill1.Image = My.Resources.Strike : Skill1Desc.Text = "Strike : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Block : Skill2Desc.Text = "Block : Reduces all damage for 2 rounds by 2. Costs 15 willpower. Causes a 2 round global cooldown of skills."
            Skill3.Image = My.Resources.Stun : Skill3Desc.Text = "Stun : Stun target for 3 rounds. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        End If
    End Sub
    Private Sub Scout_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Scout.CheckedChanged
        If Scout.Checked = True Then
            ClassDescription.Text = "Scouts are guardians of monks, and allow monks to live and reside peacefully without slight of temperament. Scouts are empty-minded warriors of wind who quickly dispatch an enemy and meditate later about the battle. Scouts are of no emotion. Their primary stat is Dexterity and secondary stat is Charisma."
            HealthValue.Text = "60"
            WillpowerValue.Text = "40"
            Skill1.Image = My.Resources.Slice : Skill1Desc.Text = "Slice : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.DoubleSlice : Skill2Desc.Text = "Double Slice : Attack the target twice in 1 round. Costs 15 willpower. Causes a 2 round global cooldown of skills."
            Skill3.Image = My.Resources.TripleSlice : Skill3Desc.Text = "Triple Slice : Attack the target 3 times in 1 round. Costs 30 willpower. Causes a 4 round global cooldown of skills."
        End If
    End Sub
    Private Sub Runescribe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Runescribe.CheckedChanged
        If Runescribe.Checked = True Then
            ClassDescription.Text = "Runescribes breathe the arcane magic of the dragons and gods into runes. They see the make-up of nature beyond the natural weaves into something more delicate and precise. They are feared greatly and demand great respect by all. They are a dying breed of warriors who speak little because their presence speaks for them. Their primary stat is Wisdom and their secondary stat is Charisma."
            HealthValue.Text = "40"
            WillpowerValue.Text = "60"
            Skill1.Image = My.Resources.Kick : Skill1Desc.Text = "Kick : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Runestrike : Skill2Desc.Text = "Runestrike : Attack the target normally and additionally prevent target from attacking for 2 rounds. Costs 20 willpower. Causes a 3 round global cooldown of skills."
            Skill3.Image = My.Resources.Whisper : Skill3Desc.Text = "Whisper : Causes target to die immediately. Costs 50 willpower. Causes a 10 round global cooldown of skills."
        End If
    End Sub
    Private Sub Monk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Monk.CheckedChanged
        If Monk.Checked = True Then
            ClassDescription.Text = "Monks are peaceful warriors who battle without weapons and are martially biased to avoid combat if at all possible. Monks relish peace and will see to it at any means they deem necessary. THei rprimary stat is Wisdom and their secondary stat is Dexterity."
            HealthValue.Text = "50"
            WillpowerValue.Text = "50"
            Skill1.Image = My.Resources.Punch : Skill1Desc.Text = "Punch : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Kick : Skill1Desc.Text = "Kick : Basic attack plus 1 damage. Costs 5 willpower."
            Skill3.Image = My.Resources.Stun : Skill3Desc.Text = "Stun : Stun target for 3 rounds. Costs 25 willpower. Causes a 5 round global cooldown of skills."
        End If
    End Sub
    Private Sub Minstrel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Minstrel.CheckedChanged
        If Minstrel.Checked = True Then
            ClassDescription.Text = "Minstrels bring the harmony of music and lilting melodies to the battlefield unconventionally to boost the morale of their comrades or later in a much more discrete and unfathomable way: battle. Minstrels are happy and are the livelier bunch who most often then not, can pull a losing battle in the opposite direction with merely their presence. They are the support strings of any group and should never be considered less than fully capable warriors as well. Their primary stat is Charisma, and secondary stat is Luck."
            HealthValue.Text = "40"
            WillpowerValue.Text = "60"
            Skill1.Image = My.Resources.Stab : Skill1Desc.Text = "Stab : Basic attack plus 1 damage. Costs 5 willpower."
            Skill2.Image = My.Resources.Empower : Skill2Desc.Text = "Empower : Converts 30 hitpoints into 30 willpower and attacks a target with a basic attack."
            Skill3.Image = My.Resources.Silence : Skill3Desc.Text = "Silence : Causes all targets attacking the minstrel to stop attacking for 5 rounds. Costs 50 willpower. Causes a 10 round global cooldown of skills."
        End If
    End Sub
#End Region
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TabControl1.SelectedTab.Text = "Basics" Then
            TabControl1.SelectedTab = RaceTab
        ElseIf TabControl1.SelectedTab.Text = "Race" Then
            TabControl1.SelectedTab = ClassTab
            Button1.Text = "Ready"
        Else
            If Race_Human.Checked = True Then
                MainForm.PlayerRace = "Human"
            ElseIf Race_Dwarf.Checked = True Then
                MainForm.PlayerRace = "Dwarf"
            ElseIf Race_Gnome.Checked = True Then
                MainForm.PlayerRace = "Gnome"
            ElseIf Race_Elf.Checked = True Then
                MainForm.PlayerRace = "Elf"
            ElseIf Race_Halfelf.Checked = True Then
                MainForm.PlayerRace = "Half-elf"
            ElseIf Race_Halfling.Checked = True Then
                MainForm.PlayerRace = "Halfling"
            ElseIf Race_Goblin.Checked = True Then
                MainForm.PlayerRace = "Goblin"
            ElseIf Race_Troll.Checked = True Then
                MainForm.PlayerRace = "Troll"
            ElseIf Race_Orc.Checked = True Then
                MainForm.PlayerRace = "Orc"
            ElseIf Race_Halforc.Checked = True Then
                MainForm.PlayerRace = "Half-orc"
            ElseIf Race_Quickling.Checked = True Then
                MainForm.PlayerRace = "Quickling"
            ElseIf Race_Pixie.Checked = True Then
                MainForm.PlayerRace = "Pixie"
            ElseIf Race_Sprite.Checked = True Then
                MainForm.PlayerRace = "Sprite"
            End If
            If Priest.Checked = True Then
                MainForm.PlayerClass = "Priest" : MainForm.PlayerMaxCHA += 5 : MainForm.PlayerMaxINT += 3
            ElseIf Woodsman.Checked = True Then
                MainForm.PlayerClass = "Woodsman" : MainForm.PlayerMaxWIS += 5 : MainForm.PlayerMaxCON += 3
            ElseIf Gravedigger.Checked = True Then
                MainForm.PlayerClass = "Gravedigger" : MainForm.PlayerMaxINT += 5 : MainForm.PlayerMaxCHA += 3
            ElseIf Mageling.Checked = True Then
                MainForm.PlayerClass = "Mageling" : MainForm.PlayerMaxINT += 5 : MainForm.PlayerMaxCHA += 3
            ElseIf Hermit.Checked = True Then
                MainForm.PlayerClass = "Hermit" : MainForm.PlayerMaxWIS += 5 : MainForm.PlayerMaxINT += 3
            ElseIf Elementalist.Checked = True Then
                MainForm.PlayerClass = "Elementalist" : MainForm.PlayerMaxSTR += 5 : MainForm.PlayerMaxINT += 3
            ElseIf Headhunter.Checked = True Then
                MainForm.PlayerClass = "Headhunter" : MainForm.PlayerMaxSTR += 5 : MainForm.PlayerMaxCHA += 3
            ElseIf PLainsman.Checked = True Then
                MainForm.PlayerClass = "Plainsman" : MainForm.PlayerMaxSTR += 5 : MainForm.PlayerMaxDEX += 3
            ElseIf Pickpocket.Checked = True Then
                MainForm.PlayerClass = "Pickpocket" : MainForm.PlayerMaxDEX += 5 : MainForm.PlayerMaxLuc += 3
            ElseIf Page.Checked = True Then
                MainForm.PlayerClass = "Page" : MainForm.PlayerMaxCON += 5 : MainForm.PlayerMaxSTR += 3
            ElseIf Scout.Checked = True Then
                MainForm.PlayerClass = "Scout" : MainForm.PlayerMaxDEX += 5 : MainForm.PlayerMaxCHA += 3
            ElseIf Runescribe.Checked = True Then
                MainForm.PlayerClass = "Runescribe" : MainForm.PlayerMaxWIS += 5 : MainForm.PlayerMaxCHA += 3
            ElseIf Monk.Checked = True Then
                MainForm.PlayerClass = "Monk" : MainForm.PlayerMaxWIS += 5 : MainForm.PlayerMaxDEX += 3
            ElseIf Minstrel.Checked = True Then
                MainForm.PlayerClass = "Minstrel" : MainForm.PlayerMaxCHA += 5 : MainForm.PlayerMaxLuc += 3
            End If
            MainForm.PlayerSTR = Val(str.Text) : MainForm.PlayerMaxSTR += 5 + MainForm.PlayerSTR
            MainForm.PlayerDEX = Val(dex.Text) : MainForm.PlayerMaxDEX += 5 + MainForm.PlayerDEX
            MainForm.PlayerINT = Val(int.Text) : MainForm.PlayerMaxINT += 5 + MainForm.PlayerINT
            MainForm.PlayerWIS = Val(wis.Text) : MainForm.PlayerMaxWIS += 5 + MainForm.PlayerWIS
            MainForm.PlayerCON = Val(con.Text) : MainForm.PlayerMaxCON += 5 + MainForm.PlayerCON
            MainForm.PlayerCHA = Val(cha.Text) : MainForm.PlayerMaxCHA += 5 + MainForm.PlayerCHA
            MainForm.PlayerLUC = Val(luc.Text) : MainForm.PlayerMaxLuc += 5 + MainForm.PlayerLUC
            MainForm.PlayerHitpoints = Val(HealthValue.Text) : MainForm.PlayerWillpower = Val(WillpowerValue.Text)
            MainForm.PlayerCurHitpoints = Val(HealthValue.Text) : MainForm.PlayerCurWillpower = Val(WillpowerValue.Text)
            MainForm.PlayerName = CharacterName.Text
            Me.Hide()
            MainForm.TopMost = True
            MainForm.Show()
        End If
    End Sub
    Private Sub Initialize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Race_Halforc.Checked = True
        Woodsman.Checked = True
    End Sub
    Private Sub CheckKey(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Keys.Enter Then
            Button1_Click(0, EventArgs.Empty)
        ElseIf e.KeyValue = Keys.Escape Then
            MainForm.Close()
            Me.Close()
        End If
    End Sub
    Private Sub AppendButton(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab.Text = "Basics" Or TabControl1.SelectedTab.Text = "Race" Then
            Button1.Text = "Next"
        Else
            Button1.Text = "Ready"
        End If
    End Sub
End Class