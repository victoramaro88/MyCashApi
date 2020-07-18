using MyCashApi.Models;
using MyCashApi.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCashApi.Controllers
{
    public class PessoaController : ApiController
    {
        PessoaRepository _pessoaRepo = new PessoaRepository();

        [HttpGet]
        public List<PessoaModel> ListarPessoas()
        {
            var retorno = _pessoaRepo.ListarPessoas();
            return retorno;
        }

        //teste para Nalini:
        string strConn = "Data Source=SRVDESENV02;Initial Catalog=DB_MUNICIPIO_OPM;User ID=WS$MA;Password=ws$ma123;";

        [HttpGet]
        public object AlteraAreGeo()
        {
            var query = @"SELECT top 5 [OpmCbGeoAre].STAsText()
                            FROM [DB_MUNICIPIO_OPM].[dbo].[OPMCB]
                            WHERE OpmCbGeoAre is not null";

            DataTable dtRetorno = new DataTable();
            

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    command.CommandText = query;
                    DataTable dt = new DataTable();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dt.Load(reader);
                    if (dt.Rows.Count > 0)
                    {
                        string[] listaPoligonos;
                        listaPoligonos = new string[dt.Rows.Count];

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if(dt.Rows[i].ItemArray[0].ToString().Substring(0,4) == "POLY")
                            {
                                string a = dt.Rows[i].ItemArray[0].ToString().Substring(10);
                                a = a.Remove(a.Length - 2);
                                listaPoligonos[i] = a;
                                //var array = 
                            }
                            else
                            {

                            }

                            //listaPoligonos[i] = dt.Rows[i].ItemArray[0].ToString();
                        }

                        return listaPoligonos;
                    }
                    else
                    {
                        return "Não foi possível recuperar os registros da tabela.";
                    }
                }
                catch (Exception ex)
                {
                    return "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }
        }
    }
}
