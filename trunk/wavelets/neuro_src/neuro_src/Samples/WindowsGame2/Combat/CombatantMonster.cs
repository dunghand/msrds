#region File Description
//-----------------------------------------------------------------------------
// CombatantPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using RolePlayingGameData;
#endregion


using Microsoft.Xna.Framework.Input;
using WindowsGame2.AI;

namespace RolePlaying
{
    /// <summary>
    /// Encapsulates all of the combat-runtime data for a particular player combatant.
    /// </summary>
    public class CombatantMonster : CombatantEx
    {
        /// <summary>
        /// The Player object encapsulated by this object.
        /// </summary>
        private Player player;

        /// <summary>
        /// The Player object encapsulated by this object.
        /// </summary>
        public Player Player
        {
            get { return player; }
        }

        /// <summary>
        /// The character encapsulated by this combatant.
        /// </summary>
        public override FightingCharacter Character
        {
            get { return player as FightingCharacter; }
        }


        #region State Data



        #endregion


        #region Graphics Data


        /// <summary>
        /// Accessor for the combat sprite for this combatant.
        /// </summary>
        public override AnimatingSprite CombatSprite
        {
            get { return player.CombatSprite; }
        }


        #endregion


        #region Current Statistics


        /// <summary>
        /// The current statistics of this combatant.
        /// </summary>
        public override StatisticsValue Statistics
        {
            get { return player.CurrentStatistics + CombatEffects.TotalStatistics; }
        }


        /// <summary>
        /// Heals the combatant by the given amount.
        /// </summary>
        public override void Heal(StatisticsValue healingStatistics, int duration)
        {
            if (duration > 0)
            {
                CombatEffects.AddStatistics(healingStatistics, duration);
            }
            else
            {
                player.StatisticsModifiers += healingStatistics;
                player.StatisticsModifiers.ApplyMaximum(new StatisticsValue());
            }
            base.Heal(healingStatistics, duration);
        }


        /// <summary>
        /// Damages the combatant by the given amount.
        /// </summary>
        public override void Damage(StatisticsValue damageStatistics, int duration)
        {
            if (duration > 0)
            {
                CombatEffects.AddStatistics(new StatisticsValue() - damageStatistics,
                    duration);
            }
            else
            {
                player.StatisticsModifiers -= damageStatistics;
                player.StatisticsModifiers.ApplyMaximum(new StatisticsValue());
            }
            base.Damage(damageStatistics, duration);
        }


        /// <summary>
        /// Pay the cost for the given spell.
        /// </summary>
        /// <returns>True if the cost could be paid (and therefore was paid).</returns>
        public override bool PayCostForSpell(Spell spell)
        {
            // check the parameter.
            if (spell == null)
            {
                throw new ArgumentNullException("spell");
            }

            // check the requirements
            if (Statistics.MagicPoints < spell.MagicPointCost)
            {
                return false;
            }

            // reduce the player's magic points by the spell's cost
            player.StatisticsModifiers.MagicPoints -= spell.MagicPointCost;

            return true;
        }


        #endregion


        #region Initialization


        /// <summary>
        /// Construct a new CombatantPlayer object containing the given player.
        /// </summary>
        public CombatantMonster(Player player)
            : base()
        {
            // check the parameter
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }

            // assign the parameters
            this.player = player;

            // if the player starts dead, make sure the sprite is already "dead"
            if (IsDeadOrDying)
            {
                if (Statistics.HealthPoints > 0)
                {
                    State = RolePlayingGameData.Character.CharacterState.Idle;
                }
                else
                {
                    CombatSprite.PlayAnimation("Die");
                    CombatSprite.AdvanceToEnd();
                }
            }
            else
            {
                State = RolePlayingGameData.Character.CharacterState.Idle;
                CombatSprite.PlayAnimation("Idle");
            }
        }


        #endregion


        #region Updating


        OUTPUT _LastAIState = OUTPUT.STAND;
        OUTPUT _CurAIState = OUTPUT.STAND;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);




            if (CombatSprite.GetcurrentAnimation() != null && CombatSprite.GetcurrentAnimation().IsLoop == false)
            {
                if (CombatSprite.IsPlaybackComplete == false)
                    return;

                State = RolePlayingGameData.Character.CharacterState.Idle;
            }


            switch (State)
            {
                case RolePlayingGameData.Character.CharacterState.Walking:
                case RolePlayingGameData.Character.CharacterState.Idle:


                    WindowsGame2.AI.InputData input = GetInputData();

                    _CurAIState = MyLearning.I.GetNextAction(input);

                    break;
                

            }


            switch (_CurAIState)
            {
                case OUTPUT.STAND:
                    State = RolePlayingGameData.Character.CharacterState.Idle;

                    break;
                case OUTPUT.ATTACK:
                    State = RolePlayingGameData.Character.CharacterState.Attack;
                    break;
                case OUTPUT.FAR:
                    break;
            }


            

        }


        #endregion
    }
}
