using RepoDb;

namespace Laboratorio3
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            GlobalConfiguration.Setup().UseSqlServer();
            ApplicationConfiguration.Initialize();

            using var login = new FrmLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmPrincipal(login.UsuarioAutenticado!));
            }
        }
    }
}