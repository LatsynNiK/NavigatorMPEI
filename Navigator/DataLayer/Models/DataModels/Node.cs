﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.DataModels
{
    public class Node
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        [ForeignKey("NextNode")]
        public int? NextNodeId { get; set; }
        public virtual Node NextNode { get; set; }
    }
}