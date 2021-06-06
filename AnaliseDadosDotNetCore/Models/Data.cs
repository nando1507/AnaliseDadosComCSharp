using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnaliseDadosDotNetCore.Models
{
    public class Data
    {
        public DateTime DtIni { get; set; }
        public DateTime DtFim { get; set; }


        public string tempo()
        {
            return $"Tempo Decorrido: {DtIni.Subtract(DtFim)}";
        }

        public string rDtIni()
        {
            return DtIni.ToString($"dd/MM/yyyy HH:mm:ss");
        }
        public string rDtFim()
        {
            return DtFim.ToString($"dd/MM/yyyy HH:mm:ss");
        }

    }
}
