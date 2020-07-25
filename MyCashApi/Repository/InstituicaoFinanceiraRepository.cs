using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class InstituicaoFinanceiraRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<InstituicaoFinanceiraModel> ListarInstitFinanceiras(int ifCodi)
        {
            SqlDataReader reader = null;
            List<InstituicaoFinanceiraModel> listaInstituicaoFinanceiraModel = new List<InstituicaoFinanceiraModel>();

            var query = @"SELECT * FROM InstituicaoFinanceira";

            if (ifCodi > 0)
            {
                query += " WHERE ifCodi = " + ifCodi;
            }

            query += " ORDER BY ifDesc";

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
                            byte[] binaryString;
                            if (reader[3].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[3]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }

                            var ret = new InstituicaoFinanceiraModel()
                            {
                                ifCodi = int.Parse(reader[0].ToString()),
                                ifDesc = reader[1].ToString(),
                                ifCod = reader[2].ToString(),
                                ifImg = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                                ifFlAt = Convert.ToBoolean(reader[4].ToString())
                            };

                            listaInstituicaoFinanceiraModel.Add(ret);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
            }

            return listaInstituicaoFinanceiraModel;
        }

        public string ManterInstitFinanc(InstituicaoFinanceiraModel instituicaoFinanceiraModel)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    //-> Convertendo a String do Base64 da imagem em array de bytes para salvar no BD.
                    string file = "";
                    if (instituicaoFinanceiraModel.ifImg.Length > 0)
                    {
                        string imgb64 = (instituicaoFinanceiraModel.ifImg);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (instituicaoFinanceiraModel.ifImg) = file;
                    }

                    if (instituicaoFinanceiraModel.ifCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[InstituicaoFinanceira]
                           SET [ifDesc] = '" + instituicaoFinanceiraModel.ifDesc +
                                  "', [ifCod] = '" + instituicaoFinanceiraModel.ifCod +
                                  "', [ifImg] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                  ", [ifFlAt] = " + (instituicaoFinanceiraModel.ifFlAt ? 1 : 0) +
                                  " WHERE [ifCodi] = " + instituicaoFinanceiraModel.ifCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[InstituicaoFinanceira]
                                       ([ifDesc],[ifCod],[ifImg],[ifFlAt])
                                 VALUES
                                       ( '" +
                                       instituicaoFinanceiraModel.ifDesc + "', '" +
                                       instituicaoFinanceiraModel.ifCod + "', " +
                                       (file.Length > 0 ? "convert(varbinary(max), '" + file + "')," : "NULL, ") +
                                       (instituicaoFinanceiraModel.ifFlAt ? 1 : 0) +
                                       ")";
                        command.ExecuteNonQuery();
                    }
                    
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