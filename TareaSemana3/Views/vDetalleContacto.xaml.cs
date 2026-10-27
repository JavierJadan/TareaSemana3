using System.Globalization;
using System.Text;

namespace TareaSemana3.Views;

public partial class vDetalleContacto : ContentPage
{
    private const decimal PorcentajeIess = 0.0945m;
    private readonly string contenidoArchivo;

    public vDetalleContacto(
        string tipo,
        string identificacion,
        string nombres,
        string apellidos,
        DateTime fecha,
        string correo,
        decimal salario)
    {
        InitializeComponent();

        decimal aporteIess = salario * PorcentajeIess;
        string fechaTexto = fecha.ToString("dd/MM/yyyy");
        string salarioTexto = salario.ToString("C2", CultureInfo.CurrentCulture);
        string iessTexto = aporteIess.ToString("C2", CultureInfo.CurrentCulture);

        lblTipo.Text = $"Tipo de identificación: {tipo}";
        lblIdentificacion.Text = $"Identificación: {identificacion}";
        lblNombres.Text = $"Nombres: {nombres}";
        lblApellidos.Text = $"Apellidos: {apellidos}";
        lblFecha.Text = $"Fecha de nacimiento: {fechaTexto}";
        lblCorreo.Text = $"Correo: {correo}";
        lblSalario.Text = $"Salario: {salarioTexto}";
        lblIess.Text = $"Aporte personal al IESS (9,45 %): {iessTexto}";

        contenidoArchivo = new StringBuilder()
            .AppendLine("DATOS DEL CONTACTO")
            .AppendLine($"Tipo de identificación: {tipo}")
            .AppendLine($"Identificación: {identificacion}")
            .AppendLine($"Nombres: {nombres}")
            .AppendLine($"Apellidos: {apellidos}")
            .AppendLine($"Fecha de nacimiento: {fechaTexto}")
            .AppendLine($"Correo: {correo}")
            .AppendLine($"Salario: {salarioTexto}")
            .AppendLine($"Aporte personal al IESS (9,45 %): {iessTexto}")
            .ToString();
    }

    private async void BtnExportar_OnClicked(object? sender, EventArgs e)
    {
        try
        {
            string nombreArchivo = $"contacto_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string ruta = Path.Combine(FileSystem.CacheDirectory, nombreArchivo);

            await File.WriteAllTextAsync(ruta, contenidoArchivo);

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Exportar datos del contacto",
                File = new ShareFile(ruta)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo exportar el archivo: {ex.Message}", "Cerrar");
        }
    }
}
