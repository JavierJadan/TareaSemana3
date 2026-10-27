using System.Globalization;
using System.Net.Mail;

namespace TareaSemana3.Views;

public partial class vRegistroContacto : ContentPage
{
    public vRegistroContacto()
    {
        InitializeComponent();
    }

    private void PckIdentificacion_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        txtIdentificacion.Text = string.Empty;

        if (pckIdentificacion.SelectedItem?.ToString() == "CI")
        {
            txtIdentificacion.Placeholder = "Ingrese 10 números";
            txtIdentificacion.Keyboard = Keyboard.Numeric;
        }
        else if (pckIdentificacion.SelectedItem?.ToString() == "RUC")
        {
            txtIdentificacion.Placeholder = "Ingrese 13 números";
            txtIdentificacion.Keyboard = Keyboard.Numeric;
        }
        else
        {
            txtIdentificacion.Placeholder = "Ingrese el pasaporte";
            txtIdentificacion.Keyboard = Keyboard.Text;
        }
    }

    private async void BtnGuardar_OnClicked(object? sender, EventArgs e)
    {
        string tipo = pckIdentificacion.SelectedItem?.ToString() ?? string.Empty;
        string identificacion = txtIdentificacion.Text?.Trim() ?? string.Empty;
        string nombres = txtNombres.Text?.Trim() ?? string.Empty;
        string apellidos = txtApellidos.Text?.Trim() ?? string.Empty;
        string correo = txtCorreo.Text?.Trim() ?? string.Empty;
        string salarioTexto = txtSalario.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(tipo) || string.IsNullOrWhiteSpace(identificacion) ||
            string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) ||
            string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(salarioTexto))
        {
            await DisplayAlertAsync("Validación", "Complete todos los campos.", "Cerrar");
            return;
        }

        if (tipo == "CI" && (identificacion.Length != 10 || !identificacion.All(char.IsDigit)))
        {
            await DisplayAlertAsync("Validación", "La CI debe contener 10 números.", "Cerrar");
            return;
        }

        if (tipo == "RUC" && (identificacion.Length != 13 || !identificacion.All(char.IsDigit)))
        {
            await DisplayAlertAsync("Validación", "El RUC debe contener 13 números.", "Cerrar");
            return;
        }

        if (tipo == "Pasaporte" && identificacion.Length < 5)
        {
            await DisplayAlertAsync("Validación", "Ingrese un pasaporte válido.", "Cerrar");
            return;
        }

        if (!EsCorreoValido(correo))
        {
            await DisplayAlertAsync("Validación", "Ingrese un correo válido.", "Cerrar");
            return;
        }

        if (!decimal.TryParse(salarioTexto, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal salario) &&
            !decimal.TryParse(salarioTexto, NumberStyles.Number, CultureInfo.InvariantCulture, out salario))
        {
            await DisplayAlertAsync("Validación", "Ingrese un salario numérico.", "Cerrar");
            return;
        }

        if (salario <= 0)
        {
            await DisplayAlertAsync("Validación", "El salario debe ser mayor que cero.", "Cerrar");
            return;
        }

        await Navigation.PushAsync(new vDetalleContacto(
            tipo,
            identificacion,
            nombres,
            apellidos,
            dpFecha.Date ?? DateTime.Today,
            correo,
            salario));
    }

    private static bool EsCorreoValido(string correo)
    {
        try
        {
            return new MailAddress(correo).Address == correo;
        }
        catch
        {
            return false;
        }
    }
}
