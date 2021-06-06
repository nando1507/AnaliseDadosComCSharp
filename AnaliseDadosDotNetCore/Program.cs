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
        public static string nmTabela = "TbCoronaVirus";


        static void Main(string[] args)
        {

            Data dtExec = new Data();
            dtExec.DtIni = DateTime.Now;
            Console.WriteLine("Inicio Execução: " + dtExec.rDtIni());
            Boolean continua = true;
            string url = @"https://media.githubusercontent.com/media/microsoft/Bing-COVID-19-Data/master/data/Bing-COVID19-Data.csv";
            StreamReader sr = null;
            List<TbCoronaVirus> lista = new List<TbCoronaVirus>();
            DataTable dtable = null;
            conn();

            #region Carrega Site
            Data dtSite = new Data();
            dtSite.DtIni = DateTime.Now;
            Console.WriteLine("Inicio Carrega Site: " + dtSite.rDtIni());
            sr = carregaSite(url);
            dtSite.DtFim = DateTime.Now;
            Console.WriteLine("Fim Carrega Site: " + dtSite.rDtFim());
            Console.WriteLine(dtSite.tempo());
            #endregion

            #region Lista
            Data dtLista = new Data();
            dtLista.DtIni = DateTime.Now;
            Console.WriteLine("Inicio carrega Lista: " + dtLista.rDtIni());
            lista = listar(sr);
            dtLista.DtFim = DateTime.Now;
            Console.WriteLine("Fim carrega Lista: " + dtLista.rDtFim());
            Console.WriteLine(dtLista.tempo());
            #endregion

            #region DataTable
            Data dtTable = new Data();
            dtTable.DtIni = DateTime.Now;
            Console.WriteLine("Inicio carrega tabela de dados: " + dtTable.rDtIni());
            dtable = ListToDataTable(lista);
            dtable.TableName = nmTabela;
            dtTable.DtFim = DateTime.Now;
            Console.WriteLine("Fim carrega tabela de dados: " + dtTable.rDtFim());
            Console.WriteLine(dtTable.tempo());
            #endregion

            #region Carga Banco

            Data DtTruncate = new Data();

            DtTruncate.DtIni = DateTime.Now;
            Console.WriteLine("Inicio Limpa tabela: " + DtTruncate.rDtIni());
            if (LimpaTabela(sqlConnection))
            {
                DtTruncate.DtFim = DateTime.Now;
                Console.WriteLine("Fim Limpa tabela: " + DtTruncate.rDtFim());
                Console.WriteLine(DtTruncate.tempo());


                Data DtCarga = new Data();
                DtCarga.DtIni = DateTime.Now;
                Console.WriteLine("Inicio Carga dados Banco: " + DtCarga.rDtIni());
                gravaBanco(dtable);
                DtCarga.DtFim = DateTime.Now;
                Console.WriteLine("Fim Carga dados banco: " + DtCarga.rDtFim());
                Console.WriteLine(DtCarga.tempo());
            }
            #endregion

            #region imprime Dados
            Data DtImprime = new Data();
            DtImprime.DtIni = DateTime.Now;
            Console.WriteLine("Imprime ultima data na tela: " + DtImprime.rDtIni());
            ImprimeDados(lista);
            DtImprime.DtFim = DateTime.Now;
            Console.WriteLine("Finaliza execução e calcula tempo levado: " + DtImprime.rDtFim());
            Console.WriteLine(DtImprime.tempo());
            #endregion

            #region a
            //int Option = 0;
            //while (0 == Option)
            //{
            //    try
            //    {
            //        Option = int.Parse(Console.ReadLine());
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Deve ser digitado um valor numerico");
            //        Option = int.Parse(Console.ReadLine());
            //    }
            //}

            //switch (Option)
            //{
            //    case 1:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        sr = carregaSite(url);
            //        continua = true;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //    case 2:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        if (sr == null)
            //        {
            //            lista = listar(carregaSite(url));
            //        }
            //        else
            //        {
            //            lista = listar(sr);
            //        }
            //        continua = true;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //    case 3:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        if (lista.Count == 0)
            //        {
            //            dtable = ListToDataTable(listar(carregaSite(url)));
            //        }
            //        else
            //        {
            //            dtable = ListToDataTable(lista);
            //        }
            //        continua = true;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //    case 4:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        if (dtable == null)
            //        {
            //            if (LimpaTabela(sqlConnection))
            //            {
            //                gravaBanco(ListToDataTable(listar(carregaSite(url))));
            //            }
            //        }
            //        else
            //        {
            //            if (LimpaTabela(sqlConnection))
            //            {
            //                gravaBanco(dtable);
            //            }
            //        }
            //        continua = true;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //    case 5:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        if (lista.Count == 0)
            //        {
            //            ImprimeDados(listar(carregaSite(url)));
            //        }
            //        else
            //        {
            //            ImprimeDados(lista);
            //        }
            //        continua = true;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //    default:
            //        dt[0] = DateTime.Now;
            //        tempo(dt);
            //        continua = false;
            //        dt[1] = DateTime.Now;
            //        tempo(dt);
            //        Console.ReadKey();
            //        break;
            //}

            //}
            #endregion
            dtExec.DtFim = DateTime.Now;
            Console.WriteLine("Fim Execução: " + dtExec.rDtFim());
            Console.WriteLine(dtExec.tempo());
            Console.WriteLine();
            Console.ReadKey();
        }
        public static void ImprimeDados(List<TbCoronaVirus> lstDados)
        {
            //Console.Clear();
            var vetor = lstDados.Select(s => s.CountryRegion).Distinct().OrderBy(o => o).ToArray();

            var data = lstDados.Select(s => new { s.CountryRegion, s.Updated }).GroupBy(g => g.CountryRegion).ToList();

            Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
            for (int i = 0; i < data.Count; i++)
            {
                dict.Add(
                    data[i].Select(s => s.CountryRegion).FirstOrDefault(),
                    data[i].Select(s => s.Updated).Max(m => m)
                    );
            }
            //string data = lstDados.Select(s => s.Updated).Where(w => w.Equals(item)).Max().ToString();//.Where(w => w.CountryRegion.Equals(item)).Max();
            List<TbCoronaVirus> dados = new List<TbCoronaVirus>();
            TbCoronaVirus coronaVirus = new TbCoronaVirus();
            foreach (var item in dict)
            {
                coronaVirus = lstDados.Where(w => w.CountryRegion.Contains(item.Key)).Where(w => w.Updated.Equals(item.Value)).OrderBy(O => O.Confirmed).FirstOrDefault();
                dados.Add(coronaVirus);
            }
            // Console.Clear();
            Console.WriteLine("CountryRegion".PadRight(40, ' ') + " | " +
                        "Deaths".PadLeft(20, ' ').Replace(",", ".") + " | " +
                        "Confirmed".PadLeft(20, ' ').Replace(",", ".") + " | " +
                        "Recovered".PadLeft(20, ' ').Replace(",", ".") + " | " +
                        "Updated".PadRight(20, ' '));
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 120)));
            Console.WriteLine("");
            int perc = 0;
            foreach (var item in dados.OrderByDescending(o => o.Confirmed))
            {
                perc++;
                //ConsoleUtility.WriteProgressBar(dados.Count / perc, true);
                CultureInfo cultureinfo = new CultureInfo("PT-BR");
                int pad = 20;
                Console.WriteLine(
                        item.CountryRegion.PadRight(pad + pad, ' ') + " | " +
                        item.Deaths.ToString("#,###").PadLeft(pad, ' ').Replace(",", ".") + " | " +
                        item.Confirmed.ToString("#,###").PadLeft(pad, ' ').Replace(",", ".") + " | " +
                        item.Recovered.ToString("#,###").PadLeft(pad, ' ').Replace(",", ".") + " | " +
                        item.Updated.ToString("dd/MM/yyyy").PadRight(pad, ' ')
                    );
            }
            Console.WriteLine("");
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 120)));
            Console.WriteLine("");
        }
        public static void conn()
        {
            // Console.Clear();
            ServiceCollection services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

            //string sqlConnection = builder.Sources.

            sqlConnection = config.GetConnectionString("DefaultConnection");
        }
        public static List<TbCoronaVirus> listar(StreamReader reader)
        {
            // Console.Clear();
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
                    dados.Latitude = decimal.TryParse(linha[8]?.Trim().Replace(".", ","), out fnumber) ? fnumber : 0;
                    dados.Longitude = decimal.TryParse(linha[9]?.Trim().Replace(".", ","), out fnumber) ? fnumber : 0;
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
            // Console.Clear();
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
            // Console.Clear();
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
            // Console.Clear();
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
            // Console.Clear();
            WebClient client = new WebClient();
            Uri uri = new Uri(url);
            Stream myStream = client.OpenRead(uri);
            Thread.Sleep(5000);
            Console.WriteLine("Acessa o Site");
            StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
            return sr;
        }
    }
}
