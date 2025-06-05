# GraphQL and CQRS

This repo is based on next links:
- My previous CQRS repo - https://github.com/BekAllaev/OMSWebMini_CQRS/tree/master
- GraphQL main page - https://graphql.org/
- Mukesh Murugan's tutorial on how to implement `GraphQL` in .NET - https://codewithmukesh.com/blog/graphql-in-aspnet-core/

## Description 
In this repo I introduce concept - `StatisticalObject`.


The idea of this concept was influinced by the `GraphQL`. `GraphQL` make it easier for you to work with large objects that can be increased in the future in the several ways, for example: 
- *from data consumer perspective* it is easier to work with big object, you can choose what fields you need exactly for you, so you don't pull whole object
- on the other side is the *one who prepares the data*, from his/hers perspective it is easier to use `GraphQL` so he/she can manipulate the type and he/she does not need to 
write new endpoint or create new type that will defer from the previous one just by one field. He/she may want to do it because some data consumer doesn't want to have this "new" field
but with `GraphQL` he/she just add the field and data consumer will decide whether he/she needs it or not

So what is actually `StasticalObject`, before it were several statistical objects like:
- [CustomersByCountry](https://github.com/BekAllaev/OMSWebMini_CQRS/blob/master/OMSWebMini/Model/CustomersByCountry.cs) - shows how many *customers* are in the **country**;
- [OrdersByCountry](https://github.com/BekAllaev/OMSWebMini_CQRS/blob/master/OMSWebMini/Model/OrdersByCountry.cs) - shows how many *orders* are in the **country**;
- [SalesByCountry](https://github.com/BekAllaev/OMSWebMini_CQRS/blob/master/OMSWebMini/Model/SalesByCountry.cs) - shows how much *sales(incomer)* was completed in the **country**;

Do you see it? Everything moves around **country**, so instead of having several types we have one type - `CountryStatisticalObject` that looks like this:
```
public class CountryStatisticalObject
{
    public string CountryName { get; set; }

    public int CustomersCount { get; set; }

    public int OrdersCount { get; set; }

    public double Sales { get; set; }
}
```
Where `CountryName` will be something like primary key and the fields below are statistics(or info) that is related to this country.
If somebody wants to know `CustomersCount` he/she asks only for this and get only what he/she asks.

Let's say tomorrow I find out that I need to have **average check** for country. No problem I add this field and that is it. 