using System;
using System.Collections.Generic;

namespace ShieldJWTLib.Interfaces
{
    public interface IShieldEntity<TypeT> where TypeT : class
    {
        IEnumerable<TypeT> GetAll(Func<TypeT, bool> predicado = null);
        int Create(IEnumerable<TypeT> entities);
        int Update(IEnumerable<TypeT> entities);
        int Delete(int id);
    }
}
