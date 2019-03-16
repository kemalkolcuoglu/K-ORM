# K-ORM

> ORM using ADO.NET Providers and System.Reflection

Kendi projelerimde veritabanı işlemlerinde kullanmak üzere geliştirmiş olduğum bu projede ADO.NET ve System.Reflection sınıfları kullanılarak CRUD işlemlerinin kolayca gerçekleştirilmesi hedeflenmiştir.

**SQL Server ve MySQL için kullanılabilir!** Repo içerisinde ihtiyacınıza bağlı projeyi indirerek **özgürce** kullanabilirsiniz.

GNU Public Lisans-3.0 ile lisanslanmıştır. Kullanımı **özgürdür!**

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
![](https://github.com/kemalkolcuoglu/K-ORM/blob/master/korm.png)

**Not:** Geliştirici dökümantasyonu en kısa sürede hazırlanacaktır.

## Gelecek Sürüm Hedefleri

 1. OracleDB ve SQLite Provider eklenmesi
 2. Çoklu Insert işlemlerinin gerçekleştirilmesi
