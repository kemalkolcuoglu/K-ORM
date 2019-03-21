# K-ORM

> ORM using ADO.NET Providers and System.Reflection

Kendi projelerimde veritabanı işlemlerinde kullanmak üzere geliştirmiş olduğum bu projede ADO.NET ve System.Reflection sınıfları kullanılarak CRUD işlemlerinin kolayca gerçekleştirilmesi hedeflenmiştir.

**SQL Server ve MySQL için kullanılabilir!** Repo içerisinde ihtiyacınıza bağlı projeyi indirerek **özgürce** kullanabilirsiniz.

GNU Public Lisans-3.0 ile lisanslanmıştır. Kullanımı **özgürdür!**

## Kütüphane Mimarisi

![](https://github.com/kemalkolcuoglu/K-ORM/blob/master/korm.png)

## Kullanım Örneği

```C#
public class Tablo
{
  public int ID { get; set; }
}

public class K_ORM_APP
{
  TemelIslemler<Tablo> tabloIslemleri = new TemelIslemler<Tablo>("tablo");
  
  public List<Tablo> VeriDoldur()
  {
    List<Tablo> veri = tabloIslemleri.VeriGetir();
    retuen veri;
  }
}
```

## K-ORM Kullanabilmek için Gerekli Adımlar
 1.  Veritabanında yer alan tablo ve niteliklere uyumlu C# sınıfları oluşturulur. (Sınıfın adının aynı olma zorunluluğu yoktur. Ancak sınıf içerisinde kullanılacak özelliklerin -property- tabloya ait niteliklerle aynı adı taşıma zorunludur.)
 2. Veritabanı işlemleri yapılacak tablo için TemelIslemler sınıfına ait nesne oluşturulur.
 ![](https://github.com/kemalkolcuoglu/K-ORM/blob/master/temelislemlernesnesi.png)
 
	 TemelIslemler sınıfına ait nesne oluşturulurken yapıcı metodu string tipinde bir değer alarak veritabanında işlem yapılacak tablonun adı tanımlanır. Bu sayede tekrar tekrar tablonun adı belirtilmeksizin işlemler gerçekleştirilir.
 3. Gerekli metotlar içerisinde oluşturulan nesnenin kullanılması.
  ```C#
Tablo tablo = tabloIslemleri.Bul("ID = " + id);
int sonuc = tabloIslemleri.Ekle(new Tablo());
int sonuc = tabloIslemleri.Guncelle("ID = " + id, new Tablo());
int deger = tabloIslemleri.MaxDeger("ID");
int deger = tabloIslemleri.MaxDeger("Etkin = 1", "ID");
int deger = tabloIslemleri.MinDeger("ID");
int deger = tabloIslemleri.MinDeger("Etkin = 1", "ID");
int sonuc = tabloIslemleri.Sil("ID = " + id);
List<Tablo> tablolar = tabloIslemleri.VeriGetir();
List<Tablo> tablolar = tabloIslemleri.VeriGetir("Etkin = 1");
List<Tablo> tablolar = tabloIslemleri.VeriGetir("Etkin = 1", "ID");
DataSet dataSet = tabloIslemleri.VeriGetirDataSet("Etkin = 1");
DataSet dataSet = tabloIslemleri.VeriGetirDataSet("Etkin = 1", "ID");
List<Tablo> tablolar = tabloIslemleri.VeriGetirSQL("Select * From tablo Where Etkin = 1");
DataSet dataSet = tabloIslemleri.VeriGetirSQLDataSet();
DataSet dataSet = tabloIslemleri.VeriGetirSQLDataSet("Select * From tablo Where Etkin = 1");
int deger = tabloIslemleri.VeriSayisi();
int deger = tabloIslemleri.VeriSayisi("Etkin = 1");
int deger = tabloIslemleri.VeriSayisi("Etkin = 1", "ID");
```
4. App.config/Web.config dosyasının içerisine ```name="DatabaseContext"``` değerli connectionString oluşturulur.

## Gelecek Sürüm Hedefleri

 1. OracleDB ve SQLite Provider eklenmesi
 2. Çoklu Insert işlemlerinin gerçekleştirilmesi
