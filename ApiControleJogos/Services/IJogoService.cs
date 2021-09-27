using ApiControleJogos.ImputModel;
using ApiControleJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiControleJogos.Services
{
    public interface IJogoService : IDisposable
    {
        Task<List<JogoViewModel>> Obter(int pagina, int quatidade);
        Task<JogoViewModel> Obter(Guid id);
        Task<JogoViewModel> Inserir(JogoImputModel jogo);
        Task Atualizar(Guid id, JogoImputModel jogo);
        Task Atualizar(Guid id, double preco);
        Task Remover(Guid id);
    }
}
