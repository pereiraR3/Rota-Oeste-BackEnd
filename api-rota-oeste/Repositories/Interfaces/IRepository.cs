namespace api_rota_oeste.Repositories.Interfaces;

/**
 * Interface que serve para disponibilizar um método genérico de SaveAsync do EntityFramework
 */
public interface IRepository
{

    void Salvar();

    void Atualizar(bool statusNovo, int id);

}