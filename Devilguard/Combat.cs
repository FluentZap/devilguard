using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Devilguard
{

    class Combat_Slot
    {
        public Actor_Type Actor;
        public int Initiative;

        public Combat_Slot(Actor_Type actor, int initiative)
        {
            Actor = actor;
            Initiative = initiative;
        }


    }

    enum Listof_MoveMode
    {
        Off,
        Waitingforcommand,
        Moving
    }

    enum Listof_AttackMode
    {
        Off,
        Waitingforcommand,
        Moving
    }

    class Combat
    {
        public List<Combat_Slot> InitiativeList = new List<Combat_Slot>();
        public Actor_Type CurrentTurn;
        public bool Advancing;

        public Listof_MoveMode MoveMode;
        public Listof_AttackMode AttackMode;

        public void AdvanceToTurn()
        {
            if (InitiativeList.Count > 0)
            {
                CurrentTurn = null;
                while (CurrentTurn == null)
                {
                    foreach (var item in InitiativeList)
                    {
                        item.Initiative += item.Actor.Stats.Speed;
                        //Need to add tie detector!
                        if (item.Initiative >= 1000)
                        {
                            CurrentTurn = item.Actor;
                            CurrentTurn.NewTurnRefresh();
                            item.Initiative -= 1000;
                            break;
                        }
                    }
                }
            }
        }

        public List<Actor_Type> GetCombatOrder(int amount)
        {
            List<Combat_Slot> order = new List<Combat_Slot>();
            List<Actor_Type> combatlist = new List<Actor_Type>();

            foreach (var actor in InitiativeList)
            {
                order.Add(new Combat_Slot(actor.Actor, actor.Initiative));
            }

            int slots = 0;

            while (slots < amount)
            {
                foreach (var item in order)
                {
                    item.Initiative += item.Actor.Stats.Speed;
                    if (item.Initiative >= 1000)
                    {
                        combatlist.Add(item.Actor);
                        item.Initiative -= 1000;
                        slots++;
                    }
                }
            }
            return combatlist;
        }


        public void Remove_Actor(Actor_Type actor)
        {
            foreach (var item in InitiativeList)
            {
                if (item.Actor == actor)
                {
                    InitiativeList.Remove(item);
                    break;
                }
            }
        }

    }

    public class AI_Combat_Move
    {



    }



}