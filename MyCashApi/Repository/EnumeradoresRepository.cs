using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class EnumeradoresRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public EnumeradoresModel ListarEnumeradores()
        {
            SqlDataReader reader = null;
            EnumeradoresModel enumeradoresModel = new EnumeradoresModel();

            var query = @"SELECT * FROM Enumeradores";

            using (SqlConnection con = new SqlConnection(strConn.ToString()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            byte[] binaryString;
                            binaryString = (byte[])reader[5]; //-> Convertendo string novamente em byte[].

                            var ret = new EnumeradoresModel()
                            {
                                enuCod = int.Parse(reader[0].ToString()),
                                versao = reader[1].ToString(),
                                dataVersao = Convert.ToDateTime(reader[2].ToString()),
                                cttEmai = reader[3].ToString(),
                                cttFone = reader[4].ToString(),
                                imgApp = Encoding.Default.GetString(binaryString),
                                siteUrl = reader[6].ToString()
                            };

                            enumeradoresModel = ret;
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

            return enumeradoresModel;
        }

        public string ManterEnumeradores(EnumeradoresModel enumeradoresModel)
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
                    if (enumeradoresModel.imgApp.Length > 0)
                    {
                        string imgb64 = (enumeradoresModel.imgApp);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (enumeradoresModel.imgApp) = file;
                    }

                    command.CommandText =
                        @"UPDATE [dbo].[Enumeradores]
                           SET [versao] = '" + enumeradoresModel.versao +
                              "', [dataVersao] = '" + enumeradoresModel.dataVersao.ToString("yyyy-MM-dd HH:mm:ss") +
                              "', [cttEmail] = '" + enumeradoresModel.cttEmai +
                              "', [cttFone] = '" + enumeradoresModel.cttFone +
                              "', [imgApp] = convert(varbinary(max), '" + file + "')" +
                              ", [siteUrl] = '" + enumeradoresModel.siteUrl +
                              "' WHERE [enuCod] = 1";
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