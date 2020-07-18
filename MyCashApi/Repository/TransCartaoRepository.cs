using MyCashApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MyCashApi.Repository
{
    public class TransCartaoRepository
    {
        string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConexaoBD"].ConnectionString;

        public List<TransCartaoModel> ListarTransacaoCartao(int transacaoCartaoCodi)
        {
            SqlDataReader reader = null;
            List<TransCartaoModel> listaTransCartaoModel = new List<TransCartaoModel>();

            var query = @"SELECT * FROM TransCartao";

            if (transacaoCartaoCodi > 0)
            {
                query += " WHERE tcCodi = " + transacaoCartaoCodi;
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
                            if (reader[9].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[9]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransCartaoModel()
                            {
                                tcCodi = int.Parse(reader[0].ToString()),
                                carCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tcValor = decimal.Parse(reader[5].ToString()),
                                tcNumParc = int.Parse(reader[6].ToString()),
                                tcDesc = reader[7].ToString(),
                                tcData = reader[8].ToString().Length > 0 ? Convert.ToDateTime(reader[8].ToString()) : Convert.ToDateTime("01/01/0001"),
                                tcComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                                tcStatus = bool.Parse(reader[10].ToString())
                            };

                            listaTransCartaoModel.Add(ret);
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

            return listaTransCartaoModel;
        }

        public List<TransCartaoModel> ListarTransacaoCartaoByIdUsuario(int idUsuario)
        {
            SqlDataReader reader = null;
            List<TransCartaoModel> listaTransCartaoModel = new List<TransCartaoModel>();

            var query = @"SELECT * FROM TransCartao WHERE usuCodi = " + idUsuario;

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
                            if (reader[9].ToString().Length > 0)
                            {
                                binaryString = (byte[])reader[9]; //-> Convertendo string novamente em byte[].
                            }
                            else
                            {
                                binaryString = null;
                            }
                            var ret = new TransCartaoModel()
                            {
                                tcCodi = int.Parse(reader[0].ToString()),
                                carCodi = int.Parse(reader[1].ToString()),
                                usuCodi = int.Parse(reader[2].ToString()),
                                ttCodi = int.Parse(reader[3].ToString()),
                                tdCodi = int.Parse(reader[4].ToString()),
                                tcValor = decimal.Parse(reader[5].ToString()),
                                tcNumParc = int.Parse(reader[6].ToString()),
                                tcDesc = reader[7].ToString(),
                                tcData = reader[8].ToString().Length > 0 ? Convert.ToDateTime(reader[8].ToString()) : Convert.ToDateTime("01/01/0001"),
                                tcComprov = binaryString != null ? Encoding.Default.GetString(binaryString) : "",
                                tcStatus = bool.Parse(reader[10].ToString())
                            };

                            listaTransCartaoModel.Add(ret);
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

            return listaTransCartaoModel;
        }

        public List<SaldoCartoesModel> RetornaSaldoCartoesByIdUsuario(int idUsuario)
        {
            List<SaldoCartoesModel> listaRetorno = new List<SaldoCartoesModel>();

            SqlDataReader reader = null;

            var query = @"SELECT --*
	                        TC.tcCodi, TC.usuCodi, TC.ttCodi, TC.tcValor,TC.tcNumPar, TC.tcData, TC.tcDesc, tcStatus
	                        , TC.carCodi, C.carDesc, C.carLimit, C.carDiaVenc, C.carSald, TD.tdDesc
                        FROM [mycash].[dbo].[TransCartao] AS TC
                        INNER JOIN [mycash].[dbo].[Cartao]  AS C ON C.carCodi = TC.carCodi
                        INNER JOIN [mycash].[dbo].[TipoDespesa] AS TD ON TD.tdCodi = TC.tdCodi
                        WHERE DATEPART(year, TC.tcData) >= DATEPART(year, GETDATE())
                        AND C.carFlAt = 1
                        AND TC.usuCodi = " + idUsuario;

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
                            var ret = new SaldoCartoesModel()
                            {
                                tcCodi = int.Parse(reader[0].ToString()),
                                usuCodi = int.Parse(reader[1].ToString()),
                                ttCodi = int.Parse(reader[2].ToString()),
                                tcValor = decimal.Parse(reader[3].ToString()),
                                tcNumPar = int.Parse(reader[4].ToString()),
                                tcData = DateTime.Parse(reader[5].ToString()),
                                tcDesc = reader[6].ToString(),
                                tcStatus = bool.Parse(reader[7].ToString()),
                                carCodi = int.Parse(reader[8].ToString()),
                                carDesc = reader[9].ToString(),
                                carLimit = decimal.Parse(reader[10].ToString()),
                                carDiaVenc = int.Parse(reader[11].ToString()),
                                carSald = decimal.Parse(reader[12].ToString()),
                                tdDesc = reader[13].ToString()
                            };

                            listaRetorno.Add(ret);
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

            return listaRetorno;
        }

        public string ManterTransacaoCartao(TransCartaoModel transCartaoModel, List<ParcelasTransCartaoModel> parcelas)
        {
            string resp = "";

            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction("TransCartaoTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    //-> Convertendo a String do Base64 da imagem em array de bytes para salvar no BD.
                    string file = "";
                    if (transCartaoModel.tcComprov.Length > 0)
                    {
                        string imgb64 = (transCartaoModel.tcComprov);
                        string ext = imgb64.Split('/')[1];
                        var extn = ext.Split(';')[0];
                        file = imgb64.Split(',')[1];
                        (transCartaoModel.tcComprov) = file;
                    }

                    if (transCartaoModel.tcCodi > 0) //-> Se vier ID, faz update, senão insere um novo.
                    {
                        // ->Recuperando o valor antigo e removendo-o do saldo do cartão
                        decimal valorAntigo = 0;
                        int tipoTransacaoAntiga = 0;
                        int diaVencimentoCartao = 0;
                        command.CommandText = @"SELECT TC.tcValor, TC.ttCodi, C.carDiaVenc FROM [mycash].[dbo].[TransCartao] AS TC
                                                INNER JOIN [mycash].[dbo].[Cartao] AS C ON C.carCodi = TC.carCodi
                                                WHERE tcCodi = " + transCartaoModel.tcCodi;
                        DataTable dt = new DataTable();
                        SqlDataReader reader;
                        reader = command.ExecuteReader();
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            valorAntigo = decimal.Parse(dt.Rows[0].ItemArray[0].ToString().ToLower());
                            tipoTransacaoAntiga = int.Parse(dt.Rows[0].ItemArray[1].ToString().ToLower());
                            diaVencimentoCartao = int.Parse(dt.Rows[0].ItemArray[2].ToString().ToLower());
                        }
                        else
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "Não foi possível recuperar o último valor da transação.";
                        }

                        //Removendo parcelas da tabela para adicionar novamente.
                        command.CommandText = @"DELETE FROM [dbo].[ParcelasTransCartao]
                                                WHERE tcCodi = " + transCartaoModel.tcCodi;
                        command.ExecuteNonQuery();

                        // ->Inserindo as Parcelas da Transação.
                        for (int i = 0; i < parcelas.Count; i++)
                        {
                            if (parcelas[i].ptcDataVenc.Day >= diaVencimentoCartao)
                            {
                                parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month + (i + 1), diaVencimentoCartao);
                            }
                            else
                            {
                                if (i != 0)
                                {
                                    parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month + i, diaVencimentoCartao);
                                }
                                else
                                {
                                    parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month, diaVencimentoCartao);
                                }
                            }

                            command.CommandText =
                                @"INSERT INTO [dbo].[ParcelasTransCartao]
                                           ([tcCodi],[ptcValorParc],[ptcDataVenc],[ptcStatus])
                                     VALUES ( " +
                                               transCartaoModel.tcCodi + ", " +
                                               parcelas[i].ptcValorParc.ToString().Replace(',', '.') + ", '" +
                                               parcelas[i].ptcDataVenc.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                               (parcelas[i].ptcStatus ? 1 : 0) +
                                           " )";
                            command.ExecuteNonQuery();
                        }

                        // ->Alterando o saldo do cartão, removendo o valor antigo da transação.
                        if (tipoTransacaoAntiga == transCartaoModel.ttCodi)
                        {
                            command.CommandText =
                                transCartaoModel.ttCodi == 2 ? // -> Se for 2 (Despesa) retira do saldo gasto, se for 1 (Receita), adiciona no saldo.
                                @"UPDATE [dbo].[Cartao]
                                SET [carSald] -= " + valorAntigo.ToString().Replace(',', '.') +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi
                            :
                                @"UPDATE [dbo].[Cartao]
                                SET [carSald] += " + valorAntigo.ToString().Replace(',', '.') +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi;
                        }
                        else
                        {
                            command.CommandText =
                                tipoTransacaoAntiga == 2 ?
                                @"UPDATE [dbo].[Cartao]
                                SET [carSald] -= " + valorAntigo.ToString().Replace(',', '.') +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi
                            :
                                @"UPDATE [dbo].[Cartao]
                                SET [carSald] += " + valorAntigo.ToString().Replace(',', '.') +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi;
                        }
                        command.ExecuteNonQuery();

                        // ->Alterando o registro da transação.
                        command.CommandText =
                            @"UPDATE [dbo].[TransCartao] SET 
                                       [carCodi] = " + transCartaoModel.carCodi +
                                      ",[usuCodi] = " + transCartaoModel.usuCodi +
                                      ",[ttCodi] = " + transCartaoModel.ttCodi +
                                      ",[tdCodi] = " + transCartaoModel.tdCodi +
                                      ",[tcValor] = " + transCartaoModel.tcValor.ToString().Replace(',', '.') +
                                      ",[tcNumPar] = " + transCartaoModel.tcNumParc +
                                      ",[tcDesc] = '" + transCartaoModel.tcDesc +
                                      "',[tcData] = '" + transCartaoModel.tcData.ToString("yyyy-MM-dd HH:mm:ss") +
                                      "',[tcComprov] = " + (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") +
                                      ",[tcStatus] = " + (transCartaoModel.tcStatus ? 1 : 0) +
                                 " WHERE [tcCodi] = " + transCartaoModel.tcCodi;
                        command.ExecuteNonQuery();

                        // ->Alterando o saldo do cartão, adicionando o valor novo da transação.
                        if (tipoTransacaoAntiga == transCartaoModel.ttCodi)
                        {
                            command.CommandText =
                            transCartaoModel.ttCodi == 2 ? // -> Se for 2 (Despesa) adiciona do saldo gasto, se for 1 (Receita), retira do saldo.
                            @"UPDATE [dbo].[Cartao]
                               SET [carSald] = [carSald] + " + transCartaoModel.tcValor.ToString().Replace(',', '.') +
                             " WHERE [carCodi] = " + transCartaoModel.carCodi
                            :
                            @"UPDATE [dbo].[Cartao]
                               SET [carSald] = [carSald] - " + transCartaoModel.tcValor.ToString().Replace(',', '.') +
                             " WHERE [carCodi] = " + transCartaoModel.carCodi;
                        }
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        // ->Inserindo a Transação
                        command.CommandText =
                            @"INSERT INTO [dbo].[TransCartao]
                                       ([carCodi],[usuCodi],[ttCodi],[tdCodi],[tcValor],[tcNumPar],[tcDesc],[tcData],[tcComprov],[tcStatus])
                                 VALUES
                                       ( " +
                                            transCartaoModel.carCodi + ", " +
                                            transCartaoModel.usuCodi + ", " +
                                            transCartaoModel.ttCodi + ", " +
                                            transCartaoModel.tdCodi + ", " +
                                            transCartaoModel.tcValor.ToString().Replace(',', '.') + ", " +
                                            transCartaoModel.tcNumParc + ", '" +
                                            transCartaoModel.tcDesc + "', '" +
                                            transCartaoModel.tcData.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                            (file.Length > 0 ? "convert(varbinary(max), '" + file + "')" : "NULL") + ", " +
                                            (transCartaoModel.tcStatus ? 1 : 0) +
                                       " )";
                        command.ExecuteNonQuery();

                        // ->Recuperando o último Id
                        int lastId = 0;
                        command.CommandText = "SELECT MAX([tcCodi]) AS lastID FROM [dbo].[TransCartao]";
                        DataTable dt = new DataTable();
                        SqlDataReader reader;
                        reader = command.ExecuteReader();
                        dt.Load(reader);
                        if (dt.Rows.Count > 0)
                        {
                            lastId = int.Parse(dt.Rows[0].ItemArray[0].ToString().ToLower());
                        }
                        else
                        {
                            return "Não foi possível recuperar o último código da tabela.";
                        }

                        // ->Retornando o dia de vencimento do Cartão.
                        int diaVencimentoCartao = 0;
                        command.CommandText = "SELECT carDiaVenc FROM [mycash].[dbo].[Cartao] WHERE carCodi = " + transCartaoModel.carCodi;
                        DataTable dt2 = new DataTable();
                        SqlDataReader reader2;
                        reader2 = command.ExecuteReader();
                        dt2.Load(reader2);
                        if (dt2.Rows.Count > 0)
                        {
                            diaVencimentoCartao = int.Parse(dt2.Rows[0].ItemArray[0].ToString().ToLower());
                        }
                        else
                        {
                            return "Não foi possível recuperar o dia de vencimento do Cartão.";
                        }

                        // ->Inserindo as Parcelas da Transação.
                        for (int i = 0; i < parcelas.Count; i++)
                        {
                            if (parcelas[i].ptcDataVenc.Day >= diaVencimentoCartao)
                            {
                                parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month + (i + 1), diaVencimentoCartao);
                            }
                            else
                            {
                                if (i != 0)
                                {
                                    parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month + i, diaVencimentoCartao);
                                }
                                else
                                {
                                    parcelas[i].ptcDataVenc = new DateTime(parcelas[i].ptcDataVenc.Year, parcelas[i].ptcDataVenc.Month, diaVencimentoCartao);
                                }
                            }

                            command.CommandText =
                                @"INSERT INTO [dbo].[ParcelasTransCartao]
                                           ([tcCodi],[ptcValorParc],[ptcDataVenc],[ptcStatus])
                                     VALUES ( " +
                                               lastId + ", " +
                                               parcelas[i].ptcValorParc.ToString().Replace(',', '.') + ", '" +
                                               parcelas[i].ptcDataVenc.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                               (parcelas[i].ptcStatus ? 1 : 0) +
                                           " )";
                            command.ExecuteNonQuery();
                        }

                        // ->Alterando o saldo da Inst. Fin. do Usuário.
                        if (transCartaoModel.ttCodi == 2)
                        {
                            command.CommandText =
                                @"UPDATE [dbo].[Cartao] SET 
                                    [carSald] = (SELECT [carSald] + " +
                                        transCartaoModel.tcValor.ToString().Replace(',', '.') +
                                        " AS Total FROM [mycash].[dbo].[Cartao] WHERE [carCodi] = " + transCartaoModel.carCodi + ") " +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi;
                            command.ExecuteNonQuery();
                        }
                        else if (transCartaoModel.ttCodi == 1)
                        {
                            command.CommandText =
                            @"UPDATE [dbo].[Cartao] SET 
                                    [carSald] = (SELECT [carSald] - " +
                                        transCartaoModel.tcValor.ToString().Replace(',', '.') +
                                        " AS Total FROM [mycash].[dbo].[Cartao] WHERE [carCodi] = " + transCartaoModel.carCodi + ") " +
                                 " WHERE [carCodi] = " + transCartaoModel.carCodi;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "Tipo de Transação inválida!";
                        }
                    }

                    transaction.Commit();
                    resp = "OK";
                    connection.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    resp = "Erro ao inserir no banco de dados: " + ex.GetType() +
                        " | Mensagem: " + ex.Message;
                }
            }

            return resp;
        }
    }
}