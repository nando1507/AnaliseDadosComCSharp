using System;
using System.Net;
using System.IO;
using AnaliseDadosDotNetCore.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Configuration;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AnaliseDadosDotNetCore.DAL;
using System.Text;
using System.Reflection;
using System.Linq;

namespace AnaliseDadosDotNetCore
{
    class Program
    {
        public readonly DbContext _context;
        public static string sqlConnection;

        static void Main(string[] args)
        {
            //Começo aplicação
            DateTime[] dt = new DateTime[2];
            dt[0] = DateTime.Now;
            Console.WriteLine($"{dt[0]}");

            string url = @"https://media.githubusercontent.com/media/microsoft/Bing-COVID-19-Data/master/data/Bing-COVID19-Data.csv";
            conn();
            //Função de Carga dos Dados
            List<TbCoronaVirus> lstDados = listar(carregaSite(url));
            DataTable dtable = ListToDataTable(lstDados);

            if (LimpaTabela(sqlConnection))
            {
                gravaBanco(dtable);
            }

            var vetor = lstDados.Select(s => s.CountryRegion).Distinct().OrderBy(o => o).ToArray();


            //var AtuDados;
            //List<[ string, DateTime]> AtuDados = new List<[ string, DateTime]>();
            //foreach (var item in vetor)
            //{
            //    string data = lstDados.Select(s => s.Updated).Where(w => w.Equals(item)).Max().ToString();//.Where(w => w.CountryRegion.Equals(item)).Max();

            //    AtuDados.Add(item, data);

            //}

            dt[1] = DateTime.Now;
            Console.WriteLine($"{dt[1]}");
            Console.WriteLine($"{dt[1].Subtract(dt[0])}");
            Console.ReadKey();
        }

        public static void conn()
        {
            ServiceCollection services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

            //string sqlConnection = builder.Sources.

            sqlConnection = config.GetConnectionString("DefaultConnection");
        }

        public static List<TbCoronaVirus> listar(StreamReader reader)
        {
            CultureInfo cultureinfo = new CultureInfo("en-us");
            List<TbCoronaVirus> lst = new List<TbCoronaVirus>();
            int index = 1;
            while (!reader.EndOfStream)
            {
                string[] linha = reader.ReadLine().Replace("'", "`").Split(',');

                if (linha.Length == 15 && Char.IsNumber(linha[0][0]))
                {
                    TbCoronaVirus dados = new TbCoronaVirus();
                    long inumber;
                    decimal fnumber;

                    dados.IdIndex = index;
                    dados.Id = long.Parse(linha[0]?.Trim());
                    dados.Updated = DateTime.Parse(linha[1]?.Trim(), cultureinfo);
                    dados.Confirmed = long.Parse(linha[2]?.Trim());
                    dados.ConfirmedChange = long.TryParse(linha[3]?.Trim(), out inumber) ? inumber : 0;
                    dados.Deaths = long.TryParse(linha[4]?.Trim(), out inumber) ? inumber : 0;
                    dados.DeathsChange = long.TryParse(linha[5]?.Trim(), out inumber) ? inumber : 0;
                    dados.Recovered = long.TryParse(linha[6]?.Trim(), out inumber) ? inumber : 0;
                    dados.RecoveredChange = long.TryParse(linha[7]?.Trim(), out inumber) ? inumber : 0;
                    dados.Latitude = decimal.TryParse(linha[8]?.Trim(), out fnumber) ? fnumber : 0;
                    dados.Longitude = decimal.TryParse(linha[9]?.Trim(), out fnumber) ? fnumber : 0;
                    dados.Iso2 = linha[10]?.Trim();
                    dados.Iso3 = linha[11]?.Trim();
                    dados.CountryRegion = linha[12]?.Trim();
                    dados.AdminRegion1 = linha[13]?.Trim();
                    dados.AdminRegion2 = linha[14]?.Trim();

                    lst.Add(dados);
                    index++;
                }
            }

            Console.WriteLine($@"Foram localizados {string.Format(lst.Count.ToString(), "C")} Registros no arquivo");
            return lst;
        }

        public static void gravaBanco(DataTable dtreader)
        {
            //Começo da Carga
            DateTime[] dt = new DateTime[2];
            dt[0] = DateTime.Now;
            Console.WriteLine("Inicio Importação");
            Console.WriteLine($"{dt[0]}");
            //conexão
            SqlConnection cn = new SqlConnection(sqlConnection);
            cn.Open();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default);
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.BatchSize = 1000000;
            bulkCopy.EnableStreaming = true;
            bulkCopy.DestinationTableName = "dbo.TbCoronaVirus";
            bulkCopy.WriteToServer(dtreader);
            cn.Close();
            //Fim da Carga
            dt[1] = DateTime.Now;
            Console.WriteLine("Fim Importação");
            Console.WriteLine($"{dt[1]}");
            Console.WriteLine($"{dt[1].Subtract(dt[0])}");
        }

        public static Boolean LimpaTabela(string cn)
        {
            Boolean status = false;
            try
            {
                SqlConnection sqlconnection = new SqlConnection(cn);
                if (sqlconnection.State != ConnectionState.Open)
                {
                    sqlconnection.Open();
                }
                SqlCommand trunc = new SqlCommand(@"Truncate Table TbCoronaVirus", sqlconnection);
                trunc.ExecuteNonQuery();
                status = true;
                sqlconnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Message Erro: {ex.Message}");
                Console.WriteLine($@"ex.StackTrace Erro: {ex.StackTrace}");
            }
            return status;
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        static StreamReader carregaSite(string url)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(url);
            Stream myStream = client.OpenRead(uri);
            Thread.Sleep(5000);
            Console.WriteLine("Exibe Dados: ");
            StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
            return sr;
        }
    }
}
