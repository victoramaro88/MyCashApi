using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyCashApi.Repository
{
    public class EnderecoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public EnderecoModel ListarEnderecoByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            EnderecoModel enderecoModel = new EnderecoModel();

            var query = @"SELECT * FROM Endereco
                            WHERE usuCodi = " + idUsuario;

            using (SqlConnection con = new SqlConnection(strConn.ToString()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    reader = com.ExecuteReader();

                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = new EnderecoModel()
                            {
                                endCodi = int.Parse(reader[0].ToString()),
                                usuCodi = int.Parse(reader[1].ToString()),
                                endNCEP = reader[2].ToString(),
                                endLogr = reader[3].ToString(),
                                endNume = reader[4].ToString(),
                                endCompl = reader[5].ToString(),
                                endBairr = reader[6].ToString(),
                                endCidad = reader[7].ToString(),
                                endEsta = reader[8].ToString()
                            };

                            enderecoModel = ret;
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
            }

            return enderecoModel;
        }

        public string InserirEndereco(EnderecoModel enderecoModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    //Inserindo na tabela Endereco.
                    command.CommandText =
                        @"INSERT INTO [dbo].[Endereco]
                                   ([usuCodi],[endNCEP],[endLogr],[endNume],[endCompl],[endBairr],[endCida],[endEsta])
                             VALUES
                                   (" +
                                        enderecoModel.usuCodi +
                                        ", '" + enderecoModel.endNCEP +
                                        "', '" + enderecoModel.endLogr +
                                        "', '" + enderecoModel.endNume +
                                        "', '" + enderecoModel.endCompl +
                                        "', '" + enderecoModel.endBairr +
                                        "', '" + enderecoModel.endCidad +
                                        "', '" + enderecoModel.endEsta +
                                        "')";
                    command.ExecuteNonQuery();

                    resp = "OK";
                }
                catch (Exception ex)
                {
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }

        public string AlterarEndereco(EnderecoModel enderecoModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    //Alterando a tabela Endereco.
                    command.CommandText =
                        @"UPDATE [dbo].[Endereco] SET
                              [endNCEP] = '" + enderecoModel.endNCEP +
                              "',[endLogr] = '" + enderecoModel.endLogr +
                              "',[endNume] = '" + enderecoModel.endNume +
                              "',[endCompl] = '" + enderecoModel.endCompl +
                              "',[endBairr] = '" + enderecoModel.endBairr +
                              "',[endCida] = '" + enderecoModel.endCidad +
                              "',[endEsta] = '" + enderecoModel.endEsta +
                         "' WHERE [endCodi] = " + enderecoModel.endCodi;

                    command.ExecuteNonQuery();

                    resp = "OK";
                }
                catch (Exception ex)
                {
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }
    }
}