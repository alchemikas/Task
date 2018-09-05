﻿using System;
using System.Threading.Tasks;
using Product.Api.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Api.Infrastructure
{
    public interface IQueryDispatcher
    {
        Task<TResponse> Execute<TRequest, TResponse>(TRequest request);
    }


    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Execute<TRequest, TResponse>(TRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException(nameof(request));
            }
            var handler = _serviceProvider.GetService<IQueryHandler<TRequest, TResponse>>();

            if (handler == null)
            {
                throw new ArgumentException(nameof(request));
            }

            return await handler.Handle(request).ConfigureAwait(false);
        }
    }
}