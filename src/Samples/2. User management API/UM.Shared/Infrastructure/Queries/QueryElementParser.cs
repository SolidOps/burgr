namespace SolidOps.UM.Shared.Infrastructure.Queries;

public class QueryElementParser
{
    public BaseQueryElement Parse(string literalFilter)
    {
        var operators = new List<string>() { "AND", "OR", ">=", "<=", ">", "<", "!=", "~=", "=", "&", "IN" };
        var element = Helper.GetNestedElement(literalFilter, '(', ')', operators);
        return Convert(element);
    }

    BaseQueryElement Convert(NestedElement element)
    {
        if (element.Operator == "AND")
        {
            AllQueryElement all = new AllQueryElement();
            foreach (var child in element.Children)
            {
                all.Elements.Add(Convert(child));
            }
            return all;
        }
        else if (element.Operator == "OR")
        {
            AnyQueryElement any = new AnyQueryElement();
            foreach (var child in element.Children)
            {
                any.Elements.Add(Convert(child));
            }
            return any;
        }
        else if (element.Operator != null)
        {
            CriteriaOperation operation;
            switch (element.Operator)
            {
                case ">":
                    operation = CriteriaOperation.GreaterThan;
                    break;
                case ">=":
                    operation = CriteriaOperation.GreaterThanOrEqual;
                    break;
                case "<":
                    operation = CriteriaOperation.LesserThan;
                    break;
                case "<=":
                    operation = CriteriaOperation.LesserThanOrEqual;
                    break;
                case "!=":
                    operation = CriteriaOperation.Different;
                    break;
                case "=":
                    operation = CriteriaOperation.Equal;
                    break;
                case "&":
                    operation = CriteriaOperation.BitwiseAnd;
                    break;
                case "IN":
                    operation = CriteriaOperation.In;
                    break;
                case "~=":
                    operation = CriteriaOperation.Like;
                    break;
                default:
                    throw new Exception($"unknown operator {element.Operator}");
            }
            decimal decimalOut;
            int intOut;
            DateTime dateTimeOut;
            SingleQueryElement single;
            if (decimal.TryParse(element.Children[1].Content, out decimalOut))
            {
                single = new SingleQueryElement(element.Children[0].Content, operation, decimalOut);
            }
            else if (int.TryParse(element.Children[1].Content, out intOut))
            {
                single = new SingleQueryElement(element.Children[0].Content, operation, decimalOut);
            }
            else if (DateTime.TryParse(element.Children[1].Content, out dateTimeOut))
            {
                single = new SingleQueryElement(element.Children[0].Content, operation, dateTimeOut);
            }
            else
            {
                if (operation == CriteriaOperation.In)
                {
                    single = new SingleQueryElement(element.Children[0].Content, operation, element.Children[1].Content.Split(",").ToList());
                }
                else
                {
                    single = new SingleQueryElement(element.Children[0].Content, operation, element.Children[1].Content);
                }
            }
            return single;
        }
        return null;
    }
}

public class LiteralFilterElement
{
    public LiteralFilterElement(string content)
    {
        Content = content;
        Elements = new List<LiteralFilterElement>();
    }
    public string Content { get; set; }
    public List<LiteralFilterElement> Elements { get; set; }
    public LiteralType LiteralType { get; set; }
    public OperatorType OperatorType { get; set; }

    public void Parse()
    {
        Content = Content.Trim();

        var nestedElement = Helper.GetNestedElement(Content, '(', ')', new List<string>());

        foreach (var child in nestedElement.Children)
        {
            var subElement = new LiteralFilterElement(child.Content);
            subElement.Parse();
            Elements.Add(subElement);
        }

        if (Content.Contains(" AND "))
        {
            LiteralType = LiteralType.And;
            var parts = Content.Split(" AND ");
            foreach (var part in parts)
            {
                var subElement = new LiteralFilterElement(part);
                subElement.Parse();
                Elements.Add(subElement);
            }
        }
        else if (Content.Contains(" OR "))
        {
            LiteralType = LiteralType.Or;
            var parts = Content.Split(" OR ");
            foreach (var part in parts)
            {
                var subElement = new LiteralFilterElement(part);
                subElement.Parse();
                Elements.Add(subElement);
            }
        }
        else
        {
            LiteralType = LiteralType.Filter;
        }

    }
}

public enum LiteralType
{
    NotSet = 0,
    And = 2,
    Or = 3,
    Filter = 4
}

public enum OperatorType
{
    And = 2,
    Or = 3
}

public class NestedElement
{
    public NestedElement(string content)
    {
        Content = content;
        Children = new List<NestedElement>();
    }
    public string Content { get; set; }
    public string Operator { get; set; }
    public List<NestedElement> Children { get; set; }
}


public static class Helper
{
    public static NestedElement GetNestedElement(string content, char start, char end, List<string> operators)
    {
        var element = new NestedElement(content);
        int firstStart = 0;
        bool shouldContinue = true;
        int startIndex = 0;
        List<string> fullContentWithParenthesis = new List<string>();
        while (shouldContinue)
        {
            var boundaries = GetBoundaries(content, start, end, startIndex);
            if (boundaries.start < 0 && boundaries.end < 0)
            {
                shouldContinue = false;
            }
            else
            {
                if (boundaries.start < 0 || boundaries.end < 0)
                    throw new Exception("Invalid nesting");
                fullContentWithParenthesis.Add(content.Substring(boundaries.start, boundaries.end - boundaries.start + 1));
                var nestedContent = content.Substring(boundaries.start + 1, boundaries.end - boundaries.start - 1);
                var nestedElement = GetNestedElement(nestedContent, start, end, operators);
                element.Children.Add(nestedElement);
                startIndex = boundaries.end + 1;
            }
        }

        foreach (var @operator in operators)
        {
            if (content.Contains(@operator))
            {
                element.Operator = @operator;
                var parts = content.Split(@operator);
                foreach (var part in parts)
                {
                    if (!fullContentWithParenthesis.Contains(part.Trim()))
                    {
                        var nestedElement = GetNestedElement(part.Trim(), start, end, operators);
                        element.Children.Add(nestedElement);
                    }
                }
                break;
            }
        }

        return element;
    }

    public static (int start, int end) GetBoundaries(string content, char start, char end, int startIndex)
    {
        int level = -1;
        int position = -1;
        int startPosition = -1;
        int endPosition = -1;
        foreach (var c in content)
        {
            position++;
            if (startIndex > position)
                continue;

            if (c == start)
            {
                level++;
                if (level == 0)
                {
                    startPosition = position;
                }
                continue;
            }

            if (c == end)
            {
                if (level == 0)
                {
                    endPosition = position;
                    break;
                }
                level--;
            }
        }
        return (startPosition, endPosition);
    }
}

