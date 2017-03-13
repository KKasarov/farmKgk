using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace farmKgk
{
    public partial class Form1 : Form
    {
       // List<Sheep> sheeps = new List<Sheep>();
        List<Sheep> showOnlySheeps = new List<Sheep>();
        List<string> sheepsStringLs = new List<string>();
        List<Lamb> showOnlyLambs = new List<Lamb>();

        List<string> ages = new List<string>();

        internal List<Sheep> ShowAllSheepsNew { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNewSheep_Click(object sender, EventArgs e)
        {

            string sexSheep;
            using (var db = new farmDbContext())
            {
                if (TextBoxAddNewSheepSerialNumber.Text == "")
                {
                    var mbox = MessageBox.Show($"Не можете да добавяте животно без сериен номер!",
                    "Неуспешно добавяне!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (!db.Sheeps.Select(a => a.SerialNumber).Contains(TextBoxAddNewSheepSerialNumber.Text))
                {

                    var mbox = MessageBox.Show($"\nСигурни ли сте, че искате да добавите ново \nживотно със сериен номер: {TextBoxAddNewSheepSerialNumber.Text} ?",
                        "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.None);

                    if (mbox == DialogResult.Yes)
                    {

                        sexSheep = "Female";

                        if (radioButtonAddSheepMale.Checked)
                        {
                            sexSheep = "Male";
                        }

                        Sheep newSheep = new Sheep(TextBoxAddNewSheepSerialNumber.Text, sexSheep, textBoxAddSheepAge.Text, richTextBoxAddSheepInfo.Text);

                        db.Sheeps.Add(newSheep);
                        db.SaveChanges();

                        TextBoxAddNewSheepSerialNumber.Text = "";
                        textBoxAddSheepAge.Text = "";
                        richTextBoxAddSheepInfo.Text = "";
                        radioButtonAddSheepFemale.Checked = true;
                    }
                }
                else
                {
                    var mbox = MessageBox.Show($"Вече има животно с сериен номер: {TextBoxAddNewSheepSerialNumber.Text}",
                        "Неуспешно добавяне!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new farmDbContext())
            {
                List<Sheep> sheepsForDb = new List<Sheep>();
                List<string> strSheeps = new List<string>();
                sheepsForDb.AddRange(db.Sheeps.ToList());

                foreach (var sh in sheepsForDb)
                {
                    strSheeps.Add($"{sh.StringLine()}");
                }

                richTextBox2.Text = (string.Join("\n", strSheeps));
            }
        }  

        private void btnAllSheepsInfo_Click(object sender, EventArgs e)
        {
            List<string> serNum = new List<string>();

            using (var db = new farmDbContext())
            {
                foreach (var sh in db.Sheeps.Select(a => a.SerialNumber))
                {
                    serNum.Add(sh.ToString());
                }

                if (!serNum.Contains(textBoxAllSheepsSN.Text))
                {

                    var mbox = MessageBox.Show("Не е открито животно с такъв сериен номер в списака!\nИскаш ли да добавиш ново животно?",
                        "Ops!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (mbox == DialogResult.Yes)
                    {
                        tabControlFirst.SelectedIndex = 0;
                        tabControlAddNew.SelectedIndex = 0;
                        TextBoxAddNewSheepSerialNumber.Text = textBoxAllSheepsSN.Text;
                    }

                }
                else
                {
                    btnShowAllEdit.Visible = true;
                    btnRemoveSheep.Visible = true;
                    //hide labels for sheep info
                    LabelSortDate.Visible = false;
                    LabelSortSex.Visible = false;
                    labelShowAllSheepsSN.Visible = false;
                    labelShowAllAgeSheeps.Visible = false;
                    

                    foreach (var sh in db.Sheeps.ToList())
                    {
                        if (sh.SerialNumber.ToString() == textBoxAllSheepsSN.Text.ToString())
                        {
                            richTextAllSheeps.Text = ($"Дата: {sh.Date}  \nГодини: {sh.Age}  \nПол: {sh.Sex} \nСериен номер: {sh.SerialNumber} \n info: {sh.Info}");
                        }
                    }

                }
            }
        }

        private void btnShowAllEdit_Click(object sender, EventArgs e)
        {
            EditAnimalTabStartVisibale();

            tabControlFirst.SelectedIndex = 1;
            textBoxEditSN.Text = textBoxAllSheepsSN.Text;
        }

        private void labelShowAllSheepsSN_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxAllSheepsData_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAllSheepsData.Checked == true)
            {
                dateTimePickerForCheckBoxData.Enabled = true;
                checkBoxAllSheepsData.ForeColor = Color.Black;
                btnAllSheepsAdvView.ForeColor = Color.Black;
                btnAllSheepsAdvView.Text = "Покажи >>";
            }
            else
            {
                dateTimePickerForCheckBoxData.Enabled = false;
                checkBoxAllSheepsData.ForeColor = Color.DarkSlateGray;

                if (checkBoxAllSheepsAge.Checked == false
                    && checkBoxAllSheepsSex.Checked == false)
                {
                    btnAllSheepsAdvView.Text = "Покажи всички >>";
                }
            }
        }

        private void checkBoxAllSheepsAge_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAllSheepsAge.Checked == true)
            {
                textBoxAllSheepsForCheckBoxAge.Enabled = true;
                checkBoxAllSheepsAge.ForeColor = Color.Black;
                btnAllSheepsAdvView.ForeColor = Color.Black;
                btnAllSheepsAdvView.Text = "Покажи >>";
            }
            else
            {
                textBoxAllSheepsForCheckBoxAge.Enabled = false;
                checkBoxAllSheepsAge.ForeColor = Color.DarkSlateGray;

                if (checkBoxAllSheepsData.Checked == false
                   && checkBoxAllSheepsSex.Checked == false)
                {
                    btnAllSheepsAdvView.Text = "Покажи всички >>";
                }
            }
        }

        private void checkBoxAllSheepsSex_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAllSheepsSex.Checked == true)
            {
                radioButtonAllSheepsForCheckBoxSexMale.Enabled = true;
                radioButtonAllSheepsForChekBoxSexFemale.Enabled = true;
                radioButtonAllSheepsForCheckBoxSexMale.ForeColor = Color.Black;
                radioButtonAllSheepsForChekBoxSexFemale.ForeColor = Color.Black;
                checkBoxAllSheepsSex.ForeColor = Color.Black;
                btnAllSheepsAdvView.ForeColor = Color.Black;
                btnAllSheepsAdvView.Text = "Покажи >>";
            }
            else
            {
                radioButtonAllSheepsForCheckBoxSexMale.Enabled = false;
                radioButtonAllSheepsForChekBoxSexFemale.Enabled = false;
                radioButtonAllSheepsForCheckBoxSexMale.ForeColor = Color.DarkSlateGray;
                radioButtonAllSheepsForChekBoxSexFemale.ForeColor = Color.DarkSlateGray;
                checkBoxAllSheepsSex.ForeColor = Color.DarkSlateGray;

                if (checkBoxAllSheepsAge.Checked == false
                   && checkBoxAllSheepsData.Checked == false)
                {
                    btnAllSheepsAdvView.Text = "Покажи всички >>";
                }
            }
        }

        private void btnAllSheepsAdvView_Click(object sender, EventArgs e)
        {
            btnShowAllEdit.Visible = false;
            btnRemoveSheep.Visible = false;

            LabelSortDate.Visible = true;
            LabelSortSex.Visible = true;
            labelShowAllSheepsSN.Visible = true;
            labelShowAllAgeSheeps.Visible = true;

            List<string> showOnlyAge = new List<string>();
            List<Sheep> showOnlySheepsNew = new List<Sheep>();

            if (btnAllSheepsAdvView.Text == "Покажи всички >>")
            {              
                using (var db = new farmDbContext())
                {
                    List<string> sheepsStr = new List<string>();

                    foreach (var sh in db.Sheeps.ToList())
                    {
                        showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                    }
                    ages = showOnlyAge;
                    showOnlySheeps = showOnlySheepsNew;
                    sheepsStr.Reverse();
                    richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                }
            }
            else
            {
                if (checkBoxAllSheepsData.Checked == true
                && checkBoxAllSheepsAge.Checked == true
                && checkBoxAllSheepsSex.Checked == true)
                {
                    if (radioButtonAllSheepsForCheckBoxSexMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString())
                                    && a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (radioButtonAllSheepsForChekBoxSexFemale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString())
                                    && a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                else if (checkBoxAllSheepsData.Checked == true
                     && checkBoxAllSheepsAge.Checked == true)
                {
                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Sheeps
                            .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString())
                                && a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString()))
                            .ToList())
                        {
                            showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlySheeps = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (checkBoxAllSheepsData.Checked == true
                      && checkBoxAllSheepsSex.Checked == true)
                {
                    if (radioButtonAllSheepsForCheckBoxSexMale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (radioButtonAllSheepsForChekBoxSexFemale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                else if (checkBoxAllSheepsData.Checked == true)
                {
                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Sheeps
                            .Where(a => a.Date.Equals(dateTimePickerForCheckBoxData.Text.ToString()))
                            .ToList())
                        {
                            showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlySheeps = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (checkBoxAllSheepsAge.Checked == true
                     && checkBoxAllSheepsSex.Checked == true)
                {
                    if (radioButtonAllSheepsForCheckBoxSexMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }

                    else if (radioButtonAllSheepsForChekBoxSexFemale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                else if (checkBoxAllSheepsAge.Checked == true
                    && checkBoxAllSheepsSex.Checked == false)
                {

                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Sheeps
                            .Where(a => a.Age.Equals(textBoxAllSheepsForCheckBoxAge.Text.ToString()))
                            .ToList())
                        {
                            showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlySheeps = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (checkBoxAllSheepsSex.Checked == true)
                {
                    if (radioButtonAllSheepsForCheckBoxSexMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (radioButtonAllSheepsForChekBoxSexFemale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {

                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Sheeps
                                .Where(a => a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsSheep(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlySheeps = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            richTextAllSheeps.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                showOnlySheeps = showOnlySheepsNew;
            }
        }

        private static void showListsSheep(List<string> showOnlyAge, List<Sheep> showOnlySheepsNew, List<string> sheepsStr, Sheep sh)
        {
            sheepsStr.Add($"{sh.StringLine()}");
            showOnlyAge.Add(sh.Age.ToString());
            showOnlySheepsNew.Add(sh);
        }

        private void LabelSortDate_Click(object sender, EventArgs e)
        {
            List<string> sheepsOnlyStringLs = new List<string>();

            foreach (var sh in showOnlySheeps)
            {
                sheepsOnlyStringLs.Add(sh.StringLine());
            }

            sheepsStringLs = sheepsOnlyStringLs;
            richTextAllSheeps.Text = string.Join("\n", sheepsOnlyStringLs);
        }

        private void labelShowAllAgeSheeps_Click_1(object sender, EventArgs e)
        {
            List<string> sheepsOnlyStringLs = new List<string>();

            ages.Sort();

            foreach (var age in ages)
            {
                foreach (var sh in showOnlySheeps.Where(a => a.Age == age))
                {
                    if (!sheepsOnlyStringLs.Contains(sh.StringLine()))
                    {
                        sheepsOnlyStringLs.Add(sh.StringLine());
                    }
                }
            }

            sheepsStringLs = sheepsOnlyStringLs;
            richTextAllSheeps.Text = string.Join("\n", sheepsOnlyStringLs);
        }

        private void LabelSortSex_Click_1(object sender, EventArgs e)
        {
            List<string> sheepsOnlyStringLs = new List<string>();
            foreach (var sh in showOnlySheeps)
            {
                if (sh.Sex == "Female")
                {
                    sheepsOnlyStringLs.Add(sh.StringLine());
                }
            }
            foreach (var sh in showOnlySheeps)
            {
                if (sh.Sex != "Female")
                {
                    sheepsOnlyStringLs.Add(sh.StringLine());
                }
            }
            sheepsStringLs = sheepsOnlyStringLs;
            richTextAllSheeps.Text = string.Join("\n", sheepsOnlyStringLs);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btnAllSheepsRevers_Click(object sender, EventArgs e)
        {
            sheepsStringLs.Reverse();
            richTextAllSheeps.Text = string.Join("\n", sheepsStringLs);
        }

        private void btnEditOk_Click(object sender, EventArgs e)
        {

            using (farmDbContext db = new farmDbContext())
            {
                if (!db.Sheeps.Select(a => a.SerialNumber).Contains(textBoxEditSN.Text))
                {
                    if (db.Lambs.Select(a => a.SerialNumber).Contains(textBoxEditSN.Text))
                    {
                        lblEditThisAnimal.Text = "Агне";

                        EditOkBtnVisible();

                        var lb = db.Lambs.First(a => a.SerialNumber.Equals(textBoxEditSN.Text));

                        textBoxEditAge.Text = lb.Age.ToString();

                        if (lb.Sex == "Female")
                        {
                            radioButtonEditFamale.Checked = true;
                        }
                        else
                        {
                            radioButtonEditMale.Checked = true;
                        }

                        richTextBoxEditInfo.Text = lb.Info.ToString();
                    }
                    else
                    {
                        var mbox = MessageBox.Show("Не е открито животно с такъв сериен номер в списака!\nИскаш ли да добавиш ново животно?",
                             "Ops!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (mbox == DialogResult.Yes)
                        {
                            tabControlFirst.SelectedIndex = 0;
                            tabControlAddNew.SelectedIndex = 0;
                            TextBoxAddNewSheepSerialNumber.Text = textBoxEditSN.Text;
                        }
                    }
                }

                else
                {
                    lblEditThisAnimal.Text = "Овца";

                    EditOkBtnVisible();

                    var sh = db.Sheeps.First(a => a.SerialNumber.Equals(textBoxEditSN.Text));

                    textBoxEditAge.Text = sh.Age.ToString();

                    if (sh.Sex == "Female")
                    {
                        radioButtonEditFamale.Checked = true;
                    }
                    else
                    {
                        radioButtonEditMale.Checked = true;
                    }

                    richTextBoxEditInfo.Text = sh.Info.ToString();
                }
            }
        }

        private void EditOkBtnVisible()
        {
            lblEditThisAnimal.Visible = true;
            btnEditOk.Visible = false;
            lblEditSex.Visible = true;
            radioButtonEditMale.Visible = true;
            radioButtonEditFamale.Visible = true;
            labelEditAge.Visible = true;
            textBoxEditAge.Visible = true;
            labelEditInfo.Visible = true;
            richTextBoxEditInfo.Visible = true;
            btnEditEdit.Visible = true;
            btnEditBack.Visible = true;

            lblEditSNInfo.Visible = false;
            textBoxEditSN.Location = new Point(108, 83);
            textBoxEditSN.Width = 167;
            labelEditId.Location = new Point(68, 83);
            labelEditId.Text = "С/н:";
        }

        private void btnEditEdit_Click(object sender, EventArgs e)
        {
            EditAnimalTabStartVisibale();

            using (farmDbContext db = new farmDbContext())
            {
                if (db.Sheeps.Select(a => a.SerialNumber).Contains(textBoxEditSN.Text))
                {
                    var sh = db.Sheeps.First(a => a.SerialNumber.Equals(textBoxEditSN.Text));

                    if (!sh.Age.Equals(textBoxEditAge.Text.ToString())
                        || !sh.Info.Equals(richTextBoxEditInfo.Text.ToString()))
                    {
                        sh.Age = textBoxEditAge.Text;
                        sh.Info = richTextBoxEditInfo.Text;
                    }

                    if (radioButtonEditFamale.Checked == true)
                    {
                        if (!sh.Sex.ToString().Equals("Female"))
                        {
                            sh.Sex = "Female";
                        }
                    }
                    else
                    {
                        sh.Sex = "Male";
                    }

                    db.SaveChanges();
                }
                //edit lambs
                else
                {
                    var lmb = db.Lambs.First(a => a.SerialNumber.Equals(textBoxEditSN.Text));

                    if (!lmb.Age.Equals(textBoxEditAge.Text.ToString())
                        || !lmb.Info.Equals(richTextBoxEditInfo.Text.ToString()))
                    {
                        lmb.Age = textBoxEditAge.Text;
                        lmb.Info = richTextBoxEditInfo.Text;
                    }

                    if (radioButtonEditFamale.Checked == true)
                    {
                        if (!lmb.Sex.ToString().Equals("Female"))
                        {
                            lmb.Sex = "Female";
                        }
                    }
                    else
                    {
                        lmb.Sex = "Male";
                    }

                    db.SaveChanges();
                }
            }
            textBoxEditSN.Text = "";
        }

        private void btnEditBack_Click(object sender, EventArgs e)
        {
            EditAnimalTabStartVisibale();
        }

        private void EditAnimalTabStartVisibale()
        {
            lblEditThisAnimal.Visible = false;
            btnEditOk.Visible = true;
            lblEditSex.Visible = false;
            radioButtonEditMale.Visible = false;
            radioButtonEditFamale.Visible = false;
            labelEditAge.Visible = false;
            textBoxEditAge.Visible = false;
            labelEditInfo.Visible = false;
            richTextBoxEditInfo.Visible = false;
            btnEditEdit.Visible = false;
            btnEditBack.Visible = false;
            labelEditId.Text = "Сериен номер:";

            textBoxEditSN.Location = new Point(215, 148);
            labelEditId.Location = new Point(91, 147);
            btnEditOk.Visible = true;
            lblEditSNInfo.Visible = true;
        }

        private void btnAddNewLamb_Click(object sender, EventArgs e)
        {
            string sexLamb;

            using (var db = new farmDbContext())
            {
                
                if (txtAddNewLambSN.Text == "")
                {
                    var mbox = MessageBox.Show($"Не можете да добавяте животно без сериен номер!",
                    "Неуспешно добавяне!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                else if (!db.Lambs.Select(a => a.SerialNumber).Contains(txtAddNewLambSN.Text))
                {

                    var mbox = MessageBox.Show($"\nСигурни ли сте, че искате да добавите ново \nживотно със сериен номер: {txtAddNewLambSN.Text} ?",
                        "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.None);

                    if (mbox == DialogResult.Yes)
                    {
                        sexLamb = "Female";

                        if (rBAddNewLambMale.Checked)
                        {
                            sexLamb = rBAddNewLambMale.Text;
                        }

                        Lamb newLamb = new Lamb(txtAddNewLambSN.Text.ToString(), sexLamb, txtAddNewLambAge.Text.ToString(), rTxtAddNewLambInfo.Text.ToString());

                        txtAddNewLambSN.Text = "";
                        rBAddNewLambFemale.Checked = true;
                        txtAddNewLambAge.Text = "0";
                        rTxtAddNewLambInfo.Text = "";

                        db.Lambs.Add(newLamb);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var mbox = MessageBox.Show($"Вече има животно с сериен номер: {txtAddNewLambSN.Text}",
                        "Неуспешно добавяне!", MessageBoxButtons.OK, MessageBoxIcon.Stop);                   
                }
            }
        }

        private void btnShowAllAdvViewLambs_Click(object sender, EventArgs e)
        {            
            btnRemoveLamb.Visible = false;
            btnShowAllLambsEdit.Visible = false;

            lblShowAllLambsSortData.Visible = true;
            lblShowAllLambsSortSex.Visible = true;
            lblShowAllLambsSortSN.Visible = true;
            lblShowAllLambsSortAge.Visible = true;

            List<string> showOnlyAge = new List<string>();
            List<Lamb> showOnlySheepsNew = new List<Lamb>();

            if (btnShowAllAdvViewLambs.Text == "Покажи всички >>")
            {
                using (var db = new farmDbContext())
                {
                    List<string> sheepsStr = new List<string>();

                    foreach (var sh in db.Lambs.ToList())
                    {
                        showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                    }
                    ages = showOnlyAge;
                    showOnlyLambs = showOnlySheepsNew;

                    rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                }
            }
            else
            {               
                if (chBShowLambsData.Checked == true
               && chBShowLambsAge.Checked == true
               && chBShowLambsSex.Checked == true)
                {
                    if (rBShowAllLambsMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString())
                                    && a.Age.Equals(txtShowAllLambsAge.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (rBShowAllLambsFemale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString())
                                    && a.Age.Equals(txtShowAllLambsAge.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }
                else if (chBShowLambsData.Checked == true
                     && chBShowLambsAge.Checked == true)
                {
                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Lambs
                            .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString())
                                && a.Age.Equals(txtShowAllLambsAge.Text.ToString()))
                            .ToList())
                        {
                            showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlyLambs = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (chBShowLambsData.Checked == true
                      && chBShowLambsSex.Checked == true)
                {
                    if (rBShowAllLambsMale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }

                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (rBShowAllLambsFemale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                else if (chBShowLambsData.Checked == true)
                {
                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Lambs
                            .Where(a => a.Date.Equals(dateTimePickerShowAllLambs.Text.ToString()))
                            .ToList())
                        {
                            showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlyLambs = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (chBShowLambsAge.Checked == true
                     && chBShowLambsSex.Checked == true)
                {
                    if (rBShowAllLambsMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Age.Equals(txtShowAllLambsAge.Text.ToString())
                                    && !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }

                    else if (rBShowAllLambsFemale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Age.Equals(txtShowAllLambsAge.Text.ToString())
                                    && a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }

                else if (chBShowLambsAge.Checked == true
                    && chBShowLambsSex.Checked == false)
                {

                    using (var db = new farmDbContext())
                    {
                        List<string> sheepsStr = new List<string>();

                        foreach (var sh in db
                            .Lambs
                            .Where(a => a.Age.Equals(txtShowAllLambsAge.Text.ToString()))
                            .ToList())
                        {
                            showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                        }
                        ages = showOnlyAge;
                        showOnlyLambs = showOnlySheepsNew;

                        sheepsStr.Reverse();
                        rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                    }
                }

                else if (chBShowLambsSex.Checked == true)
                {
                    if (rBShowAllLambsMale.Checked)
                    {

                        using (var db = new farmDbContext())
                        {
                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => !a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                    else if (rBShowAllLambsFemale.Checked)
                    {
                        using (var db = new farmDbContext())
                        {

                            List<string> sheepsStr = new List<string>();

                            foreach (var sh in db
                                .Lambs
                                .Where(a => a.Sex.Equals("Female"))
                                .ToList())
                            {
                                showListsLamb(showOnlyAge, showOnlySheepsNew, sheepsStr, sh);
                            }
                            ages = showOnlyAge;
                            showOnlyLambs = showOnlySheepsNew;

                            sheepsStr.Reverse();
                            rTextShowAllLambs.Text = (string.Join("\n", sheepsStr));
                        }
                    }
                }
            }
        }

        private static void showListsLamb(List<string> showOnlyAge, List<Lamb> showOnlySheepsNew, List<string> sheepsStr, Lamb sh)
        {
            sheepsStr.Add($"{sh.StringLine()}");
            showOnlyAge.Add(sh.Age.ToString());
            showOnlySheepsNew.Add(sh);
        }

        private void chBShowLambsData_CheckedChanged(object sender, EventArgs e)
        {
            if (chBShowLambsData.Checked == true)
            {
                dateTimePickerShowAllLambs.Enabled = true;
                chBShowLambsData.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.Text = "Покажи >>";
            }
            else
            {
                dateTimePickerShowAllLambs.Enabled = false;
                chBShowLambsData.ForeColor = Color.DarkSlateGray;

                if (chBShowLambsAge.Checked == false
                    && chBShowLambsSex.Checked == false)
                {
                    btnShowAllAdvViewLambs.Text = "Покажи всички >>";
                }
            }
        }

        private void chBShowLambsAge_CheckedChanged(object sender, EventArgs e)
        {
            if (chBShowLambsAge.Checked == true)
            {
                txtShowAllLambsAge.Enabled = true;
                chBShowLambsAge.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.Text = "Покажи >>";
            }
            else
            {
                txtShowAllLambsAge.Enabled = false;
                chBShowLambsAge.ForeColor = Color.DarkSlateGray;

                if (chBShowLambsData.Checked == false
                   && chBShowLambsSex.Checked == false)
                {
                    btnShowAllAdvViewLambs.Text = "Покажи всички >>";
                }
            }
        }

        private void chBShowLambsSex_CheckedChanged(object sender, EventArgs e)
        {
            if (chBShowLambsSex.Checked == true)
            {
                rBShowAllLambsMale.Enabled = true;
                rBShowAllLambsFemale.Enabled = true;
                rBShowAllLambsMale.ForeColor = Color.Black;
                rBShowAllLambsFemale.ForeColor = Color.Black;
                chBShowLambsSex.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.ForeColor = Color.Black;
                btnShowAllAdvViewLambs.Text = "Покажи >>";
            }
            else
            {
                rBShowAllLambsMale.Enabled = false;
                rBShowAllLambsFemale.Enabled = false;
                rBShowAllLambsMale.ForeColor = Color.DarkSlateGray;
                rBShowAllLambsFemale.ForeColor = Color.DarkSlateGray;
                chBShowLambsSex.ForeColor = Color.DarkSlateGray;

                if (chBShowLambsAge.Checked == false
                   && chBShowLambsData.Checked == false)
                {
                    btnShowAllAdvViewLambs.Text = "Покажи всички >>";
                }
            }
        }

        private void btnShowAllLamsInfo_Click(object sender, EventArgs e)
        {
            List<string> serNum = new List<string>();
            using (var db = new farmDbContext())
            {
                foreach (var sh in db.Lambs.Select(a => a.SerialNumber))
                {
                    serNum.Add(sh.ToString());
                }

                if (!serNum.Contains(txtShowAllLamsSN.Text.ToString()))
                {
                    var mbox = MessageBox.Show("Не е открито животно с такъв сериен номер в списака!\nИскаш ли да добавиш ново животно?",
                        "Ops!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (mbox == DialogResult.Yes)
                    {
                        tabControlFirst.SelectedIndex = 0;
                        tabControlAddNew.SelectedIndex = 1;
                        TextBoxAddNewSheepSerialNumber.Text = textBoxAllSheepsSN.Text;
                    }
                }
                else
                {
                    btnRemoveLamb.Visible = true;
                    btnShowAllLambsEdit.Visible = true;
                    lblShowAllLambsSortData.Visible = false;
                    lblShowAllLambsSortAge.Visible = false;
                    lblShowAllLambsSortSN.Visible = false;
                    lblShowAllLambsSortSex.Visible = false;

                    foreach (var sh in db.Lambs.ToList())
                    {
                        if (sh.SerialNumber.ToString() == txtShowAllLamsSN.Text.ToString())
                        {
                            rTextShowAllLambs.Text = ($"Дата: {sh.Date}  \nГодини: {sh.Age}  \nПол: {sh.Sex} \nСериен номер: {sh.SerialNumber} \n info: {sh.Info}");
                        }
                    }
                }
            }
        }

        private void btnRemoveSheep_Click(object sender, EventArgs e)
        {
            using (farmDbContext db = new farmDbContext())
            {
                var sh = db.Sheeps.First(a => a.SerialNumber == textBoxAllSheepsSN.Text);
                db.Sheeps.Remove(sh);
                db.SaveChanges();

                richTextAllSheeps.Text = $"Успешно изтрихте животно {sh.SerialNumber}";
                btnRemoveSheep.Visible = false;
                btnShowAllEdit.Visible = false;
                textBoxAllSheepsSN.Text = "";
            }

        }

        private void lblShowAllLambsSortData_Click(object sender, EventArgs e)
        {
            List<string> LambsOnlyStringLs = new List<string>();

            foreach (var sh in showOnlyLambs)
            {
                LambsOnlyStringLs.Add(sh.StringLine());
            }

            sheepsStringLs = LambsOnlyStringLs;
            rTextShowAllLambs.Text = string.Join("\n", LambsOnlyStringLs);
        }

        private void lblShowAllLambsSortAge_Click(object sender, EventArgs e)
        {
            List<string> lambsOnlyStringLs = new List<string>();

            ages.Sort();

            foreach (var age in ages)
            {
                foreach (var sh in showOnlyLambs.Where(a => a.Age==age))
                {
                    if (!lambsOnlyStringLs.Contains(sh.StringLine()))
                    {
                        lambsOnlyStringLs.Add(sh.StringLine());
                    }                 
                }
            }

            sheepsStringLs = lambsOnlyStringLs;
            rTextShowAllLambs.Text = string.Join("\n", sheepsStringLs);
        }

        private void lblShowAllLambsSortSex_Click(object sender, EventArgs e)
        {
            List<string> lambsOnlyStringLs = new List<string>();
            foreach (var sh in showOnlyLambs)
            {
                if (sh.Sex == "Female")
                {
                    lambsOnlyStringLs.Add(sh.StringLine());
                }
            }
            foreach (var sh in showOnlyLambs)
            {
                if (sh.Sex != "Female")
                {
                    lambsOnlyStringLs.Add(sh.StringLine());
                }
            }
            sheepsStringLs = lambsOnlyStringLs;
            rTextShowAllLambs.Text = string.Join("\n", lambsOnlyStringLs);
        }

        private void btnShowAllLambsEdit_Click(object sender, EventArgs e)
        {
            lblEditThisAnimal.Visible = false;
            btnEditOk.Visible = true;
            lblEditSex.Visible = false;
            radioButtonEditMale.Visible = false;
            radioButtonEditFamale.Visible = false;
            labelEditAge.Visible = false;
            textBoxEditAge.Visible = false;
            labelEditInfo.Visible = false;
            richTextBoxEditInfo.Visible = false;
            btnEditEdit.Visible = false;
            btnEditBack.Visible = false;

            textBoxEditSN.Location = new Point(215, 148);
            labelEditId.Location = new Point(91, 147);
            labelEditId.Text = "Сериен номер:";
            btnEditOk.Visible = true;
            lblEditSNInfo.Visible = true;

            tabControlFirst.SelectedIndex = 1;
            textBoxEditSN.Text = txtShowAllLamsSN.Text;
        }

        private void btnRemoveLamb_Click(object sender, EventArgs e)
        {
            using (farmDbContext db = new farmDbContext())
            {
                var sh = db.Lambs.First(a => a.SerialNumber == txtShowAllLamsSN.Text);
                db.Lambs.Remove(sh);
                db.SaveChanges();

                rTextShowAllLambs.Text = $"Успешно изтрихте животно {sh.SerialNumber}";
                btnRemoveLamb.Visible = false;
                btnShowAllLambsEdit.Visible = false;
                txtShowAllLamsSN.Text = "";
            }
        }
    }
}
