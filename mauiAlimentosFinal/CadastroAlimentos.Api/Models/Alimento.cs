namespace CadastroAlimentos.Api.Models;

public class Alimento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty; // Inicializa como uma string vazia
    public double Calorias { get; set; }
}