using Laboratorio3.Data;
using Laboratorio3.Models;

namespace Laboratorio3
{
    public class FrmPrincipal : Form
    {
        private readonly Empleado _empleadoActual;
        private readonly IRepository<Producto> _productoRepo = new GenericRepository<Producto>();
        private DataGridView dgvProductos = null!;

        public FrmPrincipal(Empleado empleado)
        {
            _empleadoActual = empleado;
            ConstruirInterfaz();
            Load += (s, e) => CargarProductos();
        }

        private void ConstruirInterfaz()
        {
            Text = "Gestión de Ventas - Tienda Electrónica";
            Size = new Size(1050, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.FromArgb(245, 245, 245);

            var pnlMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(24, 24, 27),
                Padding = new Padding(15)
            };

            var lblUsuario = new Label
            {
                Text = $"{_empleadoActual.Nombre} {_empleadoActual.Apellido}\n{_empleadoActual.Cargo ?? "Empleado"}",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 65
            };

            var btnNuevo = CrearBotonMenu("Nuevo Producto");
            btnNuevo.Click += btnNuevoProducto_Click;

            var btnEditar = CrearBotonMenu("Editar Producto");
            btnEditar.Click += btnEditarProducto_Click;

            var btnEliminar = CrearBotonMenu("Eliminar Producto");
            btnEliminar.Click += btnEliminarProducto_Click;

            var btnReportes = CrearBotonMenu("Reportes");
            btnReportes.Click += (s, e) => { using var rep = new FrmReportes(); rep.ShowDialog(); };

            var btnCerrar = CrearBotonMenu("Cerrar Sesión");
            btnCerrar.Dock = DockStyle.Bottom;
            btnCerrar.BackColor = Color.FromArgb(185, 28, 28);
            btnCerrar.Click += (s, e) => Application.Restart();

            pnlMenu.Controls.Add(btnCerrar);
            pnlMenu.Controls.Add(btnReportes);
            pnlMenu.Controls.Add(btnEliminar);
            pnlMenu.Controls.Add(btnEditar);
            pnlMenu.Controls.Add(btnNuevo);
            pnlMenu.Controls.Add(lblUsuario);

            var pnlContenido = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            var lblTitulo = new Label
            {
                Text = "Catálogo de Productos (Semáforo de Stock Activo)",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Dock = DockStyle.Top,
                Height = 40
            };

            dgvProductos = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            dgvProductos.CellFormatting += dgvProductos_CellFormatting;

            pnlContenido.Controls.Add(dgvProductos);
            pnlContenido.Controls.Add(lblTitulo);

            Controls.Add(pnlContenido);
            Controls.Add(pnlMenu);
        }

        private Button CrearBotonMenu(string texto)
        {
            var btn = new Button
            {
                Text = texto,
                Dock = DockStyle.Top,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(39, 39, 42),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource = _productoRepo.GetAll().ToList();
                if (dgvProductos.Columns["IdProducto"] != null)
                    dgvProductos.Columns["IdProducto"].HeaderText = "ID";
                if (dgvProductos.Columns["PrecioBase"] != null)
                {
                    dgvProductos.Columns["PrecioBase"].HeaderText = "Precio ($)";
                    dgvProductos.Columns["PrecioBase"].DefaultCellStyle.Format = "C2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoProducto_Click(object? sender, EventArgs e)
        {
            using var modal = new FrmProductoModal();
            if (modal.ShowDialog() == DialogResult.OK)
                CargarProductos();
        }

        private void btnEditarProducto_Click(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is not Producto seleccionado)
            {
                MessageBox.Show("Seleccione un producto para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var modal = new FrmProductoModal(seleccionado);
            if (modal.ShowDialog() == DialogResult.OK)
                CargarProductos();
        }

        private void btnEliminarProducto_Click(object? sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is not Producto seleccionado)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"¿Eliminar '{seleccionado.Nombre}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _productoRepo.Delete(seleccionado.IdProducto);
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se puede eliminar (puede estar asociado a ventas):\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProductos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProductos.Columns[e.ColumnIndex].Name == "Stock" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int stock))
                {
                    var fila = dgvProductos.Rows[e.RowIndex];
                    if (stock == 0)
                    {
                        fila.DefaultCellStyle.BackColor = Color.IndianRed;
                        fila.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else if (stock <= 5)
                    {
                        fila.DefaultCellStyle.BackColor = Color.Khaki;
                        fila.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        fila.DefaultCellStyle.BackColor = Color.White;
                        fila.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
    }
}