using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_ORM.MySQL
{
	public class VeriIslemleri
	{
		// con nesnesi Web/App.Config dosyası içerisinde yer alan 'DatabaseContext' adlı ConnectionString ile çalıştırılır.
		private static readonly string con = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString;
		private static MySqlConnection sqlCon = new MySqlConnection(con);

		/// <summary>
		///     İçerisine aldığı <paramref name="cmd" /> nesnesini ExecuteNonQuery() metodu ile çalıştırır.
		/// </summary>
		/// <param name="cmd">ADO.NET MySqlCommand nesnesi</param>
		/// <returns>Veritabanında etkileşen satır sayısını döndürür</returns>
		private static int CommandCalistir(MySqlCommand cmd)
		{
			int sonuc = 0;
			try
			{
				if (sqlCon.State != ConnectionState.Open)
					sqlCon.Open();

				sonuc = cmd.ExecuteNonQuery();
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.Message);
			}
			finally
			{
				if (sqlCon.State == ConnectionState.Open)
					sqlCon.Close();
			}
			return sonuc;
		}


		/// <summary>
		///     İçerisine aldığı <paramref name="cmd" /> nesnesini ExecuteScalar() metodu ile çalıştırır.
		/// </summary>
		/// <param name="cmd">ADO.NET MySqlCommand nesnesi</param>
		/// <returns>Veritabanında etkileşen satır sayısını döndürür</returns>
		private static int CommandCalistirScalar(MySqlCommand cmd)
		{
			int sonuc = 0;
			try
			{
				if (sqlCon.State != ConnectionState.Open)
					sqlCon.Open();

				sonuc = Convert.ToInt32(cmd.ExecuteScalar());
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.Message);
			}
			finally
			{
				if (sqlCon.State == ConnectionState.Open)
					sqlCon.Close();
			}
			return sonuc;
		}

		/// <summary>
		///     İçerisine aldığı <paramref name="sql" /> parametresi ile Select sorguları için kullanılır.
		/// </summary>
		/// <param name="sql">SQL sorgu cümlesi.</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected DataTable VIVeriGetir(string sql)
		{
			DataTable dataSet = new DataTable();
			MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

			da.Fill(dataSet);
			return dataSet;
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile Select sorguları için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected DataTable VIVeriGetir(string tablo, string sart)
		{
			DataTable dataSet = new DataTable();
			MySqlDataAdapter da = new MySqlDataAdapter("Select * From " + tablo + " Where " + sart, con);

			da.Fill(dataSet);
			return dataSet;
		}


		/// <summary>
		///     İçerisine aldığı parametreler ile Select sorguları için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenen">Hangi sutun/sutunların getirilmesi isteniyorsa kullanılır</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected DataTable VIVeriGetir(string tablo, string sart, string istenen)
		{
			DataTable dataSet = new DataTable();
			MySqlDataAdapter da = new MySqlDataAdapter("Select " + istenen + " From " + tablo + " Where " + sart, con);

			da.Fill(dataSet);
			return dataSet;
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile maximum değer sorgusu için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="istenilen">En yüksek değeri istenilen nitelik</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIMaxDeger(string tablo, string istenilen)
		{
			MySqlCommand cmd = new MySqlCommand("Select Max(" + istenilen + ") From " + tablo, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile maximum değer sorgusu için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">En yüksek değeri istenilen nitelik</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIMaxDeger(string tablo, string sart, string istenilen)
		{
			MySqlCommand cmd = new MySqlCommand("Select Max(" + istenilen + ") From " + tablo + " Where " + sart, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile minimum değer sorgusu için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="istenilen">En düşük değeri istenilen nitelik</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIMinDeger(string tablo, string istenilen)
		{
			MySqlCommand cmd = new MySqlCommand("Select Min(" + istenilen + ") From " + tablo, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile minimum değer sorgusu için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">En düşük değeri istenilen nitelik</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIMinDeger(string tablo, string sart, string istenilen)
		{
			MySqlCommand cmd = new MySqlCommand("Select Min(" + istenilen + ") From " + tablo + " Where " + sart, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile veri sayısı sorgulamak için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIVeriSayisi(string tablo)
		{
			MySqlCommand cmd = new MySqlCommand("Select Count(*) From " + tablo, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile veri sayısı sorgulamak için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIVeriSayisi(string tablo, string sart)
		{
			MySqlCommand cmd = new MySqlCommand("Select Count(*) From " + tablo + " Where " + sart, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile veri sayısı sorgulamak için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">Veri sayısı istenilen nitelik</param>
		/// <returns>ADO.NET MySqlDataAdaptor sınıfı kullanarak Datatable olarak döndürür</returns>
		protected int VIVeriSayisi(string tablo, string sart, string istenen)
		{
			MySqlCommand cmd = new MySqlCommand("Select Count(" + istenen + ") From " + tablo + " Where " + sart, sqlCon);
			return CommandCalistirScalar(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile veri eklemek için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sutunlar">Reflection uygulanarak nesnedeki her bir özelliğin adı sutunlara yazılır</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="parametreler">Eklenecek değerler MySqlParameter tipinde komuta eklenir</param>
		/// <returns>ADO.NET MySqlCommand sınıfı kullanarak etkilenen satır kadar int değer döndürür</returns>
		protected int VIVeriEkle(string tablo, string sutunlar, string parametreBildirisi, MySqlParameter[] parametreler)
		{
			MySqlCommand cmd = new MySqlCommand("Insert Into " + tablo + "(" + sutunlar + ") Values(" + parametreBildirisi + ")", sqlCon);
			cmd.Parameters.AddRange(parametreler);
			return CommandCalistir(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile tek bir niteliğin güncellenmesi için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="parametre">Güncellenecek değer MySqlParameter tipinde komuta eklenir</param>
		/// <returns>ADO.NET MySqlCommand sınıfı kullanarak etkilenen satır kadar int değer döndürür</returns>
		protected int VIVeriGuncelle(string tablo, string parametreBildirisi, string sart, MySqlParameter parametre)
		{
			MySqlCommand cmd = new MySqlCommand("Update " + tablo + " Set " + parametreBildirisi + " Where " + sart, sqlCon);
			cmd.Parameters.Add(parametre);
			return CommandCalistir(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile veri güncellemek için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="parametreler">Güncellenecek değerler MySqlParameter tipinde komuta eklenir</param>
		/// <returns>ADO.NET MySqlCommand sınıfı kullanarak etkilenen satır kadar int değer döndürür</returns>
		protected int VIVeriGuncelle(string tablo, string parametreBildirisi, string sart, MySqlParameter[] parametreler)
		{
			MySqlCommand cmd = new MySqlCommand("Update " + tablo + " Set " + parametreBildirisi + " Where " + sart, sqlCon);
			cmd.Parameters.AddRange(parametreler);
			return CommandCalistir(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile verinin silinmesi için kullanılır.
		/// </summary>
		/// <param name="tablo">Etkileşimdeki tablonun adı</param>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>ADO.NET MySqlCommand sınıfı kullanarak etkilenen satır kadar int değer döndürür</returns>
		protected int VIVeriSil(string tablo, string sart)
		{
			MySqlCommand cmd = new MySqlCommand("Delete From " + tablo + " Where " + sart, sqlCon);
			return CommandCalistir(cmd);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile prosedür çağırılması için kullanılır.
		/// </summary>
		/// <param name="prosedureAdi">Etkileşimdeki prosedürün adı</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="parametreler">Prosedüre uygulanacak değerler MySqlParameter tipinde komuta eklenir</param>
		/// <returns>ADO.NET MySqlDataAdapter sınıfı kullanarak Datable döndürür</returns>
		protected DataTable VIProsedurCagir(string prosedureAdi, string parametreBildirisi, MySqlParameter[] parametreler)
		{
			DataTable dataSet = new DataTable();
			MySqlCommand cmd = new MySqlCommand(prosedureAdi, sqlCon);
			cmd.Parameters.AddRange(parametreler);
			cmd.CommandType = CommandType.StoredProcedure;

			MySqlDataAdapter da = new MySqlDataAdapter();
			da.SelectCommand = cmd;

			da.Fill(dataSet);
			return dataSet;
		}
	}
}
