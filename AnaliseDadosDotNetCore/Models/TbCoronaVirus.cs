using AnaliseDadosDotNetCore.DAL.Context;
using AnaliseDadosDotNetCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace AnaliseDadosDotNetCore.Models
{
    [Table("TbCoronaVirus")]
    public class TbCoronaVirus
    {
        public int IdIndex { get; set; }
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public long Confirmed { get; set; }
        public long ConfirmedChange { get; set; }
        public long Deaths { get; set; }
        public long DeathsChange { get; set; }
        public long Recovered { get; set; }
        public long RecoveredChange { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public string CountryRegion { get; set; }
        public string AdminRegion1 { get; set; }
        public string AdminRegion2 { get; set; }

    }
}
