using CollectionTrackerAPI.Models;
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

        
        public ConditionViewModel Condition { get; set; }

        [Required]
        public int ConditionId { get; set; }

        public DateTime FabricationDate { get; set; }

        public CategoryViewModel Category { get; set; }

        public int CategoryId { get; set; }

        public BrandViewModel Brand { get; set; }

        public int BrandId { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public CollectionUser CollectionUser { get; set; }
    }
}
