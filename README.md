# Dapper.JsonTypeMapping
Dapper extension that is adding Custom Type Mapping for objects and string that are serialized as JSON objects in SQL Server.

## Installation

[Dapper.JsonTypeMapping](https://www.nuget.org/packages/Dapper.JsonTypeMapping) is available as NuGet package. You can install it using NuGet **Install-Package** command:

```
Install-Package Dapper.JsonTypeMapping
```

Or you can use .Net CLI:
```
dotnet add package Dapper.JsonTypeMapping
```

# Mapping string[] to JSON

Let's assume that you have a C# class where one of the fields is `string[]` like in the following example:
```
public class Product
{
    public int ProductID;
    public string Name;
    public double Price;
    public int Quantity;
    public string[] Tags;
}
```
Underlying table stores tags formatted as **JSON** string array in NVARCHAR column:

```
CREATE TABLE Product (
	ProductID int IDENTITY PRIMARY KEY,
	Name nvarchar(50) NOT NULL,
	Price money NOT NULL,
	Quantity int NULL,
	Tags nvarchar(4000)
)
```

The end goal is to transparently map JSON column to `string[]` C# field without any transformation:
```
var products = connection.Query<Product>(@"select * from Product");
if(products.Tags.Any(tag => tag == "Sales")) {

}
```

With [Dapper.JsonTypeMapping](https://www.nuget.org/packages/Dapper.JsonTypeMapping) this is possible with a single line of code:
```
public void ConfigureServices(IServiceCollection services)
{
    // Map string[] properties in C# model to JSON columns in a database.
    SqlMapper.AddTypeHandler(typeof(string[]), new StringArrayJsonMapper());
}
```

Type handler `StringArrayJsonMapper` should be added once when you start the application (for example in `ConfigureServices` method in ASP.NET Core). Whenever you load objects that have `string[]` properties, this TypeHandler will transparetnly load string arrays formatted as JSON text to C# string arrays in your model class.

# Mapping objects to JSON fields
Model objects might contain arbitrary objects as properties. 
```
public class Product
{
    public int ProductID;
    public string Name;
    public double Price;
    public int Quantity;
    public object Data;
}
```
You can store the object properties as in a separate table in the databaase. However, as an alternative you can store a whole object property formatted as JSON in a NVARCHAR column:
```
CREATE TABLE Product (
	ProductID int IDENTITY PRIMARY KEY,
	Name nvarchar(50) NOT NULL,
	Price money NOT NULL,
	Quantity int NULL,
	Data nvarchar(4000)
)
```
To achieve this, you should just add Type handler that will map objects to JSON:
```
public void ConfigureServices(IServiceCollection services)
{
    // Map objects in a model class to JSON column in database.
    SqlMapper.AddTypeHandler(typeof(object), new ObjectJsonMapper());
}
```
