﻿using Microsoft.Extensions.DependencyInjection;


namespace Ordering.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }
    }
}