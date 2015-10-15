using System;
using System.Windows;
using DataLayer;
using System.Windows.Media.Animation;

namespace DiCaBoo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Settings
        private void btnSubmitSettings_Click(object sender, RoutedEventArgs e)
        {
            mSettings = new Settings(txtFbAppToken.Text, txtFbUserId.Text);
            BeginStoryboard sbHide = Application.Current.Resources["HideLabelAnimation"] as BeginStoryboard;
            txtSaved.Visibility = Visibility.Visible;
            sbHide.Storyboard.Begin(txtSaved);

            if (FacebooNotValid())
            {
                chkPostOnFacebook.IsChecked = false;
                chkPostOnFacebook.IsEnabled = false;
            }
            else
            {
                chkPostOnFacebook.IsEnabled = true;
            }
        }

        private void SetSettings()
        {
            txtFbAppToken.Text = mSettings.FacebookAppToken;
            txtFbUserId.Text = mSettings.FacebookUserId;

            if (FacebooNotValid())
            {
                chkPostOnFacebook.IsChecked = false;
                chkPostOnFacebook.IsEnabled = false;
            }
        }

        private bool FacebooNotValid()
        {
            return (String.IsNullOrWhiteSpace(mSettings.FacebookAppToken)
                || String.IsNullOrWhiteSpace(mSettings.FacebookUserId));
        }
        #endregion
    }
}
