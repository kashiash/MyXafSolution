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



```

```

