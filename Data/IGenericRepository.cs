using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IGenericRepository<T> where T : class // como não se sabe a entidade usasse T -> que é uma classe
    {
        IQueryable<T> GetAll(); // Método que devolve todas as entidades (a que o T tiver a usar) (IQueryable - lista)

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity); // Parâmetro - entidade T - genérico

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id); // Se existe uma entidade que recebe o id

        // Não existe nenhum gravar pois temos de saber qual tabela vamos gravar, por isso definimos isso na classe respetiva
    }
}
