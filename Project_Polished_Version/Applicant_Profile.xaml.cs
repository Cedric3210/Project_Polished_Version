using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Project_Polished_Version.Classes
{
    public partial class Applicant_Profile : Window
    {
        public static int windowNumber { get; set; }
        public static int searchUserKey { get; set; }
        public static List<int> keys = new List<int>();

        public Applicant_Profile()
        {
            InitializeComponent();
            ShowInfo();
        }

        public Applicant_Profile(ApplicantUser userInfo)
        {
            InitializeComponent();

            // Set user information on the UI
            Full_Name.Text = $"{userInfo.First_Name} {userInfo.Last_Name}";
            Job_Title_txtbox.Text = userInfo.JobTitle;
            Address_txtbox.Text = userInfo.Address;
            searchUserKey = userInfo.Id;
           disableButton();
        }

        private void disableButton()
        {
            using (MySqlConnection connection = new MySqlConnection(
                "Server=localhost;Database=project_database;UserName=root;Password=Cedric1234%%"))
            {
                try
                {
                    connection.Open();

                    // Query to check if a friendship already exists
                    string query = "SELECT COUNT(*) FROM friends WHERE (user_id = @UserId AND friend_id = @FriendId) " +
                                   "OR (user_id = @FriendId AND friend_id = @UserId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", MainWindow.userID);
                        cmd.Parameters.AddWithValue("@FriendId", searchUserKey);


                        int friendshipCount = Convert.ToInt32(cmd.ExecuteScalar());


                        if (friendshipCount > 0)
                        {
                            Connect_Friend_Btn.IsEnabled = false;
                            Connect_Friend_Btn.Content = "Already Connected";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while checking the friendship: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void ShowInfo()
        {
            int key = MainWindow.userID;
            ApplicantUser logedUser = MainWindow.userAccountsGetID[key];

            if (MainWindow.userAccountsGetID.TryGetValue(key, out ApplicantUser storedHash))
            {
                Full_Name.Text = logedUser.First_Name + " " + logedUser.Last_Name;
                Job_Title_txtbox.Text = logedUser.JobTitle;
                Address_txtbox.Text = logedUser.Address;
            }
        }

        public void ChangeInfo(string name, string jobTitle, string address)
        {
            Full_Name.Text = name;
            Job_Title_txtbox.Text = jobTitle;
            Address_txtbox.Text = address;
        }

        private void Edit_Experience_btn_Click(object sender, RoutedEventArgs e)
        {
            EditToggle(Edit_Experience_btn);
        
        }

        private void Education_Edit_btn_Click(object sender, RoutedEventArgs e)
        {
            EditToggle(Education_Edit_btn);
        }
        int ClickNumber = 0;
        string status;
        private void EditToggle(Button btn)
        {
            int even = ClickNumber % 2;
            switch (even)
            {
                case 0:
                    btn.Content = "Edit";
                    status = "Edit";
                    ClickNumber++;
                    break;
                case 1:
                    btn.Content = "save";
                    status = "Save";
                    ClickNumber++;
                    break;
            }
        }

        private void abt_you_btn_Click(object sender, RoutedEventArgs e)
        {
            EditToggle(abt_you_btn);

            if (status.Equals("Edit"))
            {
                //About_RichTextBox.DataContext 
            }
            else if (status.Equals("Save"))
            {

            }
        }

        private void AddFriend_Btn(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Server=localhost;"
                 + "Database=project_database;UserName= root;" + "Password=Cedric1234%%");
            int Connect_Key = searchUserKey;
            int Logged_In_UserKeey = MainWindow.userID;
            MessageBox.Show("Other user key: " + Connect_Key);
            MessageBox.Show("Cure user key: " + Logged_In_UserKeey);
            string query =
                "INSERT INTO friends (user_id, friend_id) VALUES (@UserId, @FriendId)";
            try
            {
                using (connection)
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", Logged_In_UserKeey);
                        cmd.Parameters.AddWithValue("@FriendId", Connect_Key);

                        int rowsAffected = cmd.ExecuteNonQuery();


                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Friend request sent successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to send friend request.");
                        }

                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("An error occurred: " + ex.Message); }
        }

        private void showFriendList_btn(object sender, RoutedEventArgs e)
        {
            Friend_List_Window flw = new Friend_List_Window();
            flw.Show();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (windowNumber)
                {
                    case 1:
                        var dashboard = new Applicant_DashBoard();
                        this.Hide();
                        dashboard.Show();
                        break;

                    case 2:
                        var searchWindow = new Applicant_Search();
                        this.Hide();
                        searchWindow.Show();
                        break;

                    default:
                        MessageBox.Show("Invalid window number.", "Error", MessageBoxButton.OK);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while navigating back: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }
        private void CreateEducationTXT_box()
        {
            TextBox Add_Education_ntb = new TextBox
            {
                Text = "",
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 100,
                Margin = new Thickness(0, 10, 0, 0)
            };
            // Add the new TextBox and Button to the StackPanel
           // Education_StackPanel.Children.Add(Add_Education_ntb);
        }
        private void Add_new_Education(object sender, RoutedEventArgs e)
        {
            CreateEducationTXT_box();
        }

        private void Add_Data_Experience_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
