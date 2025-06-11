using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidOps.UM.Shared.Domain.Entities;
using SolidOps.UM.Shared.Infrastructure;
using SolidOps.UM.Shared.Infrastructure.Queries;
using System.Linq.Expressions;

namespace SolidOps.UM.Tests;


public class Order : IEntityOfDomain<Guid>
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public Customer Customer { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderDetail> Details { get; set; }
}

public class OrderDetail
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

[TestClass]
public class FilterTests
{
    private List<Order> CreateOrders(int orderCount, int detailsPerOrder)
    {
        var customerCount = 5;

        var customers = new List<Customer>();
        for (var i = 0; i < customerCount; i++)
        {
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                Name = $"Customer_{i}"
            };
            customers.Add(customer);
        }

        var orders = new List<Order>();
        for (var i = 0; i < orderCount; i++)
        {
            Order order = new Order();
            order.Id = Guid.NewGuid();
            var customerName = $"Customer_{i % customerCount}";
            var customer = customers.Single(c => c.Name == customerName);
            order.CustomerName = customer.Name;
            order.Customer = customer;
            order.Date = DateTime.Now.Date.AddDays(i % 4);
            order.Details = new List<OrderDetail>();
            for (var ii = 0; ii < detailsPerOrder + i % 5; ii++)
            {
                var orderDetail = new OrderDetail();
                orderDetail.Id = Guid.NewGuid();
                orderDetail.Quantity = 1 + i % 3 + ii % 2;
                orderDetail.ProductName = $"Product_{i % 2 + ii % 3}";
                orderDetail.UnitPrice = (1 + i % 2 + ii % 3) * 0.2m;
                orderDetail.TotalPrice = orderDetail.Quantity * orderDetail.UnitPrice;
                order.Details.Add(orderDetail);
            }
            order.TotalPrice = order.Details.Sum(d => d.TotalPrice);
            orders.Add(order);
        }
        return orders;
    }

    [TestMethod]
    public async Task TestThatSimpleFiltersWork()
    {
        var orders = CreateOrders(20, 10);
        Assert.AreEqual(20, orders.Count);
        var customers = orders.Select(o => o.CustomerName).Distinct().ToList();
        Assert.AreEqual(5, customers.Count());
        Assert.AreEqual(4, orders.Count(o => o.CustomerName == "Customer_1"));

        BaseQueryElement query;
        Expression exp;
        Expression<Func<Order, bool>> resultExpression;
        List<Order> result;

        QueryElementFilteringTreeVisitor<Guid, Order> visitor = new QueryElementFilteringTreeVisitor<Guid, Order>();

        // first level
        query = new SingleQueryElement("CustomerName", CriteriaOperation.Equal, "Customer_1");
        exp = visitor.Visit(query);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(4, result.Count);

        // with child
        query = new SingleQueryElement("Customer.Name", CriteriaOperation.Equal, "Customer_1");
        exp = visitor.Visit(query);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(4, result.Count);

    }

    [TestMethod]
    public async Task TestThatComplexFiltersWork()
    {
        var orders = CreateOrders(20, 10);
        Assert.AreEqual(3, orders.Count(o => o.CustomerName == "Customer_1" && o.TotalPrice > 10.0m));
        Assert.AreEqual(15, orders.Count(o => o.CustomerName == "Customer_1" || o.TotalPrice > 10.0m));

        BaseQueryElement query;
        Expression exp;
        Expression<Func<Order, bool>> resultExpression;
        List<Order> result;

        QueryElementFilteringTreeVisitor<Guid, Order> visitor = new QueryElementFilteringTreeVisitor<Guid, Order>();

        // and
        var allQuery = new AllQueryElement();
        allQuery.Elements.Add(new SingleQueryElement("CustomerName", CriteriaOperation.Equal, "Customer_1"));
        allQuery.Elements.Add(new SingleQueryElement("TotalPrice", CriteriaOperation.GreaterThan, 10.0m));
        exp = visitor.Visit(allQuery);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(3, result.Count);

        // or
        var anyQuery = new AnyQueryElement();
        anyQuery.Elements.Add(new SingleQueryElement("CustomerName", CriteriaOperation.Equal, "Customer_1"));
        anyQuery.Elements.Add(new SingleQueryElement("TotalPrice", CriteriaOperation.GreaterThan, 10.0m));
        exp = visitor.Visit(anyQuery);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(15, result.Count);

    }

    [TestMethod]
    public async Task TestThatFilterParserCanParseParenthesis()
    {
        var lep = new LiteralFilterElement("(test)");
        lep.Parse();

        Assert.AreEqual(1, lep.Elements.Count);
        Assert.AreEqual("test", lep.Elements[0].Content);
        Assert.AreEqual(LiteralType.Filter, lep.Elements[0].LiteralType);
    }

    [TestMethod]
    public async Task TestThatFilterParserCanParseDoubleParenthesis()
    {
        NestedElement nestedElement;
        var operators = new List<string>() { "AND", "OR", ">=", "<=", ">", "<", "!=", "=" };            

        //nestedElement = Helper.GetNestedElement("f = 2", '(', ')', operators);
        //Assert.IsNotNull(nestedElement);

        //nestedElement = Helper.GetNestedElement("f = 2 AND b > 3", '(', ')', operators);
        //Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("(f = 2 OR a != y) AND b > 3", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("f", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("(f)", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("((f))", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("(f)(g)", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

        nestedElement = Helper.GetNestedElement("((f)(g))(i)", '(', ')', operators);
        Assert.IsNotNull(nestedElement);

    }

    [TestMethod]
    public async Task TestThatQueryElementParserWorks()
    {
        var queryElementParser = new QueryElementParser();
        var queryElement = queryElementParser.Parse("(f = 2 OR a != y) AND b > 3");

        Assert.IsNotNull(queryElement);

    }

    [TestMethod]
    public async Task TestThatQueryElementParserWorksWithOrders()
    {
        var orders = CreateOrders(20, 10);
        Assert.AreEqual(3, orders.Count(o => o.CustomerName == "Customer_1" && o.TotalPrice > 10.0m));
        Assert.AreEqual(15, orders.Count(o => o.CustomerName == "Customer_1" || o.TotalPrice > 10.0m));

        BaseQueryElement query;
        Expression exp;
        Expression<Func<Order, bool>> resultExpression;
        List<Order> result;

        QueryElementFilteringTreeVisitor<Guid, Order> visitor = new QueryElementFilteringTreeVisitor<Guid, Order>();

        // and
        var allQuery = new QueryElementParser().Parse("CustomerName = Customer_1 AND TotalPrice > 10.0");
        exp = visitor.Visit(allQuery);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(3, result.Count);

        // or
        var anyQuery = new QueryElementParser().Parse("CustomerName = Customer_1 OR TotalPrice > 10.0");
        exp = visitor.Visit(anyQuery);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(15, result.Count);

        // and
        allQuery = new QueryElementParser().Parse("Customer.Name = Customer_1 AND TotalPrice > 10.0");
        exp = visitor.Visit(allQuery);
        resultExpression = Expression.Lambda<Func<Order, bool>>(exp, visitor.ParameterExpression);
        result = orders.AsQueryable().Where(resultExpression).ToList();
        Assert.AreEqual(3, result.Count);



    }
}
