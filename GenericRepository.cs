using GenericEntityRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericEntityRepository {
    public class GenericRepository<C, T> : IGenericRepository<T>, IDisposable
        where T : class
        where C : DbContext, new() {
        private C _entities = new C();
        private bool _disposed;
        private bool _ownsContext = true;

        public C Context {
            get { return _entities; }
            set { _entities = value; }
        }
        /// <summary>
        /// gets all records
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll() {
            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        /// <summary>
        /// gets a filtered set based on the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate) {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        /// <summary>
        /// Gets a single record based on the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T GetSingle(Expression<Func<T, bool>> predicate) {
            T query = _entities.Set<T>().Where(predicate).SingleOrDefault();
            return query;
        }

        /// <summary>
        /// adds a new record
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity) {
            _entities.Set<T>().Add(entity);
        }

        /// <summary>
        /// updates the object
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity) {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// deletes the object
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity) {
            _entities.Set<T>().Remove(entity);
        }

        /// <summary>
        /// saves the changes
        /// </summary>
        public void Save() {
            _entities.SaveChanges();
        }

        /// <summary>
        /// disposes of the data context
        /// </summary>
        /// <param name="dispose"></param>
        protected virtual void Dispose(bool dispose) {
            if (dispose) {
                if (!_disposed) {
                    if (_ownsContext) {
                        _entities.Dispose();
                    }
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
