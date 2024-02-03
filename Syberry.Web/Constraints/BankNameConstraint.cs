namespace Syberry.Web.Constraints;

public class BankNameConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out object? routeValue))
        {
            string? bankName = Convert.ToString(routeValue);

            List<string> lowerCaseNames = Constants.BanksLists.Select(s => s.ToLower()).ToList();
            if (lowerCaseNames.Contains(bankName.ToLower()))
                return true;
        }

        return false;
    }
}