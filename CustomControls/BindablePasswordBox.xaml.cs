using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace AdmissionCampaign.CustomControls
{
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePasswordBox));

        public SecureString Password { get => (SecureString)GetValue(PasswordProperty); set => SetValue(PasswordProperty, value); }

        public BindablePasswordBox()
        {
            InitializeComponent();
            txtPassword.PasswordChanged += OnPasswordChanged; // Привязка изменение поля к делегату-событию
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.SecurePassword;
        }
    }
}
