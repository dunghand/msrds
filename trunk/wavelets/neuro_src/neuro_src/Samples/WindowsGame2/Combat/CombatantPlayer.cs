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
    public class CombatantPlayer : CombatantEx
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
        public CombatantPlayer(Player player)
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




        

        /// <summary>
        /// Update the player for this frame.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (CombatSprite.GetcurrentAnimation() != null && CombatSprite.GetcurrentAnimation().IsLoop == false)
            {
                if( CombatSprite.IsPlaybackComplete == false)
                    return;
            }

            Vector2 movePos = new Vector2(0, 0);

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            {
                _LastDir = -1;

                movePos += new Vector2(-1, 0);

            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
            {
                _LastDir = 1;
                movePos += new Vector2(1, 0);
            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
            {

                movePos += new Vector2(0, -1);

            }
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
            {

                movePos += new Vector2(0, 1);

            }


            if (movePos.Length()  != 0)
            {
                
                State = RolePlayingGameData.Character.CharacterState.Walking;
                
                    



                
                Position += movePos;
            }
            else
            {

                //if( this.CombatSprite.currentAnimation == null
                //    || this.CombatSprite.currentAnimation.IsLoop == false)

                State = RolePlayingGameData.Character.CharacterState.Idle;
            }
            
            

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Space))
            {
                State = RolePlayingGameData.Character.CharacterState.Hit;

                //_Player.CombatSprite.PlayAnimation("Hit");
            }


            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.LeftControl))
            {

                
                State = RolePlayingGameData.Character.CharacterState.Attack;
                //_Player.CombatSprite.PlayAnimation("Hit");
            }



        }


        Vector2 _OldPos = new Vector2(-1,-1);

        

        Vector2 _OldTargetDist = new Vector2(-1,-1);


        Character.CharacterState _OldState;

        MyLearning _Learning = new MyLearning();
        public override void OnChangeState(Character.CharacterState State)
        {


            if (_TargetObject == null)
                return;

            if (_OldTargetDist.X == -1)
            {
                _OldTargetDist = _TargetObject.Position - Position;

                _OldState = State;

                return;
            }

            switch (State)
            {
                case RolePlayingGameData.Character.CharacterState.Idle:

                    if (_OldState == RolePlayingGameData.Character.CharacterState.Idle)
                        return;

                    UpdateAIState(WindowsGame2.AI.OUTPUT.STAND);
                    
                    break;

                case RolePlayingGameData.Character.CharacterState.Walking:




                    Vector2 dist = _TargetObject.Position - Position;

                    //dist -= _OldTargetDist;

                    float fDist = dist.Length() - _OldTargetDist.Length();

                    if (fDist < -10)
                    {//�������

                        UpdateAIState(WindowsGame2.AI.OUTPUT.NEAR);

                    }
                    else if (10 < fDist)
                    {//  ����
                        UpdateAIState(WindowsGame2.AI.OUTPUT.FAR);
                        
                    }
                    else
                    {//  
                        return;
                    }

                    _OldTargetDist = _TargetObject.Position - Position;


                    break;
                
                case RolePlayingGameData.Character.CharacterState.Attack:
                    if (_OldState == State)
                        return;

                    UpdateAIState(OUTPUT.ATTACK);


                    break;
                case RolePlayingGameData.Character.CharacterState.Dodging:
                    if (_OldState == State)
                        return;

                    UpdateAIState(OUTPUT.DODGE);
                    break;
                    

                default:



                    //UpdateAIState()

                    break;

            }
            _OldState = State;
        }

        // DISTANCE,ES_ATTACK,MS_STAND,MS_NEAR,MS_FAR,OUTPUT , ES_STAND,ES_NEAR,ES_FAR,MS_ATTACK,



        public void UpdateAIState(WindowsGame2.AI.OUTPUT state)
        {

            WindowsGame2.AI.InputData input = GetInputData();

            _LastAction = state;
            //float 

            _Learning.AddData(input, state);
        }

        //public  void OnChangeState(WindowsGame2.AI.ACTION state)
        //{
            
        //}

        #endregion
    }
}
