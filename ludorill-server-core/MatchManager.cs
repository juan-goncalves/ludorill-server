﻿using System;
using System.Collections.Generic;
using System.Text;
using ludorill_server_core.Exceptions;

namespace ludorill_server_core
{
    class MatchManager
    {
        private List<Match> matches = new List<Match>();
        private int matchIdSequence = 0;

        public Match CreateMatch(Player creator, string matchName)
        {
            if (IsAlreadyInAMatch(creator))
                throw new PlayerAlreadyInGameException();
            
            Match m = new Match(matchIdSequence++, matchName);
            m.Join(creator);
            matches.Add(m);
            return m;
        }

        public Match JoinMatch(int matchId, Player p)
        {
            if (IsAlreadyInAMatch(p))
                throw new PlayerAlreadyInGameException();         
            
            // Esta llamada puede causar un ArgumentException si no consigue la partida
            Match m = FindMatchBy(matchId);
            m.Join(p);
            return m;
        }

        /*
         * Indica si un jugador ya pertenece a alguna partida.
         */
        public bool IsAlreadyInAMatch(Player p)
        {
            foreach(Match m in matches)
            {
                if (m.Has(p))
                    return true;
            }

            return false;
        }

        public Match FindMatchBy(int matchId)
        {
            foreach (Match m in matches)
            {
                if (m.id == matchId)
                    return m;
            }

            throw new ArgumentException("Invalid match id");
        }

        public Match FindMatchBy(Player p)
        {
            foreach (Match m in matches)
            {
                if (m.Has(p))
                    return m;
            }

            throw new ArgumentException("Player is not in a match");
        }

        public string GetAvailableMatches()
        {
            string result = "";
            List<string> pieces = new List<string>();

            foreach (Match m in matches)
            {
                pieces.Add(string.Format("{0}-{1}-{2}", m.id, m.GetPlayers().Count, m.name));
            }

            result = string.Join(',', pieces);
            return result;
        }

        public void CloseMatch(int matchId)
        {
            for (int i=0; i < matches.Count; i++)
            {
                if (matches[i].id == matchId)
                {
                    matches.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
