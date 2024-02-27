﻿using System.ComponentModel.DataAnnotations;

namespace JobHunt.Services.EmployerAPI.Models
{
    public class Vacancy
    {
        public Guid Id { get; set; }
        [Required]
        public string PublishedBy { get; set; }
        [Required]
        public DateOnly PublishedDate { get; set;}
        [Required]
        public int NoOfVacancies { get; set; }
        [Required]
        public string MinimumQualification { get; set; }
        [Required]
        public string JobDescription { get; set; }
        [Required]
        public string ExperienceRequired { get; set; }
        [Required]
        public DateOnly LastDate { get; set; }
        [Required]
        public double MinimumSalary { get; set; }
        [Required]
        public double MaximumSalary { get; set; }
    }
}
