# Fakturowanie



Fakturowanie w wersji EFCore, na bazie fakturowania opisanego [kashiash/Invoice (github.com)](https://github.com/kashiash/Invoice)









docelowy model

![erd2](erd2.png)



zaczniemy jednak od wersji uproszczonej:

![erd1](erd1.png)



```

```



Grupa produktów:

```csharp
    [DefaultClassOptions]
    [DefaultProperty(nameof(Name))]
    public class ProductGroup: BaseObject
    {

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
```

Stawki Vat

```csharp
    [DefaultClassOptions]
    [NavigationItem("Others")]
    [DefaultProperty(nameof(Symbol))]
    public class VatRate :BaseObject
    {
        [FieldSize(3)]
       
        public virtual string Symbol { get; set; }
        [Precision(18, 2)]
        public virtual decimal RateValue { get; set; }
    }
```

Product

```csharp
     [DefaultClassOptions]
    [DefaultProperty(nameof(ShortName))]
    public class Product : BaseObject
    {
        [FieldSize(25)]
        public virtual string ShortName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual ProductGroup Group { get; set; }
        [Precision(18, 4)]
        public virtual decimal UnitPrice { get; set; }
        public virtual VatRate VatRate { get; set; }
        public virtual string Gtin { get; set; }
    }
```

