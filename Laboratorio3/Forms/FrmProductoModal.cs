using Laboratorio3.Data;
using Laboratorio3.Models;

namespace Laboratorio3
{
    public class FrmProductoModal : Form
    {
        private readonly IRepository<Producto> _productoRepo = new GenericRepository<Producto>();
        private readonly Producto? _productoEditar;

        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;
        private NumericUpDown numPrecio = null!;
        private NumericUpDown numStock = null!;

        public FrmProductoModal(Producto? producto = null)
        {
            _productoEditar = producto;
            ConstruirInterfaz();
            if (_productoEditar != null)
                CargarDatos();
        }

        private void ConstruirInterfaz()
        {
            Text = _productoEditar == null ? "Nuevo Producto" : "Editar Producto";
            Size = new Size(400, 360);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            var lblNombre = new Label { Text = "Nombre del Producto", Location = new Point(25, 20), AutoSize = true };
            txtNombre = new TextBox { Location = new Point(25, 43), Size = new Size(330, 28) };

            var lblDesc = new Label { Text = "Descripción", Location = new Point(25, 82), AutoSize = true };
            txtDescripcion = new TextBox { Location = new Point(25, 105), Size = new Size(330, 60), Multiline = true };

            var lblPrecio = new Label { Text = "Precio Base ($)", Location = new Point(25, 178), AutoSize = true };
            numPrecio = new NumericUpDown
            {
                Location = new Point(25, 201),
                Size = new Size(155, 28),
                DecimalPlaces = 2,
                Minimum = 0.01m,
                Maximum = 999999.99m,
                Value = 1.00m
            };

            var lblStock = new Label { Text = "Stock Inicial", Location = new Point(200, 178), AutoSize = true };
            numStock = new NumericUpDown
            {
                Location = new Point(200, 201),
                Size = new Size(155, 28),
                Minimum = 0,
                Maximum = 100000,
                Value = 10
            };

            var btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(25, 255),
                Size = new Size(155, 38),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += btnGuardar_Click;

            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(200, 255),
                Size = new Size(155, 38),
                BackColor = Color.FromArgb(235, 235, 235),
                ForeColor = Color.FromArgb(30, 30, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[]
            {
                lblNombre, txtNombre, lblDesc, txtDescripcion,
                lblPrecio, numPrecio, lblStock, numStock,
                btnGuardar, btnCancelar
            });

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
        }

        private void CargarDatos()
        {
            txtNombre.Text = _productoEditar!.Nombre;
            txtDescripcion.Text = _productoEditar.Descripcion ?? "";
            numPrecio.Value = Math.Max(numPrecio.Minimum, _productoEditar.PrecioBase);
            numStock.Value = Math.Max(numStock.Minimum, _productoEditar.Stock);
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del producto es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            try
            {
                if (_productoEditar == null)
                {
                    var nuevo = new Producto
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        PrecioBase = numPrecio.Value,
                        Stock = (int)numStock.Value
                    };
                    _productoRepo.Insert(nuevo);
                }
                else
                {
                    _productoEditar.Nombre = txtNombre.Text.Trim();
                    _productoEditar.Descripcion = txtDescripcion.Text.Trim();
                    _productoEditar.PrecioBase = numPrecio.Value;
                    _productoEditar.Stock = (int)numStock.Value;
                    _productoRepo.Update(_productoEditar);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}