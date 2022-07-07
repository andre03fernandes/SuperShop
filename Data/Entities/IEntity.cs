namespace SuperShop.Data.Entities
{
    public interface IEntity // Campos comuns entre tabelas
    {
        int Id { get; set; }

       // bool WasDeleted { get; set; } // registo fica lá, mas é considerado apagado (utlizador apaga registos mas não da base de dados)
    }
}