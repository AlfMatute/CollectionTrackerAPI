﻿using CollectionTrackerAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollectionTrackerAPI.ViewModels
{
    public class CollectionViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionId { get; set; }

        [Required]
        [MaxLength(250)]
        public string ArticleName { get; set; }

        [Required]
        public DateTime AcquisitionDate { get; set; }

        public double ArticleCost { get; set; }

        [Required]
        public Condition Condition { get; set; }

        public DateTime FabricationDate { get; set; }

        public Category Category { get; set; }

        public Brand Brand { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public CollectionUser CollectionUser { get; set; }
    }
}
