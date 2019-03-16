using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace K_ORM.MSSQL
{
	public class TemelIslemler<T> : VeriIslemleri where T : class
	{
		private string tabloAdi;
		protected string TabloAdi { get { return tabloAdi; } set { tabloAdi = value; } }
		public TemelIslemler(string tabloAdi)
		{
			this.tabloAdi = tabloAdi;
		}

		/// <summary>
		///     İçerisine aldığı nesneyi Reflection işleminden geçirir ve komutlarda kullanılacak parametreler için değerlerini ve adlarını alır.
		/// </summary>
		/// <param name="nesne">Değerleri okunacak nesne</param>
		/// <returns>Komutlarda kullanılmak üzere XXXParameter dizisi tipinde değer döndürür</returns>
		protected SqlParameter[] ParametreOlustur(T nesne)
		{
			SqlParameter parameter;
			List<SqlParameter> param = new List<SqlParameter>();
			foreach (var prop in nesne.GetType().GetProperties())
			{
				// Virtual tanımlı özelliklerden parametre oluşturulmaz.
				if (prop.GetMethod.IsVirtual)
					continue;
				parameter = new SqlParameter(prop.Name, prop.GetValue(nesne, null));
				if (parameter.Value != null && parameter.Value.GetType() == typeof(string))
					parameter.DbType = DbType.String;

				param.Add(parameter);
			}
			return param.ToArray();
		}

		/// <summary>
		///     İçerisine aldığı nesneyi Reflection işleminden geçirir ve komutlarda kullanılacak parametreler için özelliklerin adlarını alır.
		/// </summary>
		/// <param name="nesne">Değerleri okunacak nesne</param>
		/// <returns>Komutlarda kullanılmak üzere string tipinde değer döndürür</returns>
		protected string ParametreBildirisiOlustur(T nesne)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var prop in nesne.GetType().GetProperties())
			{
				// Virtual tanımlı özelliklerden parametre oluşturulmaz.
				if (prop.GetMethod.IsVirtual)
					continue;
				sb.Append("@" + prop.Name + ",");
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		/// <summary>
		///     İçerisine aldığı nesneyi Reflection işleminden geçirir ve komutlarda kullanılacak parametreler için özelliklerin adlarını alır.
		/// </summary>
		/// <param name="nesne">Değerleri okunacak nesne</param>
		/// <returns>Komutlarda kullanılmak üzere string tipinde değer döndürür</returns>
		protected string ParametreBildirisiSutunlar(T nesne)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var prop in nesne.GetType().GetProperties())
			{
				if (prop.GetMethod.IsVirtual)
					continue;
				sb.Append(prop.Name + ",");
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		/// <summary>
		///     İçerisine aldığı nesneyi Reflection işleminden geçirir ve komutlarda kullanılacak parametreler için özelliklerin adlarını alır.
		/// </summary>
		/// <param name="nesne">Değerleri okunacak nesne</param>
		/// <returns>Komutlarda kullanılmak üzere string tipinde değer döndürür</returns>
		protected string ParametreBildirisiDuzenle(T nesne)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var prop in nesne.GetType().GetProperties())
			{
				if (prop.GetMethod.IsVirtual)
					continue;
				sb.Append(prop.Name + " = @" + prop.Name + ",");
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		/// <summary>
		///     İçerisine aldığı DataTable tipindeki verileri <typeparamref name="T"/> tipinde listeye dönüştürür.
		/// </summary>
		/// <param name="dt">Datatable tipinde veritabanından aldığı değerler</param>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		protected List<T> ListeyeCevir(DataTable dt)
		{
			var columnNames = dt.Columns.Cast<DataColumn>()
					.Select(c => c.ColumnName)
					.ToList();
			var properties = typeof(T).GetProperties();
			return dt.AsEnumerable().Select(row =>
			{
				var objT = Activator.CreateInstance<T>();
				foreach (var pro in properties)
				{
					if (columnNames.Contains(pro.Name))
					{
						PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
						pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
					}
				}
				return objT;
			}).ToList();
		}

		/// <summary>
		///     İçerisine aldığı string tipindeki SQL cümlesini <typeparamref name="T"/> tipinde listeye dönüştürür.
		/// </summary>
		/// <param name="sql">SQL sorgu cümlesi</param>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		public List<T> VeriGetirSQL(string sql)
		{
			DataTable dataTable = VIVeriGetir(sql);
			List<T> liste = ListeyeCevir(dataTable);
			return liste;
		}

		/// <summary>
		///     Belirtilen tabloya ait bütün verileri getirerek <typeparamref name="T"/> tipinde listeye dönüştürür.
		/// </summary>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		public List<T> VeriGetir()
		{
			DataTable dataTable = VIVeriGetir($"Select * From {tabloAdi}");
			List<T> liste = ListeyeCevir(dataTable);
			return liste;
		}

		/// <summary>
		///     Belirtilen tabloya ait şartları sağlayan verileri getirerek <typeparamref name="T"/> tipinde listeye dönüştürür.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		public List<T> VeriGetir(string sart)
		{
			DataTable dataTable = VIVeriGetir(tabloAdi, sart);
			List<T> liste = ListeyeCevir(dataTable);
			return liste;
		}

		/// <summary>
		///     Belirtilen tabloya ait istenilen sütunlardaki şartları sağlayan verileri getirerek <typeparamref name="T"/> tipinde listeye dönüştürür.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenen">Hangi sutun/sutunların getirilmesi isteniyorsa kullanılır</param>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		public List<T> VeriGetir(string sart, string istenen)
		{
			DataTable dataTable = VIVeriGetir(tabloAdi, sart, istenen);
			List<T> liste = ListeyeCevir(dataTable);
			return liste;
		}

		/// <summary>
		///     Belirtilen tabloya ait bütün verileri getirerek DataSet tipinde döndürür.
		/// </summary>
		/// <returns>DataSet tipinde değer döndürür</returns>
		public DataSet VeriGetirSQLDataSet()
		{
			DataTable dataTable = VIVeriGetir($"Select * From {tabloAdi}");
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}

		/// <summary>
		///     İçerisine aldığı string tipindeki SQL cümlesini DataSet tipinde döndürür.
		/// </summary>
		/// <param name="sql">SQL sorgu cümlesi</param>
		/// <returns>DataSet tipinde değer döndürür</returns>
		public DataSet VeriGetirSQLDataSet(string sql)
		{
			DataTable dataTable = VIVeriGetir(sql);
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}

		/// <summary>
		///     Belirtilen tabloya ait şartları sağlayan verileri getirerek tipinde DataSet tipinde döndürür.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>DataSet tipinde değer döndürür</returns>
		public DataSet VeriGetirDataSet(string sart)
		{
			DataTable dataTable = VIVeriGetir(tabloAdi, sart);
			DataSet dataSet = new DataSet(tabloAdi);
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}

		/// <summary>
		///     Belirtilen tabloya ait istenilen sütunlardaki şartları sağlayan verileri getirerek tipinde DataSet tipinde döndürür.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenen">Hangi sutun/sutunların getirilmesi isteniyorsa kullanılır</param>
		/// <returns>DataSet tipinde değer döndürür</returns>
		public DataSet VeriGetirDataSet(string sart, string istenen)
		{
			DataTable dataTable = VIVeriGetir(tabloAdi, sart, istenen);
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}

		/// <summary>
		///     Belirtilen sutundaki maximum değeri bulmak için kullanılır.
		/// </summary>
		/// <param name="istenilen">En yüksek değeri istenilen nitelik</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int MaxDeger(string istenilen)
		{
			return VIMaxDeger(tabloAdi, istenilen);
		}

		/// <summary>
		///     Belirtilen sutundaki maximum değeri uygun şartlara bağlı olarak bulmak için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">En yüksek değeri istenilen nitelik</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int MaxDeger(string sart, string istenilen)
		{
			return VIMaxDeger(tabloAdi, sart, istenilen);
		}

		/// <summary>
		///     Belirtilen sutundaki minimum değeri bulmak için kullanılır.
		/// </summary>
		/// <param name="istenilen">En düşük değeri istenilen nitelik</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int MinDeger(string istenilen)
		{
			return VIMinDeger(tabloAdi, istenilen);
		}

		/// <summary>
		///     Belirtilen sutundaki minimum değeri uygun şartlara bağlı olarak bulmak için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">En düşük değeri istenilen nitelik</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int MinDeger(string sart, string istenilen)
		{
			return VIMinDeger(tabloAdi, sart, istenilen);
		}


		/// <summary>
		///     Tablodaki veri sayısını bulmak için kullanılır.
		/// </summary>
		/// <returns>int tipinde değer döndürür</returns>
		public int VeriSayisi()
		{
			return VIVeriSayisi(tabloAdi);
		}

		/// <summary>
		///     Tablodaki veri sayısını uygun şartlara bağlı olarak bulmak için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int VeriSayisi(string sart)
		{
			return VIVeriSayisi(tabloAdi, sart);
		}

		/// <summary>
		///     Belirtilen sutundaki veri sayısını uygun şartlara bağlı olarak bulmak için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="istenilen">Veri sayısı istenilen nitelik</param>
		/// <returns>int tipinde değer döndürür</returns>
		public int VeriSayisi(string sart, string istenen)
		{
			return VIVeriSayisi(tabloAdi, sart, istenen);
		}

		/// <summary>
		///     Tablodaki şarta uygun değeri bularak nesne olarak döndürmek için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>Değer bulunursa <typeparamref name="T"/> tipinde değer döndürür, değer bulunamazsa null döndürür.</returns>
		public T Bul(string sart)
		{
			List<T> liste = VeriGetir(sart);
			if (liste != null && liste.Count > 0)
				return liste[0];
			return null;
		}

		/// <summary>
		///     İçerisine aldığı nesneyi veritabanına eklemek için kullanılır.
		/// </summary>
		/// <param name="nesne"><typeparamref name="T"/> tipindeki Reflection uygulanarak değerleri alınacak olan nesne</param>
		/// <returns>Etkilenen satır kadar int değer döndürür</returns>
		public int Ekle(T nesne)
		{
			SqlParameter[] param = ParametreOlustur(nesne);
			string paramBildiri = ParametreBildirisiOlustur(nesne);
			string sutunlar = ParametreBildirisiSutunlar(nesne);
			int sonuc = VIVeriEkle(tabloAdi, sutunlar, paramBildiri, param);
			return sonuc;
		}

		/// <summary>
		///     İçerisine aldığı nesneyi veritabanında güncellemek için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="nesne"><typeparamref name="T"/> tipindeki Reflection uygulanarak değerleri alınacak olan nesne</param>
		/// <returns>Etkilenen satır kadar int değer döndürür</returns>
		public int Guncelle(string sart, T nesne)
		{
			SqlParameter[] param = ParametreOlustur(nesne);
			string paramBildiri = ParametreBildirisiDuzenle(nesne);
			int sonuc = VIVeriGuncelle(tabloAdi, paramBildiri, sart, param);
			return sonuc;
		}

		/// <summary>
		///     İçerisine aldığı nesneyi veritabanında güncellemek için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <param name="duzenleme">Düzeltilecek olan satırda hangi sütuna karşılık geldiğini belirtir</param>
		/// <param name="deger">Düzeltilecek olan değer</param>
		/// <param name="type">Düzeltilecek olan değerin tipi</param>
		/// <returns>Etkilenen satır kadar int değer döndürür</returns>
		public int Guncelle(string sart, string duzenleme, object deger, Type type)
		{
			SqlParameter param = new SqlParameter(duzenleme, Convert.ChangeType(deger, type));
			int sonuc = VIVeriGuncelle(tabloAdi, duzenleme + " = @" + duzenleme, sart, param);
			return sonuc;
		}

		/// <summary>
		///     Belirtilen şartlara uygun değerleri veritabanından silmek için kullanılır.
		/// </summary>
		/// <param name="sart">Where ifadesindeki şart/şartlar</param>
		/// <returns>Etkilenen satır kadar int değer döndürür</returns>
		public int Sil(string sart)
		{
			int sonuc = VIVeriSil(tabloAdi, sart);
			return sonuc;
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile prosedür çağırılması için kullanılır.
		/// </summary>
		/// <param name="prosedureAdi">Etkileşimdeki prosedürün adı</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="parametreler">Prosedüre uygulanacak değerler XXXParameter tipinde komuta eklenir</param>
		/// <returns>List<typeparamref name="T"/> tipinde değer döndürür</returns>
		public List<T> ProsedureCagir(string prosedureAdi, string parametreBildirisi, SqlParameter[] parametreler)
		{
			DataTable dataTable = VIProsedurCagir(prosedureAdi, parametreBildirisi, parametreler);

			return ListeyeCevir(dataTable);
		}

		/// <summary>
		///     İçerisine aldığı parametreler ile prosedür çağırılması için kullanılır.
		/// </summary>
		/// <param name="prosedureAdi">Etkileşimdeki prosedürün adı</param>
		/// <param name="parametreBildirisi">@parametreAdi olarak komutun oluşturulaması sağlanır</param>
		/// <param name="parametreler">Prosedüre uygulanacak değerler XXXParameter tipinde komuta eklenir</param>
		/// <returns>DataSet tipinde dğer döndürür</returns>
		public DataSet ProsedureCagirDataSet(string prosedureAdi, string parametreBildirisi, SqlParameter[] parametreler)
		{
			DataTable dataTable = VIProsedurCagir(prosedureAdi, parametreBildirisi, parametreler);
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(dataTable);
			return dataSet;
		}
	}
}
