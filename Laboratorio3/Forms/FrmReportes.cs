using Laboratorio3.Data;
using Laboratorio3.Models;

namespace Laboratorio3
{
    public class FrmReportes : Form
    {
        private readonly IRepository<Producto> _productoRepo = new GenericRepository<Producto>();
        private readonly ConsultasRepository _consultasRepo = new();

        private DataGridView dgvVentas = null!;
        private DataGridView dgvStock = null!;
        private TextBox txtBuscarStock = null!;
        private List<Producto> _listaProductos = new();

        public FrmReportes()
        {
            ConstruirInterfaz();
            Load += (s, e) => CargarReportes();
        }

        private void ConstruirInterfaz()
        {
            Text = "Módulo de Reportes - Ventas y Stock";
            Size = new Size(920, 560);
            StartPosition = FormStartPosition.CenterParent;
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.White;

            var tabs = new TabControl { Dock = DockStyle.Fill };

            var tabVentas = new TabPage("Productos Vendidos") { Padding = new Padding(15), BackColor = Color.White };
            dgvVentas = CrearGrid();
            tabVentas.Controls.Add(dgvVentas);

            var tabStock = new TabPage("Stock de Inventario") { Padding = new Padding(15), BackColor = Color.White };

            var pnlFiltro = new Panel { Dock = DockStyle.Top, Height = 45 };
            var lblFiltro = new Label { Text = "Buscar producto:", Location = new Point(0, 10), AutoSize = true };
            txtBuscarStock = new TextBox
            {
                Location = new Point(120, 7),
                Size = new Size(300, 28),
                PlaceholderText = "Escriba nombre para filtrar (vacío = todos)..."
            };
            txtBuscarStock.TextChanged += (s, e) => FiltrarStock();

            var lblLeyenda = new Label
            {
                Text = "🔴 0: Agotado   🟡 1-5: Crítico",
                Location = new Point(440, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            pnlFiltro.Controls.AddRange(new Control[] { lblFiltro, txtBuscarStock, lblLeyenda });

            dgvStock = CrearGrid();
            dgvStock.CellFormatting += dgvStock_CellFormatting;

            tabStock.Controls.Add(dgvStock);
            tabStock.Controls.Add(pnlFiltro);

            tabs.TabPages.Add(tabVentas);
            tabs.TabPages.Add(tabStock);

            Controls.Add(tabs);
        }

        private DataGridView CrearGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false
            };
        }

        private void CargarReportes()
        {
            try
            {
                dgvVentas.DataSource = _consultasRepo.ObtenerProductosVendidos().ToList();
                if (dgvVentas.Columns["MontoAcumulado"] != null)
                {
                    dgvVentas.Columns["MontoAcumulado"].HeaderText = "Monto Acumulado ($)";
                    dgvVentas.Columns["MontoAcumulado"].DefaultCellStyle.Format = "C2";
                }

                _listaProductos = _productoRepo.GetAll().ToList();
                dgvStock.DataSource = _listaProductos;
                if (dgvStock.Columns["PrecioBase"] != null)
                {
                    dgvStock.Columns["PrecioBase"].HeaderText = "Precio ($)";
                    dgvStock.Columns["PrecioBase"].DefaultCellStyle.Format = "C2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar reportes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarStock()
        {
            string filtro = txtBuscarStock.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
            {
                dgvStock.DataSource = _listaProductos;
            }
            else
            {
                dgvStock.DataSource = _listaProductos
                    .Where(p => p.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        private void dgvStock_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvStock.Columns[e.ColumnIndex].Name == "Stock" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int stock))
                {
                    var fila = dgvStock.Rows[e.RowIndex];
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