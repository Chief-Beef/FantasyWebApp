using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FantasyWebApp.Models
{
    public class Player
    {
        //Visible in Index
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Team { get; set; }
        public string Grade { get; set; }

        //Not Visible in Index
        public string Overview { get; set; }
        //Game Log
        //Statistics
            //Season Statistics
            //Career Totals + Avgs
        
        
        //Projections

        public Player()
        {

        }

    }
}