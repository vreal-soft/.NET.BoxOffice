﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoxOffice.Core.Data.Entities
{
    public class Spectacle
    {
        public Spectacle()
        {
            Tickets = new List<Ticket>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint TotalTicket { get; set; }
        public uint StartTime { get; set; }
        public uint EndTime { get; set; }

        public int AdminId { get; set; }
        public Admin Admin { get; set; }

        public IList<Ticket> Tickets { get; set; }
    }
}
