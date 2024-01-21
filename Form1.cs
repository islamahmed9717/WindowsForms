using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Windows;

namespace Windows
{
    public partial class Form1 : Form
    {
        private BindingList<WayPoints> wayPoints;
        private BindingSource wayPointsBindingSource;

        public Form1()
        {
            InitializeComponent();
            wayPoints = new BindingList<WayPoints>
        {
            new WayPoints(40.7128, -74.0060), // New York City
            new WayPoints(34.0522, -118.2437), // Los Angeles
            new WayPoints(41.8781, -87.6298) // Chicago
        };

            wayPointsBindingSource = new BindingSource(wayPoints, null);
            dataGridView1.DataSource = wayPointsBindingSource;


            // optional
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["latitude"].HeaderText = "Latitude";
            dataGridView1.Columns["longitude"].HeaderText = "Longitude";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            wayPoints.Add(new WayPoints(0.0, 0.0));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow SelectedRow = dataGridView1.CurrentRow;
            if (SelectedRow != null)
            {
                WayPoints SelectedWayPoint = (WayPoints)SelectedRow.DataBoundItem;
                wayPoints.Remove(SelectedWayPoint);
            }


        }
    }
}

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Windows
{
    public partial class Form1 : Form
    {
        private BindingList<WayPoints> wayPoints;
        private BindingSource wayPointsBindingSource;

        private const string DataFilePath = "wayPointsData.dat";

        public Form1()
        {
            InitializeComponent();
            LoadData(); // Load data from the file

            // Initialize the BindingList if it's null (first run)
            if (wayPoints == null)
            {
                wayPoints = new BindingList<WayPoints>
                {
                    new WayPoints(40.7128, -74.0060), // New York City
                    new WayPoints(34.0522, -118.2437), // Los Angeles
                    new WayPoints(41.8781, -87.6298) // Chicago
                };
            }

            wayPointsBindingSource = new BindingSource(wayPoints, null);
            dataGridView1.DataSource = wayPointsBindingSource;

            // Optional
            dataGridView1.Columns["Id"].HeaderText = "ID";
            dataGridView1.Columns["Latitude"].HeaderText = "Latitude";
            dataGridView1.Columns["Longitude"].HeaderText = "Longitude";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Prompt the user to enter latitude and longitude
            double latitude, longitude;

            if (TryGetUserInput(out latitude, out longitude))
            {
                WayPoints newWayPoint = new WayPoints(latitude, longitude);
                wayPoints.Add(newWayPoint);

                // Save immediately after creating a new record
                SaveData();
            }
        }

        private bool TryGetUserInput(out double latitude, out double longitude)
        {
            // Prompt the user for latitude and longitude
            // You may use a dialog or other means to get the input
            // For simplicity, this example uses simple MessageBox prompts

            string latitudeInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Latitude:", "Latitude", "0.0");
            string longitudeInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Longitude:", "Longitude", "0.0");

            if (double.TryParse(latitudeInput, out latitude) && double.TryParse(longitudeInput, out longitude))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter valid numeric values for Latitude and Longitude.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;
            if (selectedRow != null)
            {
                WayPoints selectedWayPoint = (WayPoints)selectedRow.DataBoundItem;
                wayPoints.Remove(selectedWayPoint);

                // Save immediately after deleting a record
                SaveData();
            }
        }

        private void SaveData()
        {
            try
            {
                using (FileStream fileStream = new FileStream(DataFilePath, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, wayPoints);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            if (File.Exists(DataFilePath))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(DataFilePath, FileMode.Open))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        wayPoints = (BindingList<WayPoints>)formatter.Deserialize(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    wayPoints = null;
                }
            }
        }
    }
}

private void btnSaveAll_Click(object sender, EventArgs e)
{
    SaveData();
}

private void SaveData()
{
    try
    {
        using (FileStream fileStream = new FileStream(DataFilePath, FileMode.Create))
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, wayPoints);
        }

        MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void btnLoadData_Click(object sender, EventArgs e)
{
    LoadData();
}

private void LoadData()
{
    try
    {
        using (FileStream fileStream = new FileStream(DataFilePath, FileMode.Open))
        {
            IFormatter formatter = new BinaryFormatter();
            wayPoints = (BindingList<WayPoints>)formatter.Deserialize(fileStream);
        }

        wayPointsBindingSource.DataSource = wayPoints;
        wayPointsBindingSource.ResetBindings(false);

        MessageBox.Show("Data loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}



