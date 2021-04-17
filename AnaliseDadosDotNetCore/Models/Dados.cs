using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnaliseDadosDotNetCore.Models
{
    public class Dados
    {
        public Int64 ID { get; set; }
        public DateTime Updated { get; set; }
        public Int64 Confirmed { get; set; }
        public Int64 ConfirmedChange { get; set; }
        public Int64 Deaths { get; set; }
        public Int64 DeathsChange { get; set; }
        public Int64 Recovered { get; set; }
        public Int64 RecoveredChange { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
        public string Country_Region { get; set; }
        public string AdminRegion1 { get; set; }
        public string AdminRegion2 { get; set; }
        
    }
}
