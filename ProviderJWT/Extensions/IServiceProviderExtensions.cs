namespace ProviderJWT.Extensions;

public static class IServiceProviderExtensions
{
    public static IServiceScope CMX_ObterIServiceScope(this IServiceProvider p_IServiceProvider)
    {
        var m_Service = p_IServiceProvider.GetService<IServiceScopeFactory>();
        if (m_Service == null)
            throw new NullReferenceException($"{nameof(IServiceScopeFactory)} é null");

        return m_Service.CreateScope();
    }
}