// after DatabaseHelper.AddPatient returns newId
txtPatientID.Text = newId.ToString();           // internal id
txtPatientIDDisplay.Text = newId.ToString("D3"); // optional formatted display (001)