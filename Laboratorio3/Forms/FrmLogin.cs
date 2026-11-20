using Laboratorio3.Data;
using Laboratorio3.Models;
using Laboratorio3.Utils;

namespace Laboratorio3
{
    public partial class FrmLogin : Form
    {
        public Empleado? UsuarioAutenticado { get; private set; }
        private readonly ConsultasRepository _consultasRepo = new();

        private TextBox txtUsuario = null!;
        private TextBox txtPassword = null!;
        private Button btnIngresar = null!;
        private Button btnSalir = null!;

        public FrmLogin()
        {
            InitializeComponent();
            ConstruirInterfaz();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void ConstruirInterfaz()
        {
            Text = "Iniciar Sesión";
            Size = new Size(360, 290);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            var lblTitulo = new Label
            {
                Text = "Gestión de Ventas",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                Location = new Point(30, 25),
                AutoSize = true
            };

            var lblUser = new Label { Text = "Usuario", Location = new Point(30, 75), AutoSize = true };
            txtUsuario = new TextBox
            {
                Location = new Point(30, 98),
                Size = new Size(280, 28),
                PlaceholderText = "Ej: admin"
            };

            var lblPass = new Label { Text = "Contraseña", Location = new Point(30, 138), AutoSize = true };
            txtPassword = new TextBox
            {
                Location = new Point(30, 161),
                Size = new Size(280, 28),
                UseSystemPasswordChar = true,
                PlaceholderText = "••••••••"
            };

            btnIngresar = new Button
            {
                Text = "Ingresar",
                Location = new Point(30, 205),
                Size = new Size(135, 36),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += btnIngresar_Click;

            btnSalir = new Button
            {
                Text = "Salir",
                Location = new Point(175, 205),
                Size = new Size(135, 36),
                BackColor = Color.FromArgb(235, 235, 235),
                ForeColor = Color.FromArgb(30, 30, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblTitulo, lblUser, txtUsuario, lblPass, txtPassword, btnIngresar, btnSalir });

            AcceptButton = btnIngresar;
            CancelButton = btnSalir;
        }

        private void btnIngresar_Click(object? sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Campos vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            try
            {
                string hash = SecurityHelper.ComputeSha256Hash(password);
                var empleado = _consultasRepo.ValidarLogin(usuario, hash);

                if (empleado != null)
                {
                    UsuarioAutenticado = empleado;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}