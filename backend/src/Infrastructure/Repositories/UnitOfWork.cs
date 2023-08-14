﻿using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable? _repositories;
    private readonly EcommerceDbContext _context;

    public UnitOfWork(EcommerceDbContext ecommerceDb)
    {
        _context = ecommerceDb;
    }

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {

            throw new Exception("Error en transaccion", e); 
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    // ENLAZA LA ENTIDAD A UNA INSTANCIA DE REPOSITORIO
    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if( _repositories is null)
        {
            _repositories = new Hashtable();    
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);

            var repositoryInstance = Activator
                .CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)),
                _context); 

            _repositories.Add(type, repositoryInstance);
        }

        return (IAsyncRepository<TEntity>)_repositories[type]!; 
    }
}
