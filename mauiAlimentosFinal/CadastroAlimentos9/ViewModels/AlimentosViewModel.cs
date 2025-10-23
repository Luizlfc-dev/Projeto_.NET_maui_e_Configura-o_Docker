// Local: CadastroAlimentos9/ViewModels/AlimentosViewModel.cs

using CadastroAlimentos9.Models; // O modelo que acabamos de criar
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json; // O pacote que instalamos
using System.Threading.Tasks;

namespace CadastroAlimentos9.ViewModels
{
    public class AlimentosViewModel : BindableObject
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Alimento> Alimentos { get; set; }

        private string _nome = string.Empty;
        public string Nome
        {
            get => _nome;
            set { _nome = value; OnPropertyChanged(); }
        }

        private int _calorias;
        public int Calorias
        {
            get => _calorias;
            set { _calorias = value; OnPropertyChanged(); }
        }

        public Command SalvarCommand { get; }

        public AlimentosViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient; // Apenas atribui
            Alimentos = new ObservableCollection<Alimento>();
            SalvarCommand = new Command(async () => await SalvarAlimento());
        }

        public async Task CarregarAlimentos()
        {
            try
            {
                
                var alimentosDaApi = await _httpClient.GetFromJsonAsync<List<Alimento>>("/api/alimentos");

                if (alimentosDaApi != null)
                {
                    // Atualiza a lista na tela (tem que ser na MainThread)
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Alimentos.Clear();
                        foreach (var alimento in alimentosDaApi)
                        {
                            Alimentos.Add(alimento);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode mostrar um Alerta de erro
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar alimentos: {ex.Message}");
            }
        }

        private async Task SalvarAlimento()
        {
            if (string.IsNullOrWhiteSpace(Nome) || Calorias <= 0)
                return;

            var novoAlimento = new Alimento { Nome = this.Nome, Calorias = this.Calorias };

            try
            {
                
                var response = await _httpClient.PostAsJsonAsync("/api/alimentos", novoAlimento);

                if (response.IsSuccessStatusCode)
                {
                    // Limpa os campos e recarrega a lista
                    Nome = string.Empty;
                    Calorias = 0;
                    await CarregarAlimentos();
                }
            }
            catch (Exception ex)
            {
                // Aqui você pode mostrar um Alerta de erro
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar alimento: {ex.Message}");
            }
        }
    }
}