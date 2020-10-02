using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class BandeiraCartaoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<BandeiraCartaoModel> ListarBandeiraCartao(int id)
        {
            SqlDataReader reader = null;
            List<BandeiraCartaoModel> listaBandeiraCartao = new List<BandeiraCartaoModel>();

            var query = @"SELECT * FROM BandeiraCartao";
            if(id > 0)
            {
                query += " WHERE bcCodi = " + id;
            }

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
                            if (reader[2].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[2]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }

                            var ret = new BandeiraCartaoModel()
                            {
                                bcCodi = int.Parse(reader[0].ToString()),
                                bcDesc = reader[1].ToString(),
                                bcImg = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                                bcFlAt = Convert.ToBoolean(reader[3].ToString())
                            };

                            listaBandeiraCartao.Add(ret);
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

            return listaBandeiraCartao;
        }

        public string AlteraStatusBandeiraCartao(int bcCodi, bool bcFlAt)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    if (bcCodi > 0)
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[BandeiraCartao] SET 
                                   [bcFlAt] = " + (bcFlAt ? 1 : 0) +
                             " WHERE [bcCodi] = " + bcCodi;
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

        public string ManterBandeiraCartao(BandeiraCartaoModel bandeiraCartaoModel)
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
                    if (bandeiraCartaoModel.bcImg != null && bandeiraCartaoModel.bcImg.Length > 0)
                    {
                        string imgb64 = (bandeiraCartaoModel.bcImg);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (bandeiraCartaoModel.bcImg) = file;
                    }

                    if (bandeiraCartaoModel.bcCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        command.CommandText =
                            @"UPDATE [dbo].[BandeiraCartao] SET 
                                   [bcDesc] = '" + bandeiraCartaoModel.bcDesc +
                                  "',[bcImg] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                  ",[bcFlAt] = " + (bandeiraCartaoModel.bcFlAt ? 1 : 0) +
                             " WHERE [bcCodi] = " + bandeiraCartaoModel.bcCodi;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                            @"INSERT INTO [dbo].[BandeiraCartao]
                                       ([bcDesc],[bcImg],[bcFlAt])
                                 VALUES
                                       ( '" +
                                            bandeiraCartaoModel.bcDesc + "', " +
                                            (file.Length > 0 ? "convert(varbinary(max), '" + file + "')," : "NULL, ") +
                                            (bandeiraCartaoModel.bcFlAt ? 1 : 0) +
                                       " )";
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