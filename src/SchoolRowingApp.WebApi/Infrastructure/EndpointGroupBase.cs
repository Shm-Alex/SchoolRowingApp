using Microsoft.AspNetCore.Builder;

namespace SchoolRowingApp.Web.Infrastructure;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
