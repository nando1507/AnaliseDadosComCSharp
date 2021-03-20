using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnaliseDadosDotNetCore.Models
{
    [Table("Dados")]
    public class Dados
    {
        public static Int64 ID { get; set; }
        public static DateTime Updated { get; set; }
        public static Int64 Confirmed { get; set; }
        public static Int64 ConfirmedChange { get; set; }
        public static Int64 Deaths { get; set; }
        public static Int64 DeathsChange { get; set; }
        public static Int64 Recovered { get; set; }
        public static Int64 RecoveredChange { get; set; }
        public static float Latitude { get; set; }
        public static float Longitude { get; set; }
        public static string ISO2 { get; set; }
        public static string ISO3 { get; set; }
        public static string Country_Region { get; set; }
        public static string AdminRegion1 { get; set; }
        public static string AdminRegion2 { get; set; }
        public ICollection<Dados> dados { get; set; }
    }
}
