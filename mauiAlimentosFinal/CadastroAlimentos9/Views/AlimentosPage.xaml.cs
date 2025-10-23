// Local: CadastroAlimentos9/Views/AlimentosPage.xaml.cs

using CadastroAlimentos9.ViewModels; // Verifique se este using está aqui

namespace CadastroAlimentos9.Views
{
    public partial class AlimentosPage : ContentPage
    {
        // Este é o seu construtor (deixe como está)
        public AlimentosPage(AlimentosViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        // --- ADICIONE ESTE MÉTODO ---
        // Este método é chamado automaticamente pelo MAUI
        // toda vez que a página aparece na tela.
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Carrega os dados com segurança
            if (BindingContext is AlimentosViewModel vm)
            {
                await vm.CarregarAlimentos();
            }
        }
    }
}