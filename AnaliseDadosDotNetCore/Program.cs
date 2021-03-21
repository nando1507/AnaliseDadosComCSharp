using System;
using System.Net;
using System.IO;
using AnaliseDadosDotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading;

namespace AnaliseDadosDotNetCore
{
    class Program : WebClient
    {

        static void Main(string[] args)
        {
            carregaSite();
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 20 * 60 * 1000;
            return w;
        }

        static async void carregaSite()
        {
            DateTime[] dt = new DateTime[2];
            dt[0] = DateTime.Now;

            Console.WriteLine($"{dt[0]}");
            string url = @"https://media.githubusercontent.com/media/microsoft/Bing-COVID-19-Data/master/data/Bing-COVID19-Data.csv";

            WebClient client = new WebClient();
            SqlConnection sqlconnection = new SqlConnection(@"Data Source = DESKTOP-JQKENAS; Initial Catalog = dbApi; Integrated Security = True");
            if (sqlconnection.State != ConnectionState.Open)
            {
                sqlconnection.Open();
            }
            SqlCommand trunc = new SqlCommand(@"Truncate Table TbCoronaVirus", sqlconnection);
            trunc.ExecuteNonQuery();
            //var dados = client.OpenRead(url);
            Uri uri = new Uri(url);
            Stream myStream = client.OpenRead(uri);
            Thread.Sleep(5000);
            Console.WriteLine("\nDisplaying Data :\n");
            StreamReader sr = new StreamReader(myStream);

            //SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlconnection);

            //Dados dados = new Dados();
            //DataTable dt = new DataTable(dados);
            //dt.


            int Count = 0;

            while (!sr.EndOfStream)
            {
                string[] linha = sr.ReadLine().Replace("'", "`").Split(',');

                if (linha.Length == 15 && Char.IsNumber(linha[0][0]))
                {



                    string sql = string.Empty;
                    sql = $@"INSERT INTO dbo.TbCoronaVirus ( 
                                                            ID, 
                                                            Updated,
                                                            Confirmed, 
                                                            ConfirmedChange,
                                                            Deaths, 
                                                            DeathsChange, 
                                                            Recovered, 
                                                            RecoveredChange, 
                                                            Latitude, 
                                                            Longitude, 
                                                            ISO2, 
                                                            ISO3, 
                                                            Country_Region, 
                                                            AdminRegion1, 
                                                            AdminRegion2 
                                                        ) VALUES (
                                                            @ID, 
                                                            @Updated,
                                                            @Confirmed, 
                                                            @ConfirmedChange,
                                                            @Deaths, 
                                                            @DeathsChange, 
                                                            @Recovered, 
                                                            @RecoveredChange, 
                                                            @Latitude, 
                                                            @Longitude, 
                                                            @ISO2, 
                                                            @ISO3, 
                                                            @Country_Region, 
                                                            @AdminRegion1, 
                                                            @AdminRegion2 
                                                            )";
                    SqlCommand cmd = new SqlCommand(sql, sqlconnection);
                    cmd.Parameters.AddWithValue(@"ID", string.IsNullOrEmpty(linha[0].Trim()) ? Convert.DBNull : float.Parse(linha[0].Trim()));
                    string data_atualizacao = linha[01].Trim().Substring(6, 4) + "-" + linha[01].Trim().Substring(0, 2) + "-" + linha[01].Trim().Substring(3, 2);
                    cmd.Parameters.AddWithValue(@"Updated", data_atualizacao);
                    cmd.Parameters.AddWithValue(@"Confirmed", string.IsNullOrEmpty(linha[2].Trim()) ? 0 : float.Parse(linha[2].Trim()));
                    cmd.Parameters.AddWithValue(@"ConfirmedChange", string.IsNullOrEmpty(linha[3].Trim()) ? 0 : float.Parse(linha[3].Trim()));
                    cmd.Parameters.AddWithValue(@"Deaths", string.IsNullOrEmpty(linha[4].Trim()) ? 0 : float.Parse(linha[4].Trim()));
                    cmd.Parameters.AddWithValue(@"DeathsChange", string.IsNullOrEmpty(linha[5].Trim()) ? 0 : float.Parse(linha[5].Trim()));
                    cmd.Parameters.AddWithValue(@"Recovered", string.IsNullOrEmpty(linha[6].Trim()) ? 0 : float.Parse(linha[6].Trim()));
                    cmd.Parameters.AddWithValue(@"RecoveredChange", string.IsNullOrEmpty(linha[7].Trim()) ? 0 : float.Parse(linha[7].Trim()));
                    cmd.Parameters.AddWithValue(@"Latitude", string.IsNullOrEmpty(linha[8].Trim()) ? Convert.DBNull : linha[8].Trim());
                    cmd.Parameters.AddWithValue(@"Longitude", string.IsNullOrEmpty(linha[9].Trim()) ? Convert.DBNull : linha[9].Trim());
                    cmd.Parameters.AddWithValue(@"ISO2", string.IsNullOrEmpty(linha[10].Trim()) ? Convert.DBNull : linha[10].Trim());
                    cmd.Parameters.AddWithValue(@"ISO3", string.IsNullOrEmpty(linha[11].Trim()) ? Convert.DBNull : linha[11].Trim());
                    cmd.Parameters.AddWithValue(@"Country_Region", string.IsNullOrEmpty(linha[12].Trim()) ? Convert.DBNull : linha[12].Trim());
                    cmd.Parameters.AddWithValue(@"AdminRegion1", string.IsNullOrEmpty(linha[13].Trim()) ? Convert.DBNull : linha[13].Trim());
                    cmd.Parameters.AddWithValue(@"AdminRegion2", string.IsNullOrEmpty(linha[14].Trim()) ? Convert.DBNull : linha[14].Trim());

                    cmd.ExecuteNonQuery();

                    //sr.ReadLine();
                    Count++;
                }
                Console.WriteLine(Count);

            }

            myStream.Close();
            sr.Close();


            dt[1] = DateTime.Now;

            Console.WriteLine($"{dt[1]}");
            Console.WriteLine($"{dt[1].Subtract(dt[0])}");
        }
    }
}
